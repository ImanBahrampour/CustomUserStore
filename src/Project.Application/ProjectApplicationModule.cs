using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Project.Authorization;

namespace Project
{
    [DependsOn(
        typeof(ProjectCoreModule), 
        typeof(AbpAutoMapperModule))]
    public class ProjectApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Authorization.Providers.Add<ProjectAuthorizationProvider>();
        }

        public override void Initialize()
        {
            var thisAssembly = typeof(ProjectApplicationModule).GetAssembly();

            IocManager.RegisterAssemblyByConvention(thisAssembly);

            Configuration.Modules.AbpAutoMapper().Configurators.Add(
                // Scan the assembly for classes which inherit from AutoMapper.Profile
                cfg => cfg.AddMaps(thisAssembly)
            );
        }
    }
}
