using Project.DAL.models;

namespace Project.DAL.services
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