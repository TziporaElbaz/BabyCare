using BL.API;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using WEB_API.BL.API;

namespace WEB_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IBabyManagementBL _babyManagementBL;
        private readonly IEmailService _emailService;

        public LoginController(IBabyManagementBL babyManagementBL, IEmailService emailService)
        {
            _babyManagementBL = babyManagementBL;
            _emailService = emailService;
        }

        [HttpPost("checkUser")]
        public async Task<IActionResult> CheckUser([FromQuery] string id, [FromQuery] string email)
        {
            var baby = await _babyManagementBL.GetBabyById(id);
            if (baby != null)
            {
                if(baby.ParentEmail.Equals(email))
                return Ok(new { exists = true });
                else return BadRequest("המשתמש לא קיים במערכת או שהאימייל שגוי");
            }
            return Ok(new { exists = false });
        }

        [HttpPost("sendVarificationCode")]
        public async Task<IActionResult> SendVerificationCode([FromQuery] string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest("Email is required");
            }
            await _emailService.SendVerificationCode(email);
            return Ok(new { success = true, message = "קוד אימות נשלח אליך ברגעים אלו" });
        }

        [HttpPost("validate")]
        public IActionResult ValidateOTP([FromQuery] string email, [FromQuery] string otp)
        {
            Console.WriteLine("In code checking");

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(otp))
            {
                return BadRequest("נתונים חסרים");
            }

            if (_emailService.ValidateOTP(email, otp))
            {
                return Ok(new { success = true });
            }
            return BadRequest("הקוד לא תקין או פג תוקף");
        }
    }
}
