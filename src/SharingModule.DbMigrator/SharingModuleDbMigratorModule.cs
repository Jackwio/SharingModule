using SharingModule.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace SharingModule.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(SharingModuleEntityFrameworkCoreModule),
    typeof(SharingModuleApplicationContractsModule)
    )]
public class SharingModuleDbMigratorModule : AbpModule
{
}
