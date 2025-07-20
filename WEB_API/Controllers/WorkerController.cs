using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WEB_API.BL.API;
using WEB_API.BL.Models;
using WEB_API.DAL.Models;


namespace WEB_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkerController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IWorkerManegmentBL _workerManagementBL;
        private IJwtService _jwtService;

        public WorkerController(IMapper mapper, IWorkerManegmentBL workerManegmentBL, IJwtService jwtService)
        {
            _mapper = mapper;
            _workerManagementBL = workerManegmentBL;
            _jwtService = jwtService;
        }

        // GET: api/WorkerManagement
        [HttpGet]
        public async Task<ActionResult<List<WorkerDTO>>> GetAllWorkers()
        {
            var workers = await _workerManagementBL.GetAllWorkersAsync();
            return Ok(workers);
        }

        // GET: api/WorkerManagement/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<WorkerDTO>> GetWorkerById(string id)
        {
            var worker = await _workerManagementBL.GetWorkerByIdAsync(id);
            if (worker == null)
            {
                return NotFound("worker not found");
            }
            return Ok(worker);
        }

        // POST: api/WorkerManagement
        [HttpPost("addWorker")]
        public async Task<ActionResult<WorkerDTO>> AddWorker([FromBody] WorkerDTO workerDto)
        {
            try
            {
                await _workerManagementBL.AddWorkerAsync(workerDto);

                var token = _jwtService.GenerateToken(workerDto.WorkerId, "worker");

                _jwtService.SetTokenCookie(Response, token);

                return CreatedAtAction(nameof(GetWorkerById), new { id = workerDto.WorkerId }, new { worker = workerDto, token });
            }
            catch (Exception ex)
            {
                return BadRequest($"Error adding worker: {ex.Message}");
            }
        }

        // PUT: api/WorkerManagement/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateWorker(string id, [FromBody] WorkerDTO updatedWorker)
        {
            if (!(id.Equals(updatedWorker.WorkerId)))
            {
                return BadRequest();
            }

            var workerEntity = _mapper.Map<Worker>(updatedWorker);
            await _workerManagementBL.UpdateWorkerDetailsAsync(workerEntity);
            return Ok(updatedWorker);
        }

        // DELETE: api/WorkerManagement/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteWorker(string id, [FromQuery] string name)
        {
            await _workerManagementBL.DeleteWorkerAsync(id, name);
            return Ok("success");
        }

        [HttpGet("getAmountOfEachType")]
        public async Task<ActionResult<Dictionary<string, int>>> GetAmountOfEachType()
        {
            var result = await _workerManagementBL.GetWorkerTypeStatistics();
            if (result == null || result.Count == 0)
            {
                return NotFound("No workers found.");
            }
            return Ok(result);
        }
    }
}



