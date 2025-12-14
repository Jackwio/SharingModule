using Volo.Abp.Modularity;

namespace SharingModule;

[DependsOn(
    typeof(SharingModuleApplicationModule),
    typeof(SharingModuleDomainTestModule)
)]
public class SharingModuleApplicationTestModule : AbpModule
{

}
