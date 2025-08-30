using SSE.Core.Services.JwT;

namespace SSE.IntegrationTest.Services
{
    public class TokenService
    {
        private static string JwtKey = "12b6fb24-adb8-4ce5-aa49-79b265ebf256";
        private static string audience = "yourdomain.com";
        private static string issuer = "yourdomain.com";
        private static string userId = "15";
        private static string userName = "DUNGDD";
        private static string hostId = "00";
        private static string companyId = "CTY";
        private static int role = 1;
        private static string phoneNumber = "0964898335";
        private static string email = "hoivd@sse.net.vn";

        private static TokenService tokenService = null;
        public JwtService jwtService { get; private set; }

        public static TokenService Instance
        {
            get
            {
                if (tokenService == null)
                {
                    tokenService = new TokenService();
                }
                return tokenService;
            }
        }

        private TokenService()
        {
            jwtService = new JwtService();
        }

        public string GetToken()
        {
            JwtEntity jwtEntity = new JwtEntity
            {
                JwtKey = JwtKey,
                UserId = userId,
                UserName = userName,
                HostId = hostId,
                Role = role,
                PhoneNumber = phoneNumber,
                Email = email,
                Audience = audience,
                Issuer = issuer
            };

            return jwtService.GenerateJwtToken(jwtEntity);
        }
    }
}