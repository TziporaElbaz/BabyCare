using WEB_API.BL.Models;
using WEB_API.DAL.Models;

namespace WEB_API.BL.API
{
    public interface IWorkerManegmentBL
    {
        Task AddWorkerAsync(WorkerDTO workerDTO);
        Task DeleteWorkerAsync(string id, string name);
        Task<List<WorkerDTO>> GetAllWorkersAsync();
        Task<WorkerDTO?> GetWorkerByIdAsync(string id);
        Task UpdateWorkerDetailsAsync(Worker updatedWorker);
        Task<Dictionary<string, int>> GetWorkerTypeStatistics();
    }
}