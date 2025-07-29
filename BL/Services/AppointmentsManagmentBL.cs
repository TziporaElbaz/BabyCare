using AutoMapper;
using WEB_API.DAL.API;
using WEB_API.BL.API;
using WEB_API.BL.Models;
using WEB_API.DAL.Models;

namespace WEB_API.BL.Services
{
    public class AppointmentsManagementBL : IAppointmentsManagementBL
    {
        private readonly IAvailableAppointmentManagementDAL availableAppointmentManagementDAL;
        private readonly IAppointmentManagementDAL appointmentManagementDAL;
        private readonly IBabyManagementDAL babyManagementDAL;
        private readonly IWorkersManagmentDAL workerManagementDAL;
        private readonly IBabyVaccineManagementDAL babyVaccineManagementDAL;
        private readonly IVaccineManagementDAL vaccineManagementDAL;
        private readonly IMapper mapper;

        private static readonly SemaphoreSlim _bookingSemaphore = new SemaphoreSlim(1, 1);

        public AppointmentsManagementBL(
            IAppointmentManagementDAL _appointmentManagementDAL,
            IWorkersManagmentDAL _workerManagementDAL,
            IAvailableAppointmentManagementDAL _availableAppointmentDAL,
            IBabyManagementDAL _babyManagementDAL,
            IWorkersManagmentDAL _workersManagmentDAL,
            IBabyVaccineManagementDAL _babyVaccineManagementDAL,
            IVaccineManagementDAL _vaccineManagementDAL,
            IMapper _mapper)
        {
            appointmentManagementDAL = _appointmentManagementDAL;
            availableAppointmentManagementDAL = _availableAppointmentDAL;
            babyManagementDAL = _babyManagementDAL;
            workerManagementDAL = _workerManagementDAL;
            babyVaccineManagementDAL = _babyVaccineManagementDAL;
            vaccineManagementDAL = _vaccineManagementDAL;
            mapper = _mapper;
        }

        //public async Task<AppointmentDTO> BookAppointment(string babyId, string workerType, DateOnly date, TimeOnly time)
        //{
        //    var baby = await babyManagementDAL.GetBabyByIdAsync(babyId);

        //    var availableAppointment = await availableAppointmentManagementDAL.GetAvailableAppointmentByWorkerAndDatetime(date, time, workerType);

        //    if (baby == null || availableAppointment == null)
        //        return null;

        //    var babyAppointments = await babyManagementDAL.GetBabyAppointments(baby);
        //    if (babyAppointments.Any(a => a.AppointmentDate.Equals(availableAppointment.AppointmentDate)))
        //        return null;

        //    AppointmentDTO appointmentEntity = new AppointmentDTO()
        //    {
        //        WorkerId = availableAppointment.WorkerId,
        //        BabyId = baby.Id,
        //        AppointmentDate = date,
        //        StartTime = time,
        //        EndTime = availableAppointment.EndTime,
        //        Worker = availableAppointment.Worker,
        //        Baby = baby
        //    };

        //    var appointment = mapper.Map<Appointment>(appointmentEntity);
        //    await appointmentManagementDAL.AddAppointment(appointment);
        //    await babyManagementDAL.AddAppointmentToBaby(baby, appointment);
        //    await workerManagementDAL.AddAppointmentToWorker(availableAppointment.Worker, appointment);
        //    await availableAppointmentManagementDAL.DeleteAvailableAppointmentAsync(availableAppointment.Id);

        //    return appointmentEntity;
        //}
        public async Task<AppointmentDTO> BookAppointment(string babyId, string workerType, DateOnly date, TimeOnly time)
        {
            await _bookingSemaphore.WaitAsync();
            try
            {
                var baby = await babyManagementDAL.GetBabyByIdAsync(babyId);
                var availableAppointment = await availableAppointmentManagementDAL.GetAvailableAppointmentByWorkerAndDatetime(date, time, workerType);

                if (baby == null || availableAppointment == null)
                    return null;
                var babyAppointments = await babyManagementDAL.GetBabyAppointments(baby);
                if (babyAppointments.Any(a => a.AppointmentDate.Equals(availableAppointment.AppointmentDate)))
                    return null;

                AppointmentDTO appointmentEntity = new AppointmentDTO()
                {
                    WorkerId = availableAppointment.WorkerId,
                    BabyId = baby.Id,
                    AppointmentDate = date,
                    StartTime = time,
                    EndTime = availableAppointment.EndTime,
                    Worker = availableAppointment.Worker,
                    Baby = baby
                };

                var appointment = mapper.Map<Appointment>(appointmentEntity);
                await appointmentManagementDAL.AddAppointment(appointment);
                await babyManagementDAL.AddAppointmentToBaby(baby, appointment);
                await workerManagementDAL.AddAppointmentToWorker(availableAppointment.Worker, appointment);
                await availableAppointmentManagementDAL.DeleteAvailableAppointmentAsync(availableAppointment.Id);

                return appointmentEntity;
            }
            finally
            {
                _bookingSemaphore.Release();
            }
        }

        public async Task<AppointmentDTO> BookVaccineAppointment(string babyId, DateOnly date, TimeOnly time, int vaccineId)
        {
            var baby = await babyManagementDAL.GetBabyByIdAsync(babyId);
            var vaccine = await vaccineManagementDAL.GetVaccineByIdAsync(vaccineId);
            if (baby == null || vaccine == null)
                return null;
            await babyVaccineManagementDAL.AddBabyVaccineAsync(baby, vaccine, date);
            return await BookAppointment(babyId, "Nurse", date, time);
        }

        public async Task<List<AppointmentDTO>> GetUpcomingAppointmentsForBaby(string babyId)
        {
            var baby = await babyManagementDAL.GetBabyByIdAsync(babyId);
            if (baby != null)
            {
                var appointments = await babyManagementDAL.GetBabyAppointments(baby);
                var currentDate = DateOnly.FromDateTime(DateTime.Now);
                return appointments
                    .Where(a => a.AppointmentDate >= currentDate)
                    .Select(a => mapper.Map<AppointmentDTO>(a))
                    .ToList();
            }
            return null;
        }

        public async Task<List<AppointmentDTO>?> GetWorkerAppointments(string workerId)
        {
            var worker = await workerManagementDAL.GetWorkerByIdAsync(workerId);
            if (worker != null)
            {
                var appointments = await workerManagementDAL.GetWorkerAppointments(worker);
                return appointments.Select(a => mapper.Map<AppointmentDTO>(a)).ToList();
            }
            return null;
        }

        public async Task<List<AppointmentDTO>> GetAppointmentsByDateAsync(DateOnly date)
        {
            var appointments = await appointmentManagementDAL.GetAppointmentsByDateAsync(date);
            return appointments.Select(a => mapper.Map<AppointmentDTO>(a)).ToList();
        }

        public async Task DeleteAppointmentAsync(int id)
        {
            await appointmentManagementDAL.DeleteAppointmentAsync(id);
        }

        public async Task<List<AppointmentDTO>> GetBabyAppointmentsHistory(string babyId)
        {
            var baby = await babyManagementDAL.GetBabyByIdAsync(babyId);
            if (baby != null)
            {
                var appointments = await babyManagementDAL.GetBabyAppointments(baby);
                return appointments.Select(a => mapper.Map<AppointmentDTO>(a)).ToList();
            }
            return null;
        }

        public async Task<Dictionary<string, int>> GetMonthlyAppointmentStatistics()
        {
            var appointments = await appointmentManagementDAL.GetAllAppointmentsAsync();
            var monthlyStatistics = new Dictionary<string, int>();

            foreach (var appointment in appointments)
            {
                var monthYear = $"{appointment.AppointmentDate.Year}-{appointment.AppointmentDate.Month}";
                if (monthlyStatistics.ContainsKey(monthYear))
                {
                    monthlyStatistics[monthYear]++;
                }
                else
                {
                    monthlyStatistics[monthYear] = 1;
                }
            }

            return monthlyStatistics;
        }

        //public DateTime LastVisit(string babyId)
        //{

        //    List<Appointment> babyAppointments =await GetBabyAppointmentsHistory(babyId);


        //    if (!babyAppointments.Any())
        //    {
        //        throw new Exception("No appointments found for the given baby ID.");
        //    }
        //    Appointment lastAppointment = babyAppointments.Last(); // מאחר והרשימה כבר מסודרת לפי תאריך
        //    return new DateTime(
        //        lastAppointment.AppointmentDate.Year,
        //        lastAppointment.AppointmentDate.Month,
        //        lastAppointment.AppointmentDate.Day,
        //        lastAppointment.StartTime.Hour,
        //        lastAppointment.StartTime.Minute,
        //        lastAppointment.StartTime.Second
        //    );
        //}

    }
}

