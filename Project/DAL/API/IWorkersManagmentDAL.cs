using Project.DAL.models;

namespace Project.DAL.services
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