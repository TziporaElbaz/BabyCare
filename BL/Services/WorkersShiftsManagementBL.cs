using WEB_API.BL.API;
using WEB_API.DAL.API;

namespace WEB_API.BL.Services
{
    public class WorkersShiftsManagementBL : IWorkersShiftManagementBL
    {
        private readonly IWorkerShiftManagementDAL workerShiftManagement;
        private readonly IWorkersManagmentDAL workersManagment;
        private readonly IShiftManagementDAL shiftManagement;

        public WorkersShiftsManagementBL(IWorkerShiftManagementDAL _workerShiftManagementDAL, IWorkersManagmentDAL _workersManagmentDAL, IShiftManagementDAL _shiftManagement)
        {
            workerShiftManagement = _workerShiftManagementDAL;
            workersManagment = _workersManagmentDAL;
            shiftManagement = _shiftManagement;
        }
        public async Task AddShiftToWorker(int shiftId, string workerId)
        {
            var shift = await shiftManagement.GetShiftByIdAsync(shiftId);
            var worker = await workersManagment.GetWorkerByIdAsync(workerId);

            if (shift == null || worker == null)
            {
                throw new ArgumentNullException("Shift or Worker cannot be null");
            }
            await workerShiftManagement.AddWorkerShiftAsync(worker, shift);
        }
    }
}
