using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WEB_API.DAL.API;
using WEB_API.DAL.Models;


namespace WEB_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkerManagmentController : ControllerBase
    {
        private readonly IWorkersManagmentDAL _workersManagmentDAL;

        public WorkerManagmentController(IWorkersManagmentDAL workersManagmentDAL)
        {
            _workersManagmentDAL = workersManagmentDAL;
        }

        // GET: api/WorkerManagment
        [HttpGet]
        public async Task<ActionResult<List<Worker>>> GetAllWorkers()
        {
            var workers = await _workersManagmentDAL.GetAllWorkersAsync();
            return Ok(workers);
        }
    }

}

