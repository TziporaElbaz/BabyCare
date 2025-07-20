using WEB_API.DAL.Models;

namespace WEB_API.DAL.API
{
    public interface IAvailableAppointmentManagementDAL
    {
        Task DeleteAvailableAppointmentAsync(int id);
        Task<List<AvailableAppointment>> GetAllAvailableAppointmentsAsync();
        Task<List<AvailableAppointment>> GetAppointmentsByDateAsync(DateOnly date);
        Task<List<AvailableAppointment?>> GetAppointmentsByWorkerType(string workerType);
        //Task<List<AvailableAppointment?>> GetAppointmentsByWorkerTypeAndMonth(string workerType, int month);
        Task<AvailableAppointment?> GetAvailableAppointmentByWorkerAndDatetime(DateOnly date, TimeOnly time, string workerType);
        Task<bool> IsTimeSlotAvailableAsync(DateOnly date, TimeOnly startTime, TimeOnly endTime);
        Task AddAvailableAppointmentAsync(AvailableAppointment appointment);
    }
}