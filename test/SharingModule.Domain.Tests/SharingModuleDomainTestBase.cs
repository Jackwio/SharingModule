using Volo.Abp.Modularity;

namespace SharingModule;

/* Inherit from this class for your domain layer tests. */
public abstract class SharingModuleDomainTestBase<TStartupModule> : SharingModuleTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
