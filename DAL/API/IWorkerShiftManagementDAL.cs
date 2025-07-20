using WEB_API.DAL.Models;

namespace WEB_API.DAL.API
{
    public interface IWorkerShiftManagementDAL
    {
        //Task AssignAllWorkersToAllShifts();
        //Task AssignAllWorkersToShiftsForDayAsync(int dayOfWeek);
        Task AddWorkerShiftAsync(Worker worker, Shift shift);
        Task<List<WorkerShift>> GetAllWorkerShiftsAsync();
        Task<List<Worker>> GetWorkersByShiftID(int shiftId);
        Task UpdateWorkerShiftDetailsAsync(WorkerShift updatedWorkerShift);
    }
}