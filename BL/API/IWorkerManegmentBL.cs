using WEB_API.DAL.Models;

namespace WEB_API.BL.API
{
    public interface IWorkerManegmentBL
    {
        Task AddWorkerAsync(Worker worker);
        Task DeleteWorkerAsync(int id, string name);
        Task<List<Worker>> GetAllWorkersAsync();
        Task<Worker?> GetWorkerByIdAsync(int id);
        Task UpdateWorkerDetailsAsync(Worker updatedWorker);
    }
}