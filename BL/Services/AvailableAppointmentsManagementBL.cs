using System.Globalization;
using System.Text.Json;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Configuration;
using WEB_API.BL.API;
using WEB_API.DAL.API;
using WEB_API.DAL.Models;
using WEB_API.BL.Models;
using AutoMapper;


namespace WEB_API.BL.Services
{
    public class AvailableAppointmentsManagementBL : IAvailableAppointmentsManagementBL
    {
        private static readonly string _baseUrl = "https://www.hebcal.com/hebcal";
        private readonly IWorkerShiftManagementDAL _shiftWorkerManager;
        private readonly IShiftManagementDAL _shiftManager;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IAvailableAppointmentManagementDAL _dal;
        private readonly HttpClient _httpClient; 

        public AvailableAppointmentsManagementBL(
            IWorkerShiftManagementDAL shiftWorkerManager,
            IShiftManagementDAL shiftManager,
            IConfiguration configuration,
            IMapper mapper,
            IAvailableAppointmentManagementDAL dal,
            HttpClient httpClient)
        {
            _shiftWorkerManager = shiftWorkerManager;
            _shiftManager = shiftManager;
            _configuration = configuration;
            _mapper = mapper;
            _dal = dal;
            _httpClient = httpClient; 
        }

        public async Task<bool> IsHoliday(DateTime date)
        {
            string year = date.Year.ToString();
            string month = date.Month.ToString("D2");
            string day = date.Day.ToString("D2");

            var url = $"{_baseUrl}?v=1&cfg=json&year={year}&month={month}&maj=on&min=off&mod=on&nx=off";

            var response = await _httpClient.GetAsync(url); // שימוש ב-HttpClient המוזרק

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync(); // await במקום .Result
                var json = JObject.Parse(jsonString);

                var items = json["items"];
                if (items != null)
                {
                    foreach (var item in items)
                    {
                        var holidayDate = DateTime.Parse(item["date"].ToString());
                        if (holidayDate.Date == date.Date)
                        {
                            if (item["title"].ToString().Equals("Yom HaAtzma'ut", StringComparison.OrdinalIgnoreCase))
                            {
                                return true;
                            }
                            else if (item["subcat"].ToString().Equals("modern"))
                            {
                                return false;
                            }
                            return true;
                        }
                    }
                }
                return false;
            }
            else
            {
                throw new Exception($"API Error: {response.StatusCode}");
            }
        }

        public async Task AddAvailableAppointmentsToWorkers(DateTime date)
        {
            List<Shift> shifts = await _shiftManager.GetShiftsByDayAsync((int)date.DayOfWeek);
            var workerTypes = _configuration.GetSection("WorkerAppointmentDuration").GetChildren()
                                 .Select(x => x.Key)
                                 .ToList();
            if (!await IsHoliday(date)) 
            {
                foreach (Shift shift in shifts)
                {
                    List<Worker> workers = await _shiftWorkerManager.GetWorkersByShiftID(shift.Id);
                    foreach (Worker worker in workers)
                    {
                        if (!workerTypes.Contains(worker.WorkerType))
                        {
                            throw new Exception("Invalid worker");
                        }

                        var appointmentDurations = _configuration[$"WorkerAppointmentDuration:{worker.WorkerType}"];
                        if (int.TryParse(appointmentDurations, out int appointmentDuration))
                        {
                            for (TimeOnly time = shift.StartTime; time <= shift.EndTime; time = time.AddMinutes(appointmentDuration))
                            {
                                AvailableAppointmentBL appointment = new AvailableAppointmentBL
                                {
                                    WorkerId = worker.Id,
                                    AppointmentDate = DateOnly.FromDateTime(date),
                                    StartTime = time,
                                    EndTime = time.AddMinutes(appointmentDuration)
                                };

                                var appointmentEntity = _mapper.Map<AvailableAppointment>(appointment);

                                await _dal.AddAvailableAppointmentAsync(appointmentEntity);
                            }
                        }
                    }
                }
            }
        }
    }
}