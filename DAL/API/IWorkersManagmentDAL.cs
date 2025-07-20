using WEB_API.DAL.Models;

namespace WEB_API.DAL.API
{
    public interface IWorkersManagmentDAL
    {
        Task<int> GetWorkerIdByName(string name);
        Task AddWorkerAsync(Worker worker);
        Task DeleteWorkerAsync(string id, string name);
        Task<List<Worker>> GetAllWorkersAsync();
        Task<Worker?> GetWorkerByIdAsync(string id);
        Task UpdateWorkerDetailsAsync(Worker updatedWorker);
        Task AddAppointmentToWorker(Worker worker, Appointment appointment);
        Task<List<Appointment>> GetWorkerAppointments(Worker worker);
    }
}