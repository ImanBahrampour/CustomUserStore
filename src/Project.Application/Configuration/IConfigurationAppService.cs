using System.Threading.Tasks;
using Project.Configuration.Dto;

namespace Project.Configuration
{
    public interface IConfigurationAppService
    {
        Task ChangeUiTheme(ChangeUiThemeInput input);
    }
}
