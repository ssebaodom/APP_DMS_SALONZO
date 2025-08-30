namespace SSE.Core.Common.Entities
{
    public partial class UserInfoCache
    {
        public long UserId { get; set; }
        public string UserName { get; set; }
        public string HostId { set; get; }
        public string UnitId { set; get; }
        public string StoreId { set; get; }
        public string CompanyId { set; get; }
        public string PhoneNumber { set; get; }
        public string Email { set; get; }
        public int Role { set; get; } = 0;
        public string ServerName { get; set; }
        public string SqlUserLogin { get; set; }
        public string SqlPassLogin { get; set; }
        public string DbSysName { set; get; }
        public string DbAppName { set; get; }
        public string Lang { set; get; } = "v";
        public string DeviceToken { set; get; }
    }
}