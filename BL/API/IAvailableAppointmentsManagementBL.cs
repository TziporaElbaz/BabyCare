using WEB_API.BL.Models;

namespace WEB_API.BL.API
{
    public interface IAvailableAppointmentsManagementBL
    {
        Task AddAvailableAppointmentsToAllWorkers(DateTime date);
        //Task<List<AvailableAppointment>> findDoctorAppointments();
        //Task<List<AvailableAppointment>> findNurseAppointments(string babyId);
        Task<List<AvailableAppointmentDTO>> FindSpecificTypeOfAvailableAppointments(string worketType);
        Task<List<AvailableAppointmentDTO>> FindAllAvailableAppointmentsByDate(DateOnly date);
        Task<List<AvailableAppointmentDTO>> GetAllAvailableAppointments();
        Task<bool> IsHoliday(DateTime date);
        Task<bool> IsTimeSlotAvailableAsync(DateOnly date, TimeOnly startTime, string workerType);
    }
}