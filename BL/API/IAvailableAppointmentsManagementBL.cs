namespace WEB_API.BL.API
{
    public interface IAvailableAppointmentsManagementBL
    {
        void AddAvailableAppointmentsToWorkers(DateTime day);
        bool IsHoliday(DateTime date);
    }
}