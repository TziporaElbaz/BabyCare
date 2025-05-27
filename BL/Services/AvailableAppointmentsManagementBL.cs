using WEB_API.BL.API;
using WEB_API.DAL.API;
using WEB_API.Models;
using System.Globalization;
using System.Text.Json;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Configuration;
using WEB_API.DAL.Models;
using BL.Models;
using AutoMapper;
using DAL.API;

namespace WEB_API.BL.Services
{
    public class AvailableAppointmentsManagementBL : IAvailableAppointmentsManagementBL
    {
        private static readonly string _baseUrl = "https://www.hebcal.com/hebcal";
        private readonly IWorkerShiftManagementDAL shiftWorkerManager;
        private readonly IShiftManagementDAL shiftManager;
        private readonly IConfiguration configuration;
        private readonly IMapper mapper;
        private readonly IAvailableAppointmentManagementDAL dal;

        public AvailableAppointmentsManagementBL(IWorkerShiftManagementDAL _shiftWorkerManagement, IShiftManagementDAL _shiftManager, IConfiguration _configuration, IMapper _mapper, IAvailableAppointmentManagementDAL _dal)
        {
            shiftWorkerManager = _shiftWorkerManagement;
            shiftManager = _shiftManager;
            configuration = _configuration;
            mapper = _mapper;
            dal = _dal;
        }
        //public void GenerateAvailableAppointments(DateTime startDate, DateTime endDate)
        //{

        //    for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
        //    {
        //        //// בדיקה אם זה שבת
        //        //if (date.DayOfWeek == DayOfWeek.Saturday|| date.DayOfWeek == DayOfWeek.Friday)
        //        //    continue;

        //        if (!isHoliday(date))
        //            BuildAppointments(date);
        //    }

        //}
        //    public async Task a() { 
        //    DateTime date = new DateTime(2025, 5, 1);
        //    bool isHoliday = await IsHolidayAsync(date);

        //    Console.WriteLine(isHoliday
        //        ? "is holiday!"
        //        : "is not holiday.");
        //}

        public bool IsHoliday(DateTime date)
        {
            string year = date.Year.ToString();
            string month = date.Month.ToString("D2");
            string day = date.Day.ToString("D2");

            var url = $"{_baseUrl}?v=1&cfg=json&year={year}&month={month}&maj=on&min=off&mod=on&nx=off";

            using (HttpClient client = new HttpClient())
            {
                var response = client.GetAsync(url).Result;

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = response.Content.ReadAsStringAsync().Result;
                    var json = JObject.Parse(jsonString);

                    var items = json["items"];
                    Console.WriteLine(items);
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
                                    /* && item["subcat"].Equals("modern")*/
                                    Console.WriteLine(item["title"]);
                                    Console.WriteLine(item["subcat"]);

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
        }

        public async Task AddAvailableAppointmentsToWorkers(DateTime date)
        {
            List<Shift> shifts = await shiftManager.GetShiftsByDayAsync((int)date.DayOfWeek);
            var workerTypes = configuration.GetSection("WorkerAppointmentDuration").GetChildren()
                                 .Select(x => x.Key)
                                 .ToList();
            if (!IsHoliday(date))
            {
                foreach (Shift shift in shifts)
                {
                    List<Worker> workers = await shiftWorkerManager.GetWorkersByShiftID(shift.Id);
                    foreach (Worker worker in workers)
                    {
                        if (!workerTypes.Contains(worker.WorkerType))
                        {
                            throw new Exception("Invalid worker");
                        }

                        var appointmentDurations = configuration[$"WorkerAppointmentDuration:{worker.WorkerType}"];
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

                                var appointmentEntity = mapper.Map<AvailableAppointment>(appointment);

                                await dal.AddAvailableAppointmentAsync(appointmentEntity);
                            }
                        }
                    }
                }
            }
        }
    }
}



