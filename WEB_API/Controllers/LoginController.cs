using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using WEB_API.BL.API;

namespace WEB_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IBabyManagementBL _babyManagement;
        private readonly IWorkerManegmentBL _workerManagement;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;
        private readonly IJwtService _jwtService;

        public LoginController(IBabyManagementBL babyManagementBL, IWorkerManegmentBL workerManagement, IEmailService emailService, IMapper mapper, IJwtService jwtService)
        {
            _babyManagement = babyManagementBL;
            _workerManagement = workerManagement;
            _emailService = emailService;
            _mapper = mapper;
            _jwtService = jwtService;
        }

   
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromQuery] string id, [FromQuery] string email)
        {
            var baby = await _babyManagement.GetBabyById(id);
            if (baby != null && baby.ParentEmail.Equals(email))
            {
                var token = _jwtService.GenerateToken(baby.BabyId, "regularUser");
                _jwtService.SetTokenCookie(Response, token);
                // כאן להחזיר גם את babyId ב-response
                return Ok(new { userExists = true, userType = "regularUser", token, babyId = baby.BabyId });
            }

            var worker = await _workerManagement.GetWorkerByIdAsync(id);
            if (worker != null && worker.Email.Equals(email))
            {
                if (worker.WorkerType.Equals("Manager"))
                {
                    var token = _jwtService.GenerateToken(worker.WorkerId, "admin");
                    _jwtService.SetTokenCookie(Response, token);
                    return Ok(new { userExists = true, userType = "admin", token });
                }
                else
                {
                    var token = _jwtService.GenerateToken(worker.WorkerId, "worker");
                    _jwtService.SetTokenCookie(Response, token);
                    return Ok(new { userExists = true, userType = "worker", token });
                }
            }

            return BadRequest(new { userExists = false, message = "User not found or invalid credentials" });
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("token");
            return Ok(new { success = true, message = "Logged out successfully" });
        }

        [HttpPost("refresh")]
        public IActionResult RefreshToken()
        {
            var token = Request.Cookies["token"];
            if (string.IsNullOrEmpty(token))
            {
                return BadRequest(new { message = "Token is required" });
            }

            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadToken(token) as JwtSecurityToken;

                if (jwtToken == null || jwtToken.ValidTo > DateTime.UtcNow)
                    return BadRequest("Token is not expired yet");

                var idClaim = jwtToken.Claims.First(claim => claim.Type == "id");
                var userTypeClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "userType");
                var userType = userTypeClaim?.Value ?? "regularUser";

                var newToken = _jwtService.RefreshToken(token);
                _jwtService.SetTokenCookie(Response, newToken);

                return Ok(new { token = newToken, user = idClaim.Value, userType });
            }
            catch (SecurityTokenException ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        [HttpPost("sendVarificationCode")]
        public async Task<IActionResult> SendVerificationCode([FromQuery] string email)
        {
            Console.WriteLine($"[SEND OTP] email={email}");
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
            Console.WriteLine($"[VALIDATE OTP] email={email}, otp={otp}");
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
