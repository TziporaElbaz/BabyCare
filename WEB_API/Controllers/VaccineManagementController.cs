using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WEB_API.BL.API;
using WEB_API.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WEB_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VaccineManagementController : ControllerBase
    {
        private readonly IVaccineManagementBL _vaccineManagementBL;
        private readonly IMapper _mapper;

        public VaccineManagementController(IVaccineManagementBL vaccineManagementBL, IMapper mapper)
        {
            _vaccineManagementBL = vaccineManagementBL;
            _mapper = mapper;
        }

        // GET: api/VaccineManagement/baby/{id}/vaccines
        [HttpGet("baby/{id}/vaccines")]
        public async Task<ActionResult<Dictionary<Vaccine, bool>>> GetListOfBabysVaccines(string id)
        {
            var vaccines =await _vaccineManagementBL.ListOfBabysVaccines(id);
            return Ok(vaccines);
        }

        // GET: api/VaccineManagement/baby/{id}/unvaccinated
        [HttpGet("baby/{id}/unvaccinated")]
        public async Task< ActionResult<List<Vaccine>>> GetListOfBabysUnvaccinatedVaccines(string id)
        {
            var vaccines = await _vaccineManagementBL.ListOfBabysUnvaccinatedVaccines(id);
            return Ok(vaccines);
        }

        // GET: api/VaccineManagement/baby/{id}/upcoming
        [HttpGet("baby/{id}/upcoming")]
        public async Task< ActionResult<List<Vaccine>>> GetUpcomingVaccines(string id)
        {
            var vaccines =await _vaccineManagementBL.ShowUpcomingVaccines(id);
            return Ok(vaccines);
        }
    }
}
