using AutoMapper;
using WEB_API.BL.API;
using WEB_API.BL.Models;
using WEB_API.DAL.API;
using WEB_API.DAL.Models;

namespace WEB_API.BL.Services
{
    public class WorkerManegmentBL : IWorkerManegmentBL
    {
        private readonly IWorkersManagmentDAL _workersManagmentDAL;
        private readonly IMapper _mapper;
        public WorkerManegmentBL(IWorkersManagmentDAL workersManagmentDAL, IMapper mapper)
        {
            _workersManagmentDAL = workersManagmentDAL;
            _mapper = mapper;
        }

        public async Task AddWorkerAsync(WorkerDTO workerDTO)
        {
            var worker = _mapper.Map<Worker>(workerDTO);
            if (worker == null)
            {
                throw new ArgumentNullException(nameof(worker), "Worker cannot be null");
            }
            await _workersManagmentDAL.AddWorkerAsync(worker);
        }

        public async Task DeleteWorkerAsync(string id, string name)
        {
            await _workersManagmentDAL.DeleteWorkerAsync(id, name);
        }

        public async Task<WorkerDTO?> GetWorkerByIdAsync(string id)
        {
            var worker = await _workersManagmentDAL.GetWorkerByIdAsync(id);
            return worker == null ? null : _mapper.Map<WorkerDTO>(worker);
        }

        public async Task<List<WorkerDTO>> GetAllWorkersAsync()
        {
            var workers = await _workersManagmentDAL.GetAllWorkersAsync();
            return workers.Select(a => _mapper.Map<WorkerDTO>(a)).ToList();
        }

        public async Task UpdateWorkerDetailsAsync(Worker updatedWorker)
        {
            await _workersManagmentDAL.UpdateWorkerDetailsAsync(updatedWorker);
        }

        public async Task<Dictionary<string, int>> GetWorkerTypeStatistics()
        {
            var workers = await _workersManagmentDAL.GetAllWorkersAsync();
            var workerTypeStatistics = new Dictionary<string, int>();

            foreach (var worker in workers)
            {
                var workerType = worker.WorkerType;
                if (workerTypeStatistics.ContainsKey(workerType))
                {
                    workerTypeStatistics[workerType]++;
                }
                else
                {
                    workerTypeStatistics[workerType] = 1;
                }
            }

            return workerTypeStatistics;
        }

    }
}

