using WEB_API.DAL.Models;

namespace WEB_API.DAL.API
{
    public interface IWorkersManagmentDAL
    {
        Task AddWorkerAsync(Worker worker);
        Task DeleteWorkerAsync(string id, string name);
        Task<List<Worker>> GetAllWorkersAsync();
        Task<Worker?> GetWorkerByIdAsync(string id);
        Task UpdateWorkerDetailsAsync(Worker updatedWorker);
    }
}