namespace WEB_API.BL.API
{
    public interface IAvailableAppointmentsManagementBL
    {
        Task AddAvailableAppointmentsToWorkers(DateTime day);
        bool IsHoliday(DateTime date);
    }
}