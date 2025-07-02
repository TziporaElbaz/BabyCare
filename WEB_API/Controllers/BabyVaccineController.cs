using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WEB_API.BL.API;
using WEB_API.BL.Models; // הנח שיש לך מודל DTO
using WEB_API.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WEB_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BabyVaccineController : ControllerBase
    {
        private readonly IBabyVaccineManagementBL _babyVaccineManagementBL;
        private readonly IMapper _mapper;

        public BabyVaccineController(IBabyVaccineManagementBL babyVaccineManagementBL, IMapper mapper)
        {
            _babyVaccineManagementBL = babyVaccineManagementBL;
            _mapper = mapper;
        }

        // יצירת חיסון חדש
        [HttpPost]
        public async Task<ActionResult> CreateBabyVaccine([FromQuery] string babyId, [FromQuery] int vaccineId)
        {
            var result = await _babyVaccineManagementBL.CreateBabyVaccineAsync(babyId, vaccineId);
            return Ok();
        }

        // קבלת חיסונים לפי מזהה תינוק
        [HttpGet("{babyId}")]
        public async Task<ActionResult<List<Vaccine>>> GetVaccinesByBabyId(string babyId)
        {
            var vaccines = await _babyVaccineManagementBL.GetVaccinesByBabyIdAsync(babyId);
            if (vaccines == null || vaccines.Count == 0)
            {
                return NotFound();
            }
            return Ok(vaccines);
        }

        // קבלת כל החיסונים
        [HttpGet]
        //public async Task<ActionResult<IEnumerable<BabyVaccine>>> GetAllBabyVaccines()
        //{
        //    var babyVaccines = await _babyVaccineManagementBL.GetAllBabyVaccinesAsync();
        //    return Ok(babyVaccines);
        //}

     
       //[HttpPut]
       // public async Task<IActionResult> UpdateBabyVaccine([FromBody] string babyId,[FromBody]  )
       // {
       //     var babyVaccineEntity = _mapper.Map<BabyVaccine>(babyVaccineDto);
       //     var updatedVaccine = await _babyVaccineManagementBL.UpdateBabyVaccineAsync(babyVaccineEntity);
       //     return Ok(updatedVaccine);
       // }

        // מחיקת חיסון
        [HttpDelete("delete{babyId}/{vaccineId}")]
        public async Task<IActionResult> DeleteBabyVaccine(string babyId, string vaccineId)
        {
            await _babyVaccineManagementBL.DeleteBabyVaccineAsync(babyId, vaccineId);
            return NoContent();
        }
    }
}
