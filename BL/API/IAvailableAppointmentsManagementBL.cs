using WEB_API.BL.Models;
using WEB_API.DAL.Models;

namespace BL.API
{
    public interface IAvailableAppointmentsManagementBL
    {
        Task AddAvailableAppointmentsToAllWorkers(DateTime date);
        Task AddAvailableAppointmentsForNextYear();
        Task<List<AvailableAppointmentDTO>> FindAllAvailableAppointmentsByDate(DateOnly date);
        Task<List<AvailableAppointment>> findNurseAppointments(string babyId);
        Task<List<AvailableAppointment>> findPhysicalTherapistAppointments(string physiotherapistName, DateOnly startDate, int sessionsCount);
        Task<List<AvailableAppointmentDTO>> FindSpecificTypeOfAvailableAppointments(string worketType);
        Task<List<AvailableAppointmentDTO>> GetAllAvailableAppointments();
        Task<bool> IsHoliday(DateTime date);
        Task<bool> IsTimeSlotAvailableAsync(DateOnly date, TimeOnly startTime, string workerType);
    }
}