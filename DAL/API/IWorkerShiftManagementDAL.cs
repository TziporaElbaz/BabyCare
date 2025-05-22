
using Project.Models;

namespace Project.DAL.API
{
    public interface IWorkerShiftManagementDAL
    {
        Task AddWorkerShiftAsync(Worker worker, Shift shift);
        Task DeleteWorkerShiftAsync(int id);
        Task<List<WorkerShift>> GetAllWorkerShiftsAsync();
        Task<WorkerShift?> GetWorkerShiftByIdAsync(int id);
        Task<List<WorkerShift>> GetWorkerShiftsByShiftIdAsync(int day);
        Task<List<Worker>> GetWorkersByShiftID(int shiftId);
        Task<List<Shift>>GetShiftByWorkerID(int workerId);
        Task UpdateWorkerShiftDetailsAsync(WorkerShift updatedWorkerShift);
    }
}