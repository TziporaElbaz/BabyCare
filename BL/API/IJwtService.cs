using Microsoft.AspNetCore.Http;

namespace WEB_API.BL.API
{
    public interface IJwtService
    {
        string GenerateToken(string id, string userType);
        string RefreshToken(string token);
        void SetTokenCookie(HttpResponse response, string token);
    }
}
