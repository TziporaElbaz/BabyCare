namespace BL.API
{
    public interface IEmailService
    {
        Task SendVerificationCode(string email);
        bool ValidateOTP(string email, string otp);
    }
}