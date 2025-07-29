using AutoMapper;
using BL.API;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using WEB_API.BL.API;
using WEB_API.BL.Models;
using WEB_API.DAL.API;
using WEB_API.DAL.Models;
using WEB_API.DAL.Services;
using WEB_API.Services;

namespace WEB_API.BL.Services
{
    public class AvailableAppointmentsManagementBL : IAvailableAppointmentsManagementBL
    {
        private static readonly string _baseUrl = "https://www.hebcal.com/hebcal";
        private readonly IWorkerShiftManagementDAL _shiftWorkerManager;
        private readonly IShiftManagementDAL _shiftManager;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IAvailableAppointmentManagementDAL availableAppointmentManagementDAL;
        private readonly HttpClient _httpClient;
        private readonly IWorkersManagmentDAL _workersManagmentDAL;
        private readonly IVaccineManagementBL _vaccineManagementBL;
        private readonly IBabyManagementBL _babyManagementBL;

        public AvailableAppointmentsManagementBL(
            IWorkerShiftManagementDAL shiftWorkerManager,
            IShiftManagementDAL shiftManager,
            IConfiguration configuration,
            IMapper mapper,
            IAvailableAppointmentManagementDAL dal,
            IWorkersManagmentDAL workersManagmentDAL,
            IVaccineManagementBL vaccineManagementBL,
            IBabyManagementBL babyManagementBL,
            HttpClient httpClient)
        {
            _shiftWorkerManager = shiftWorkerManager;
            _shiftManager = shiftManager;
            _configuration = configuration;
            _mapper = mapper;
            availableAppointmentManagementDAL = dal;
            _httpClient = httpClient;
            _vaccineManagementBL = vaccineManagementBL;
            _workersManagmentDAL = workersManagmentDAL;
            _babyManagementBL = babyManagementBL;
        }

        public async Task<bool> IsHoliday(DateTime date)
        {
            string year = date.Year.ToString();
            string month = date.Month.ToString("D2");
            string day = date.Day.ToString("D2");

            var url = $"{_baseUrl}?v=1&cfg=json&year={year}&month={month}&maj=on&min=off&mod=on&nx=off";

            var response = await _httpClient.GetAsync(url);
            Console.WriteLine(response);
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
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

        public async Task<List<AvailableAppointmentDTO>> GetAllAvailableAppointments()
        {
            var appointments = await availableAppointmentManagementDAL.GetAllAvailableAppointmentsAsync();
            return appointments.Select(a => _mapper.Map<AvailableAppointmentDTO>(a)).ToList();
        }

        public async Task<List<AvailableAppointment>> findNurseAppointments(string babyId)
        {
            // שלב 1: קבלת חיסונים שלא בוצעו
            List<VaccineDTO> unvaccinatedVaccines = await _vaccineManagementBL.ListOfBabysUnvaccinatedVaccines(babyId);

            if (unvaccinatedVaccines == null || !unvaccinatedVaccines.Any())
                throw new Exception("No unvaccinated vaccines found for the given baby ID.");

            // שלב 2: קביעת גיל התינוק
            int babyAgeMonths = _babyManagementBL.GetBabysAge(babyId);

            // שלב 3: חישוב טווח גילאים לחיסונים
            VaccineDTO firstVaccine = unvaccinatedVaccines.First();
            int minAge = firstVaccine.MinAgeMonths;
            int maxAge = firstVaccine.MaxAgeMonths;


            DateTime earliestAppointmentDate = DateTime.Now.AddMonths(minAge - babyAgeMonths);
            DateTime latestAppointmentDate = earliestAppointmentDate.AddMonths(1);
            DateOnly earliestDate = DateOnly.FromDateTime(earliestAppointmentDate);
            DateOnly latestDate = DateOnly.FromDateTime(earliestAppointmentDate.AddMonths(1));


            // שלב 4: שליפת תורים מתאימים
            var allNurseAppointments = await availableAppointmentManagementDAL.GetAppointmentsByWorkerType("nurse");

            var appointmentsInRange = allNurseAppointments
                .Where(a => a.AppointmentDate.CompareTo(earliestDate) >= 0 && a.AppointmentDate.CompareTo(latestDate.AddMonths(1)) <= 0)
                .ToList();

            if (!appointmentsInRange.Any())
                throw new Exception("No available appointments found for the required time range.");

            return appointmentsInRange;
        }


        //public async Task<List<AvailableAppointment>> findDoctorAppointments()
        //{
        //    var availableAppointments = (await availableAppointmentManagementDAL.GetAppointmentsByWorkerType("Developmental Pediatrician")).
        //            FindAll(a => a.AppointmentDate.ToDateTime(TimeOnly.MinValue) >= DateTime.Now
        //         && a.AppointmentDate.ToDateTime(TimeOnly.MinValue) < DateTime.Now.AddMonths(1));
        //    return availableAppointments;
        //}
        public async Task<List<AvailableAppointment>> findPhysicalTherapistAppointments(string physiotherapistName, DateOnly startDate, int sessionsCount)
        {

            DateOnly endDate = startDate.AddMonths(1);

            int workerId = await _workersManagmentDAL.GetWorkerIdByName(physiotherapistName);
            // Filter appointments by worker type, therapist name, and date range
            var availableAppointments = (await availableAppointmentManagementDAL
                .GetAppointmentsByWorkerType("physical Therapist"))
                .FindAll(a =>
                    a.AppointmentDate >= startDate &&
                    a.AppointmentDate <= endDate && a.WorkerId == workerId
                );
            foreach (var firstAppointment in availableAppointments)
            {
                var candidateSeries = new List<AvailableAppointment> { firstAppointment };
                bool seriesFound = true;
                var currDate = firstAppointment.AppointmentDate;
                var currHour = firstAppointment.StartTime;

                for (int i = 1; i < sessionsCount; i++)
                {
                    var nextDate = currDate.AddDays(7);
                    var nextAppointment = availableAppointments.FirstOrDefault(
                        a => a.AppointmentDate == nextDate && a.StartTime == currHour);

                    if (nextAppointment != null)
                    {
                        candidateSeries.Add(nextAppointment);
                        currDate = nextDate;
                    }
                    else
                    {
                        seriesFound = false;
                        break;
                    }
                }

                if (seriesFound)
                {
                    return candidateSeries;
                }
            }

            return new List<AvailableAppointment>();
        }

        public async Task<List<AvailableAppointmentDTO>> FindSpecificTypeOfAvailableAppointments(string worketType)
        {
            var appointments = await availableAppointmentManagementDAL.GetAppointmentsByWorkerType(worketType);
            return appointments.Select(a => _mapper.Map<AvailableAppointmentDTO>(a)).ToList();
        }

        public async Task<List<AvailableAppointmentDTO>> FindAllAvailableAppointmentsByDate(DateOnly date)
        {
            var appointments = await availableAppointmentManagementDAL.GetAppointmentsByDateAsync(date);
            return appointments.Select(a => _mapper.Map<AvailableAppointmentDTO>(a)).ToList();
        }

        public async Task<bool> IsTimeSlotAvailableAsync(DateOnly date, TimeOnly startTime, string workerType)
        {
            string normalizedWorkerType = workerType.Replace(" ", "").ToLower();

            var appointmentDurationsSection = _configuration.GetSection("WorkerAppointmentDuration");
            var durationValue = appointmentDurationsSection[normalizedWorkerType];

            if (int.TryParse(durationValue, out int duration))
            {
                return await availableAppointmentManagementDAL.IsTimeSlotAvailableAsync(date, startTime, startTime.AddMinutes(duration));
            }

            return false;
        }

        public async Task AddAvailableAppointmentsToAllWorkers(DateTime date)
        {
            if (date < DateTime.Now)
            {
                throw new ArgumentException("The date must be in the future.");
            }
            int dayOfWeek = (int)date.DayOfWeek;
            List<Shift> shifts = await _shiftManager.GetShiftsByDayAsync(dayOfWeek + 1);

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
                        string normalizedWorkerType = worker.WorkerType.Replace(" ", "").ToLower();

                        var normalizedWorkerTypes = workerTypes
                            .Select(x => x.Replace(" ", "").ToLower())
                            .ToList();

                        if (!normalizedWorkerTypes.Contains(normalizedWorkerType))
                        {
                            Console.WriteLine($"Skipped worker with type: {worker.WorkerType} (not eligible for appointments)");
                            continue;
                        }

                        var appointmentDurations = _configuration[$"WorkerAppointmentDuration:{worker.WorkerType.Replace(" ", "")}"];
                        if (int.TryParse(appointmentDurations, out int appointmentDuration))
                        {
                            for (TimeOnly time = shift.StartTime; time <= shift.EndTime; time = time.AddMinutes(appointmentDuration))
                            {
                                AvailableAppointmentDTO appointment = new AvailableAppointmentDTO
                                {
                                    WorkerId = worker.Id,
                                    AppointmentDate = DateOnly.FromDateTime(date),
                                    StartTime = time,
                                    EndTime = time.AddMinutes(appointmentDuration),
                                    Worker = worker
                                };

                                var appointmentEntity = _mapper.Map<AvailableAppointment>(appointment);

                                await availableAppointmentManagementDAL.AddAvailableAppointmentAsync(appointmentEntity);
                            }
                        }
                    }
                }
            }
        }
        public async Task AddAvailableAppointmentsForNextYear()
        {
            DateTime startDate = DateTime.Today.AddDays(1);
            DateTime endDate = startDate.AddYears(1);

            for (DateTime date = startDate; date < endDate; date = date.AddDays(1))
            {
                await AddAvailableAppointmentsToAllWorkers(date); // זו פונקציה קיימת שלך
            }
        }
       
    }
}
