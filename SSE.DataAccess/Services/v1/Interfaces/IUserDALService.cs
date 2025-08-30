using SSE.Common.DTO.v1;
using System.Threading.Tasks;

namespace SSE.DataAccess.Services.v1.Interfaces
{
    public interface IUserDALService
    {
        void SetConnectionString(string conStr);

        Task<UserInfoDTO> GetUserByName(string userName);
    }
}