using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace SharingModule.Data;

/* This is used if database provider does't define
 * ISharingModuleDbSchemaMigrator implementation.
 */
public class NullSharingModuleDbSchemaMigrator : ISharingModuleDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
