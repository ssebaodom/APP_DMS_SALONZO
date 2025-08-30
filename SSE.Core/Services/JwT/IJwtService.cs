using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;

namespace SSE.Core.Services.JwT
{
    public interface IJwtService
    {
        bool ValidateToken(string token, string JwTkey, string ValidIssuer, string ValidAudience, bool checkExpired = false);

        TokenValidationParameters GetValidationParameters(string ValidIssuer, string ValidAudience, string JwtKey);

        string GenerateJwtToken(JwtEntity jwtEntity);

        string GetToken(IHeaderDictionary headers);

        string GetTokenRefresh(IHeaderDictionary headers);
    }
}