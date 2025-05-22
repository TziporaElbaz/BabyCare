using Project.BL.API;
using Project.DAL.API;
using Project.Models;
using System.Globalization;
using System.Text.Json;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Configuration;
using Project.DAL.Models;

namespace Project.BL.Services
{
    public class AvailableAppointmentsManagementBL : IAvailableAppointmentsManagementBL
    {
        private static readonly string _baseUrl = "https://www.hebcal.com/hebcal";
        private readonly IWorkerShiftManagementDAL shiftWorkerManager;
        private readonly IShiftManagementDAL shiftManager;
        private readonly IConfiguration _configuration;

        public AvailableAppointmentsManagementBL(IWorkerShiftManagementDAL _shiftWorkerManagement, IShiftManagementDAL _shiftManager, IConfiguration configuration)
        {
            shiftWorkerManager = _shiftWorkerManagement;
            shiftManager = _shiftManager;
            _configuration = configuration;
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

        public void AddAvailableAppointmentsToWorkers(DateTime date)
        {
            List<Shift> shifts = shiftManager.GetShiftsByDayAsync((int)date.DayOfWeek).Result;
            var workerTypes = _configuration.GetSection("WorkerAppointmentDuration").GetChildren()
                                 .Select(x => x.Key)
                                 .ToList();
            if (!IsHoliday(date))
            {
                foreach (Shift shift in shifts)
                {
                    List<Worker> workers = shiftWorkerManager.GetWorkersByShiftID(shift.Id).Result;
                    foreach (Worker worker in workers)
                    {
                        if (!workerTypes.Contains(worker.WorkerType))
                        {
                            throw new Exception("Invalid worker");
                        }
                        //int appointmentDuration1 = _configuration.GetSection("WorkerAppointmentDuration")
                        //                         .GetValue<int>(worker.WorkerType);

                        var appointmentDurations = _configuration[$"WorkerAppointmentDuration:{worker.WorkerType}"];
                        if (int.TryParse(appointmentDurations, out int appointmentDuration))
                        {

                            for (TimeOnly time = shift.StartTime; time <= shift.EndTime; time = time.AddMinutes(appointmentDuration))
                        {
                            //worker.AvailableAppointments.Add(new AvailableAppointment
                            //{
                            //    WorkerId = worker.Id,
                            //    AppointmentDate = DateOnly.FromDateTime(DateTime.Now),
                            //    StartTime = time,
                            //    EndTime = time.AddMinutes(appointmentDuration)
                            //});

                            //var appointmentService = new AppointmentService(new YourDbContext());
                            //appointmentService.SaveAvailableAppointments(availableAppointments);
                        }
                    }
                }
            }
        }

    }
}
}


