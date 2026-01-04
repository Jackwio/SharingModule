using Microsoft.Extensions.DependencyInjection;
using SharingModule.Data;
using Volo.Abp.Modularity;

namespace SharingModule;

[DependsOn(
    typeof(SharingModuleDomainModule),
    typeof(SharingModuleTestBaseModule)
)]
public class SharingModuleDomainTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        // Register a test implementation of ICurrentWorkspace
        context.Services.AddSingleton<ICurrentWorkspace, TestCurrentWorkspace>();
    }
}
