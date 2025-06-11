using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WEB_API.BL.Models;
using WEB_API.DAL.API;
using WEB_API.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using WEB_API.BL.Services;
using WEB_API.BL.API;


namespace WEB_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkerManagementController : ControllerBase
    {
        private readonly IWorkersManagmentDAL _workersManagmentDAL;
        private readonly IMapper _mapper;
        private readonly IWorkerManegmentBL _workerManegmentBL;
        public WorkerManagementController(IWorkersManagmentDAL workersManagmentDAL, IMapper mapper, IWorkerManegmentBL workerManegmentBL )
        {
            _workersManagmentDAL = workersManagmentDAL;
            _mapper = mapper;
            _workerManegmentBL= workerManegmentBL;
        }

        // GET: api/WorkerManagement
        [HttpGet]
        public async Task<ActionResult<List<Worker>>> GetAllWorkers()
        {
            var workers = await _workerManegmentBL.GetAllWorkersAsync();
            return Ok(workers);
        }

        // GET: api/WorkerManagement/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Worker>> GetWorkerById(string id)
        {
            var worker = await _workerManegmentBL.GetWorkerByIdAsync(id);
            if (worker == null)
            {
                return NotFound();
            }
            return Ok(worker);
        }

        // POST: api/WorkerManagement
        [HttpPost]
        public async Task<ActionResult> AddWorker([FromBody] WorkerBL worker)
        {
            var workerEntity = _mapper.Map<Worker>(worker);

            await _workersManagmentDAL.AddWorkerAsync(workerEntity);
            return CreatedAtAction(nameof(GetWorkerById), new { id = workerEntity.Id }, workerEntity);
        }

        // PUT: api/WorkerManagement/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateWorker(string id, [FromBody] WorkerBL updatedWorker)
        {
            if (!(id.Equals(updatedWorker.WorkerId)))
            {
                return BadRequest();
            }

            var workerEntity = _mapper.Map<Worker>(updatedWorker);
            await _workerManegmentBL.UpdateWorkerDetailsAsync(workerEntity);
            return Ok(updatedWorker);
        }

        // DELETE: api/WorkerManagement/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteWorker(string id, [FromQuery] string name)
        {
                await _workerManegmentBL.DeleteWorkerAsync(id, name);
                return Ok("success");
        }
    }
}



