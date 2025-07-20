using System.Net;
using System.Net.Mail;
using WEB_API.BL.API;

namespace WEB_API.BL.Services
{
    public class EmailService : IEmailService
    {

        private readonly Dictionary<string, (string Code, DateTime Expiration)> _otpStore = new();

        public async Task SendEmail(string email, string subject, string body)
        {
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("tipatchalav1234@gmail.com", "pxbb uepy gvjn gnxj"),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress("tipatchalav1234@gmail.com"),
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
            };
            mailMessage.To.Add(email);

            try
            {
                await smtpClient.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
            }
        }

        public async Task SendVerificationCode(string email)
        {

            var otp = GenerateOTP();
            _otpStore[email] = (otp, DateTime.Now.AddMinutes(15));
            await SendEmail(email, "קוד אימות חד-פעמי", $"קוד האימות שלך הוא: {otp}. הקוד תקף ל-15 דקות.");
        }

        private string GenerateOTP(int length = 6)
        {
            Random random = new Random();
            string characters = "0123456789";
            char[] otp = new char[length];

            for (int i = 0; i < length; i++)
            {
                otp[i] = characters[random.Next(characters.Length)];
            }

            return new string(otp);
        }

        public bool ValidateOTP(string email, string otp)
        {
            if (_otpStore.TryGetValue(email, out var storedOtp))
            {
                if (DateTime.Now <= storedOtp.Expiration && storedOtp.Code == otp)
                {
                    _otpStore.Remove(email);
                    return true;
                }
            }
            return false;
        }
    }
}
