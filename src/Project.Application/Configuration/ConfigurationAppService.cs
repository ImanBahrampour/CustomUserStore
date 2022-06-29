using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Runtime.Session;
using Project.Configuration.Dto;

namespace Project.Configuration
{
    [AbpAuthorize]
    public class ConfigurationAppService : ProjectAppServiceBase, IConfigurationAppService
    {
        public async Task ChangeUiTheme(ChangeUiThemeInput input)
        {
            await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), AppSettingNames.UiTheme, input.Theme);
        }
    }
}
