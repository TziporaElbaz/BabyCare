using WEB_API.DAL.Models;

namespace WEB_API.DAL.API
{
    public interface IAppointmentManagementDAL
    {
        Task DeleteAppointmentAsync(int id);
        Task<List<Appointment>> GetAllAppointmentsAsync();
        Task<Appointment?> GetAppointmentByIdAsync(int id);
        //Task<Appointment?> GetAppointmentByWorkerAndDatetime(int workerId, DateOnly date, TimeOnly time);
        Task AddAppointment(Appointment appointment);
        Task<List<Appointment>> GetAppointmentsByDateAsync(DateOnly date);
        Task UpdateAppointmentAsync(Appointment updatedAppointment);
    }
}