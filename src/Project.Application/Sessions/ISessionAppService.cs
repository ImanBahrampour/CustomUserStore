using System.Threading.Tasks;
using Abp.Application.Services;
using Project.Sessions.Dto;

namespace Project.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}
