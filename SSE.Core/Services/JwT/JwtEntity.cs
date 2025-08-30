namespace SSE.Core.Services.JwT
{
    public class JwtEntity
    {
        public string JwtKey { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string HostId { set; get; }
        public int Role { set; get; }
        public string PhoneNumber { set; get; }
        public string Email { set; get; }
        public string Audience { set; get; }
        public string Issuer { set; get; }
        public int ExpireHours { set; get; } = 8;
    }
}