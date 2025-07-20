namespace WEB_API.BL.API
{
    public interface IEmailService
    {
        Task SendEmail(string email, string subject, string body);
        Task SendVerificationCode(string email);
        bool ValidateOTP(string email, string otp);
    }
}