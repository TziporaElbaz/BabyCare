using Project.BL.API;
using Project.DAL.API;
using Project.Models;
using Microsoft.Extensions.Configuration;
using System.Linq;
using Project.DAL.Models;

namespace Project.BL.Services
{
    public class WorkersShiftsManagementBL:IWorkersShiftManagementBL
    {
        private readonly IShiftManagementDAL shiftsManagement;
        private readonly IWorkerShiftManagementDAL workerShiftsManagement;
        private readonly IConfiguration _configuration;


        public WorkersShiftsManagementBL(IShiftManagementDAL _shiftManagementBL, IWorkerShiftManagementDAL _workerShiftManagementDAL)
        {
            shiftsManagement = _shiftManagementBL;  
            workerShiftsManagement=_workerShiftManagementDAL;

        }

       
    }
}
