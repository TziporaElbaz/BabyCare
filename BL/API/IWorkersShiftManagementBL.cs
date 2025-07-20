namespace WEB_API.BL.API
{
    public interface IWorkersShiftManagementBL
    {
        Task AddShiftToWorker(int shiftId, string workerId);
    }
}
