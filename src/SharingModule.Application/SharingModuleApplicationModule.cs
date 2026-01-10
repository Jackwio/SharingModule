using Volo.Abp.Account;
using Volo.Abp.Mapperly;
using Volo.Abp.AutoMapper;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SettingManagement;
using Volo.Abp.TenantManagement;
using Microsoft.Extensions.DependencyInjection;

namespace SharingModule;

[DependsOn(
    typeof(SharingModuleDomainModule),
    typeof(AbpAccountApplicationModule),
    typeof(SharingModuleApplicationContractsModule),
    typeof(AbpIdentityApplicationModule),
    typeof(AbpPermissionManagementApplicationModule),
    typeof(AbpTenantManagementApplicationModule),
    typeof(AbpFeatureManagementApplicationModule),
    typeof(AbpSettingManagementApplicationModule),
    typeof(AbpAutoMapperModule)
    )]
public class SharingModuleApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        // Register HttpContextAccessor
        context.Services.AddHttpContextAccessor();
        
        // Register AutoMapper profile for ABP's AutoMapper integration
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddProfile<SharingModuleApplicationAutoMapperProfile>(validate: true);
        });
    }
}
