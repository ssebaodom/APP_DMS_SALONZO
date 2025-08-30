using Dapper;
using SSE.Common.DTO.v1;
using SSE.Core.Services.Dapper;
using SSE.DataAccess.Services.v1.Interfaces;
using System.Threading.Tasks;

namespace SSE.DataAccess.Services.v1.Implements
{
    public class UserDALService : IUserDALService
    {
        private IDapperService dapperService { get; set; }

        public UserDALService(IDapperService dapperService)
        {
            this.dapperService = dapperService;
        }

        public void SetConnectionString(string conStr)
        {
            this.dapperService.SetNewConnection(conStr);
        }

        public async Task<UserInfoDTO> GetUserByName(string userName)
        {
            DynamicParameters dynamicParameter = new DynamicParameters();
            dynamicParameter.Add("@user_name", userName);

            return await dapperService.QueryFirstOrDefaultAsync<UserInfoDTO>("app_get_user_by_name", dynamicParameter);
        }
        
    }
}