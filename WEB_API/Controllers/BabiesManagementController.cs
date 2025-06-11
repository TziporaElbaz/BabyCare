using Microsoft.AspNetCore.Mvc;
using BabyCare.BL.Models;
using BabyCare.BL.API;
using AutoMapper;
using BabyCare.DAL.Models;

namespace BabyCare.WEB_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BabiesController : ControllerBase
    {
        private readonly IBabyManagementBL _bl;
        private readonly IMapper _mapper;

        public BabiesController(IBabyManagementBL bl, IMapper mapper)
        {
            _bl = bl;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<BabyBL>> AddBaby([FromBody] BabyBL babyDto)
        {
            var babyEntity = _mapper.Map<Baby>(babyDto);
            await _bl.AddBaby(babyEntity);

            var babyCreatedDto = _mapper.Map<BabyBL>(babyEntity);
            return CreatedAtAction(nameof(GetBabyById), new { id = babyEntity.BabyId}, babyCreatedDto);
        }

        [HttpGet]
        public async Task<ActionResult<List<BabyBL>>> GetAllBabies()
        {
            var babies = await _bl.GetAllBabies();
            var babyDtos = _mapper.Map<List<BabyBL>>(babies);
            return Ok(babyDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BabyBL>> GetBabyById(string id)
        {
            var baby = await _bl.GetBabyById(id);
            if (baby == null)
            {
                return NotFound();
            }
            var babyDto = _mapper.Map<BabyBL>(baby);
            return Ok(babyDto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateBabyDetails(string id, [FromBody] BabyBL updatedBabyDto)
        {
            if (id != updatedBabyDto.BabyId)
                return BadRequest("ID mismatch");

            var updatedBaby = _mapper.Map<Baby>(updatedBabyDto);
            try
            {
                await _bl.UpdateBabyDetails(updatedBaby);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Baby not found");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBaby(string id)
        {
            try
            {
                await _bl.DeleteBaby(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Baby not found");
            }
        }
    }
}