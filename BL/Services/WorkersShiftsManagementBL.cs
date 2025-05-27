using WEB_API.BL.API;
using WEB_API.DAL.API;
using WEB_API.Models;
using Microsoft.Extensions.Configuration;
using System.Linq;
using WEB_API.DAL.Models;

namespace WEB_API.BL.Services
{
    public class WorkersShiftsManagementBL : IWorkersShiftManagementBL
    {
        private readonly IShiftManagementDAL shiftsManagement;
        private readonly IWorkerShiftManagementDAL workerShiftsManagement;
        private readonly IConfiguration _configuration;


        public WorkersShiftsManagementBL(IShiftManagementDAL _shiftManagementBL, IWorkerShiftManagementDAL _workerShiftManagementDAL)
        {
            shiftsManagement = _shiftManagementBL;
            workerShiftsManagement = _workerShiftManagementDAL;
        }

    }
}
