using WEB_API.Models;

namespace BL.API
{
    public interface IAppointmentsManagementBL
    {
        List<Appointment> GetBabyAppointmentsHistory(string babyId);
        List<Appointment> getBookedAppointmentsForBaby(string babyId);
        DateTime LastVisit(string babyId);
    }
}