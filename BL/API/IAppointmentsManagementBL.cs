using WEB_API.BL.Models;

namespace WEB_API.BL.API
{
    public interface IAppointmentsManagementBL
    {
        Task<AppointmentDTO> BookAppointment(string babyId, string workerType, DateOnly date, TimeOnly time);
        Task<AppointmentDTO> BookVaccineAppointment(string babyId, DateOnly date, TimeOnly time, int vaccineId);
        Task DeleteAppointmentAsync(int id);
        Task<List<AppointmentDTO>> GetAppointmentsByDateAsync(DateOnly date);
        Task<List<AppointmentDTO>> GetBabyAppointmentsHistory(string babyId);
        Task<List<AppointmentDTO>> GetUpcomingAppointmentsForBaby(string babyId);
        Task<List<AppointmentDTO>?> GetWorkerAppointments(string workerId);
        Task<Dictionary<string, int>> GetMonthlyAppointmentStatistics();

        //DateTime LastVisit(string babyId);
    }
}