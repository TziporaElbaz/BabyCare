using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WEB_API.BL.API;
using WEB_API.BL.Models;

namespace WEB_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VaccineController : ControllerBase
    {
        private readonly IVaccineManagementBL _vaccineManagementBL;
        private readonly IMapper _mapper;

        public VaccineController(IVaccineManagementBL vaccineManagementBL, IMapper mapper)
        {
            _vaccineManagementBL = vaccineManagementBL;
            _mapper = mapper;
        }

        // GET: api/VaccineManagement/baby/{id}/vaccines
        [HttpGet("vaccinated/{babyId}")]
        public async Task<ActionResult<Dictionary<string, string>>> GetListOfBabysVaccines(string babyId)
        {
            var vaccines = await _vaccineManagementBL.ListOfBabysVaccines(babyId);
            return Ok(vaccines);
        }

        // GET: api/VaccineManagement/baby/{id}/unvaccinated
        [HttpGet("unvaccinated/{babyId}")]
        public async Task<ActionResult<List<VaccineDTO>>> GetListOfBabysUnvaccinatedVaccines(string babyId)
        {
            var vaccines = await _vaccineManagementBL.ListOfBabysUnvaccinatedVaccines(babyId);
            return Ok(vaccines);
        }

        // GET: api/VaccineManagement/baby/{id}/upcoming
        [HttpGet("upcoming/{babyId}")]
        public async Task<ActionResult<List<VaccineDTO>>> GetUpcomingVaccines(string babyId)
        {
            var vaccines = await _vaccineManagementBL.ShowUpcomingVaccines(babyId);
            return Ok(vaccines);
        }
    }
}
