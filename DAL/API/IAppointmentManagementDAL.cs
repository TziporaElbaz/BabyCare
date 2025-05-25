

using WEB_API.Models;

namespace WEB_API.DAL.API
{
    public interface IAppointmentManagementDAL
    {
        Task AddAppointmentAsync(Appointment appointment);
        Task DeleteAppointmentAsync(int id);
        Task<List<Appointment>> GetAllAppointmentsAsync();
        Task<Appointment?> GetAppointmentByIdAsync(int id);
        Task<Appointment?> GetAppointmentByWorkerAndDatetime(int workerId, DateOnly date, TimeOnly time);
        Task<List<Appointment>> GetAppointmentsByDateAsync(DateOnly date);
        Task<bool> IsTimeSlotAvailableAsync(DateOnly date, TimeOnly startTime, TimeOnly endTime);
        Task UpdateAppointmentAsync(Appointment updatedAppointment);
    }
}