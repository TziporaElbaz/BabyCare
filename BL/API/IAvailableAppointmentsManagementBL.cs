namespace BL.API
{
    public interface IAvailableAppointmentsManagementBL
    {
        void AddAvailableAppointmentsToWorkers(DateTime date);
        bool IsHoliday(DateTime date);
    }
}