using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WEB_API.DAL.API;
using WEB_API.DAL.Models;
using WEB_API.BL.API;

namespace WEB_API.BL.Services
{
    public class WorkerManegmentBL : IWorkerManegmentBL
    {
        private readonly IWorkersManagmentDAL _workersManagmentDAL;

        public WorkerManegmentBL(IWorkersManagmentDAL workersManagmentDAL)
        {
            _workersManagmentDAL = workersManagmentDAL;
        }

        public async Task AddWorkerAsync(Worker worker)
        {
            await _workersManagmentDAL.AddWorkerAsync(worker);
        }

        public async Task DeleteWorkerAsync(string id, string name)
        {
            await _workersManagmentDAL.DeleteWorkerAsync(id, name);
        }

        public async Task<Worker?> GetWorkerByIdAsync(string id)
        {
            return await _workersManagmentDAL.GetWorkerByIdAsync(id);
        }

        public async Task<List<Worker>> GetAllWorkersAsync()
        {
            return await _workersManagmentDAL.GetAllWorkersAsync();
        }

        public async Task UpdateWorkerDetailsAsync(Worker updatedWorker)
        {
            await _workersManagmentDAL.UpdateWorkerDetailsAsync(updatedWorker);
        }
    }
}
