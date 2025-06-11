using WEB_API.DAL.Models;
namespace WEB_API.DAL.API
{
    public interface IAvailableAppointmentManagementDAL
    {
        Task AddAvailableAppointmentAsync(AvailableAppointment appointment);
        Task DeleteAvailableAppointmentAsync(int id);
        Task<List<AvailableAppointment>> GetAllAvailableAppointmentsAsync();
        Task<AvailableAppointment?> GetAvailableAppointmentByIdAsync(int id);
        Task<AvailableAppointment?> GetAvailableAppointmentByWorkerAndDatetime(string workerId, DateOnly date, TimeOnly time);
        Task<List<AvailableAppointment>> GetAvailableAppointmentsByDateAsync(DateOnly date);
        Task<bool> IsTimeSlotAvailableAsync(DateOnly date, TimeOnly startTime, TimeOnly endTime);
        Task UpdateAvailableAppointmentAsync(AvailableAppointment updatedAppointment);
    }
}