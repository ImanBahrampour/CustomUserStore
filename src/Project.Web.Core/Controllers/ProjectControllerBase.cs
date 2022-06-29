using Abp.AspNetCore.Mvc.Controllers;
using Abp.IdentityFramework;
using Microsoft.AspNetCore.Identity;

namespace Project.Controllers
{
    public abstract class ProjectControllerBase: AbpController
    {
        protected ProjectControllerBase()
        {
            LocalizationSourceName = ProjectConsts.LocalizationSourceName;
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
