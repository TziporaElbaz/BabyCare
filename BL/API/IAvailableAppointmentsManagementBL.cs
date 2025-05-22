namespace Project.BL.API
{
    public interface IAvailableAppointmentsManagementBL
    {
        void AddAvailableAppointmentsToWorkers(DateTime day);
        bool IsHoliday(DateTime date);
    }
}