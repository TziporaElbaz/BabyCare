using WEB_API.DAL.Models;


namespace WEB_API.BL.API
{
    public interface IAppointmentsManagementBL
    {
        List<Appointment> GetBabyAppointmentsHistory(string babyId);
        List<Appointment> getBookedAppointmentsForBaby(string babyId);
        DateTime LastVisit(string babyId);
    }
}