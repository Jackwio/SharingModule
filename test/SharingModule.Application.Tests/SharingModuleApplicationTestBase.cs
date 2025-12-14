using Volo.Abp.Modularity;

namespace SharingModule;

public abstract class SharingModuleApplicationTestBase<TStartupModule> : SharingModuleTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
