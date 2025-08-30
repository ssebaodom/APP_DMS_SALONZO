using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SSE.Core.Common.Constant
{
    public class CLAIM_NAMES
    {
        public const string USERNAME = JwtRegisteredClaimNames.Sub;
        public const string UUID = JwtRegisteredClaimNames.Jti;
        public const string USERID = ClaimTypes.NameIdentifier;
        public const string HOSTID = "hostid";
        public const string EMAIL = "email";
        public const string GENDER = "gender";
        public const string BIRTHDAY = "birth_day";
        public const string WEBSITE = "webSite";
        public const string ROLE = ClaimTypes.Role;
        public const string MOBILE_PHONE = "mobile_phone";
        public const string HOME_PHONE = "home_phone";
        public const string CITY = "city";
        public const string DISTRCIT = "district";
        public const string WARD = "ward";
        public const string ADDRESS = "address";
    }
}