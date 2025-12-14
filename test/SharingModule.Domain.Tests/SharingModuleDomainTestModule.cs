using Volo.Abp.Modularity;

namespace SharingModule;

[DependsOn(
    typeof(SharingModuleDomainModule),
    typeof(SharingModuleTestBaseModule)
)]
public class SharingModuleDomainTestModule : AbpModule
{

}
