using WEB_API.DAL.Models;

namespace WEB_API.DAL.API
{
    public interface IWorkerShiftManagementDAL
    {
        Task AddWorkerShiftAsync(Worker worker, Shift shift);
        Task DeleteWorkerShiftAsync(string id);

        Task<List<WorkerShift>> GetAllWorkerShiftsAsync();
        Task<List<Shift>> GetShiftsByWorkerID(string workerId);
        Task<List<Worker>> GetWorkersByShiftID(int shiftId);
        Task UpdateWorkerShiftDetailsAsync(WorkerShift updatedWorkerShift);
    }
}