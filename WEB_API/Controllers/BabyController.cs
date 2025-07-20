using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WEB_API.BL.API;
using WEB_API.BL.Models;
using WEB_API.DAL.Models;

namespace WEB_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BabiesController : ControllerBase
    {
        private readonly IBabyManagementBL _bl;
        private readonly IMapper _mapper;
        private readonly IJwtService _jwtService;

        public BabiesController(IBabyManagementBL bl, IMapper mapper, IJwtService jwtService)
        {
            _bl = bl;
            _mapper = mapper;
            _jwtService = jwtService;
        }

        [HttpPost("addBaby")]
        public async Task<ActionResult<BabyDTO>> AddBaby([FromBody] BabyDTO babyDto)
        {
            try
            {
                await _bl.AddBaby(babyDto);

                var token = _jwtService.GenerateToken(babyDto.BabyId, "regularUser");

                _jwtService.SetTokenCookie(Response, token);

                return CreatedAtAction(nameof(GetBabyById), new { id = babyDto.BabyId }, new { baby = babyDto, token });
            }
            catch (Exception ex)
            {
                return BadRequest($"Error adding baby: {ex.Message}");
            }
        }

        [HttpGet("getAllBabies")]
        public async Task<ActionResult<List<BabyDTO>>> GetAllBabies()
        {
            var babies = await _bl.GetAllBabies();
            return Ok(babies);
        }

        [HttpGet("getBaby/{id}")]
        public async Task<ActionResult<BabyDTO>> GetBabyById(string id)
        {
            var baby = await _bl.GetBabyById(id);
            if (baby == null)
            {
                return NotFound("baby not found");
            }
            return Ok(baby);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateBabyDetails(string id, [FromBody] BabyDTO updatedBabyDto)
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
