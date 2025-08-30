using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using SSE.Core.Common.Constant;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SSE.Core.Services.JwT
{
    public class JwtService : IJwtService
    {
        private JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
        private TokenValidationParameters tokenValidationParameters = null;

        public JwtService()
        {
        }

        private TokenValidationParameters CreateTokenValidationParameters(string JwTkey, string ValidIssuer, string ValidAudience, bool checkExpired = true)
        {
            return new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true, // xác thực chữ ký
                ValidateAudience = true, // xác thực người ủy quyền
                ValidateIssuer = true,   // xác thực máy chủ
                ValidateLifetime = checkExpired, // xác thực thời gian sống token
                ValidIssuer = ValidIssuer,
                ValidAudience = ValidAudience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwTkey)),
                //LifetimeValidator = CheckLastLogin,
                ClockSkew = TimeSpan.Zero
            };
        }

        private bool CheckLastLogin(DateTime? notBefore, DateTime? expires, SecurityToken securityToken, TokenValidationParameters validationParameters)
        {
            DateTime issueAt = securityToken.ValidFrom;
            DateTime lastLogin = DateTime.Now;

            var jwtSecurityHandler = new JwtSecurityTokenHandler();
            SecurityToken jwtToken;
            var claimsPrincipal = jwtSecurityHandler.ValidateToken(securityToken.ToString(), tokenValidationParameters, out jwtToken);

            var x1 = claimsPrincipal.Identities;
            var x2 = claimsPrincipal.Claims;
            var x3 = claimsPrincipal.Identity;

            //return issueAt <= lastLogin ? true : false;

            if (expires != null)
            {
                if (DateTime.UtcNow < expires) return true;
            }
            return false;
        }

        public bool ValidateToken(string token, string JwTkey, string ValidIssuer, string ValidAudience, bool checkExpired = false)
        {
            var tokenValidationParameters = CreateTokenValidationParameters(JwTkey, ValidIssuer, ValidAudience, checkExpired);
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            try
            {
                tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            }
            catch (Exception)
            {
                return false;
            }
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                return false;

            return true;
        }

        public TokenValidationParameters GetValidationParameters(string ValidIssuer, string ValidAudience, string JwtKey)
        {
            if (tokenValidationParameters == null)
            {
                tokenValidationParameters = CreateTokenValidationParameters(JwtKey, ValidIssuer, ValidAudience, true);
            }
            return tokenValidationParameters;
        }

        public string GenerateJwtToken(JwtEntity jwtEntity)
        {
            var claims = new List<Claim>
            {
                new Claim(CLAIM_NAMES.UUID, Guid.NewGuid().ToString()),
                new Claim(CLAIM_NAMES.USERID, jwtEntity.UserId),
                new Claim(CLAIM_NAMES.USERNAME, jwtEntity.UserName),
                new Claim(CLAIM_NAMES.HOSTID, jwtEntity.HostId),
                new Claim(CLAIM_NAMES.ROLE, jwtEntity.Role.ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString())
            };
            if (jwtEntity.PhoneNumber != null)
            {
                claims.Add(new Claim(CLAIM_NAMES.MOBILE_PHONE, jwtEntity.PhoneNumber));
            }
            if (jwtEntity.Email != null)
            {
                claims.Add(new Claim(CLAIM_NAMES.EMAIL, jwtEntity.Email));
            }
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtEntity.JwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddHours(jwtEntity.ExpireHours);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = expires,
                SigningCredentials = creds,
                Issuer = jwtEntity.Issuer,
                Audience = jwtEntity.Audience,
                IssuedAt = DateTime.UtcNow,
                NotBefore = DateTime.UtcNow,
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public IEnumerable<Claim> GetClaims(string token)
        {
            var JwtSecurityToken = tokenHandler.ReadJwtToken(token);
            return JwtSecurityToken.Claims;
        }

        public string GetToken(IHeaderDictionary headers)
        {
            string authHeader = headers["Authorization"];
            return authHeader.Replace("Bearer ", "");
        }

        public string GetTokenRefresh(IHeaderDictionary headers)
        {
            return headers["Refresh-Access-Token"].ToString();
        }
    }
}