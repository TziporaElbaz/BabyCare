using WEB_API.DAL.Models;

namespace DAL.API
{
    internal interface IAvailableAppointmentDAL
    {
        Task DeleteAppointmentAsync(int id);
        Task DeleteAvailableAppointmentAsync(int id);
        Task<List<AvailableAppointment>> GetAllAppointmentsAsync();
        Task<AvailableAppointment?> GetAppointmentById(int appointmentsId);
        Task<List<AvailableAppointment>> GetAppointmentsByDateAsync(DateOnly date);
        Task<List<AvailableAppointment?>> GetAppointmentsByWorkerType(string workerType);
        Task<List<AvailableAppointment?>> GetAppointmentsByWorkerTypeAndMonth(string workerType, int month);
    }
}