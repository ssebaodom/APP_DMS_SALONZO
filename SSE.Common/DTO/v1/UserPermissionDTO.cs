using System.Collections.Generic;

namespace SSE.Common.DTO.v1
{
    public class UserPermissionDTO
    {
        public string menuId { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string icon { get; set; }
        public string parentMenuId { get; set; }
    }

    public class UserPermissionAccountDTO
    {
        public string name { get; set; }
        public int value { get; set; }
    }
}