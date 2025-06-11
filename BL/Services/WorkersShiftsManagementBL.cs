using BabyCare.BL.API;
using BabyCare.DAL.API;
using BabyCare.DAL.Models;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace BabyCare.BL.Services
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
