using Abp.Authorization.Users;
using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.Localization;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Runtime.Security;
using Abp.Timing;
using Abp.Zero;
using Abp.Zero.Configuration;
using Project.Authorization.Roles;
using Project.Authorization.Users;
using Project.Configuration;
using Project.Localization;
using Project.MultiTenancy;
using Project.Timing;

namespace Project
{
    [DependsOn(typeof(AbpZeroCoreModule))]
    public class ProjectCoreModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Auditing.IsEnabledForAnonymousUsers = true;

            // Declare entity types
            Configuration.Modules.Zero().EntityTypes.Tenant = typeof(Tenant);
            Configuration.Modules.Zero().EntityTypes.Role = typeof(Role);
            Configuration.Modules.Zero().EntityTypes.User = typeof(User);

            ProjectLocalizationConfigurer.Configure(Configuration.Localization);

            // Enable this line to create a multi-tenant application.
            Configuration.MultiTenancy.IsEnabled = ProjectConsts.MultiTenancyEnabled;

            // Configure roles
            AppRoleConfig.Configure(Configuration.Modules.Zero().RoleManagement);

            Configuration.Settings.Providers.Add<AppSettingProvider>();
            
            Configuration.Localization.Languages.Add(new LanguageInfo("fa", "فارسی", "famfamfam-flags ir"));
            
            Configuration.Settings.SettingEncryptionConfiguration.DefaultPassPhrase = ProjectConsts.DefaultPassPhrase;
            SimpleStringCipher.DefaultPassPhrase = ProjectConsts.DefaultPassPhrase;
            //custom userStore
            Configuration.ReplaceService<AbpUserStore<Role, User>, CustomUserStore>(DependencyLifeStyle.Transient);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(ProjectCoreModule).GetAssembly());
        }

        public override void PostInitialize()
        {
            IocManager.Resolve<AppTimes>().StartupTime = Clock.Now;
        }
    }
}
