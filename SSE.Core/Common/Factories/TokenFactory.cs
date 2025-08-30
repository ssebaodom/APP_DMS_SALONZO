using Microsoft.Extensions.Configuration;
using SSE.Core.Common.Constants;
using SSE.Core.Common.Enums;
using SSE.Core.Services.Helpers;
using SSE.Core.Services.JwT;
using System;
using System.Linq;

namespace SSE.Core.Common.Factories
{
    public class TokenFactory
    {
        private readonly IJwtService jwtService;
        private readonly IConfiguration configuration;

        public TokenFactory(IJwtService jwtService, IConfiguration configuration)
        {
            this.jwtService = jwtService;
            this.configuration = configuration;
        }

        /// <summary>
        /// Tạo mới access token.
        /// </summary>
        /// <param name="tokenType"></param>
        /// <param name="userId"></param>
        /// <param name="phone_number"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        //public string createToken(TokenTypes tokenType, string userId, string user_name, string phone_number, string email)
        public string createToken(TokenTypes tokenType, JwtEntity jwtEntity = null)
        {
            switch (tokenType)
            {
                case TokenTypes.AccessToken:
                    return accessToken(jwtEntity);

                case TokenTypes.RefreshAccessToken:
                    return refreshAccessToken();
            }
            return "";
        }

        private string accessToken(JwtEntity jwtEntity)
        {
            var jwtConfig = this.configuration.GetSection(CONFIGURATION_KEYS.JWT);
            jwtEntity.JwtKey = jwtConfig.GetValue<string>(CONFIGURATION_KEYS.JWT_KEY);
            jwtEntity.Issuer = jwtConfig.GetValue<string>(CONFIGURATION_KEYS.JWT_ISSUER);
            jwtEntity.Audience = jwtConfig.GetValue<string>(CONFIGURATION_KEYS.JWT_AUDIENCE);
            jwtEntity.ExpireHours = jwtConfig.GetValue<int>(CONFIGURATION_KEYS.JWT_EXPIRE_HOUR);

            return this.jwtService.GenerateJwtToken(jwtEntity);
        }

        private string refreshAccessToken(int lengthRaw = 64)
        {
            string characters = "1234567890-=qwertyuiop[]';lkjhgfdsazxcvbnm,./";
            Random rd = new Random();
            string raw = new String(Enumerable.Repeat(characters, lengthRaw).Select(item => item[rd.Next(characters.Length)]).ToArray());
            return CryptHelper.GetHashMD5(raw);
        }
    }
}