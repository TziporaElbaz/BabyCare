using BabyCare.DAL.Models;

namespace BabyCare.DAL.API
{
    public interface IWorkersManagmentDAL
    {
        Task AddWorkerAsync(Worker worker);
        Task DeleteWorkerAsync(int id, string name);
        Task<List<Worker>> GetAllWorkersAsync();
        Task<Worker?> GetWorkerByIdAsync(int id);
        Task UpdateWorkerDetailsAsync(Worker updatedWorker);
    }
}