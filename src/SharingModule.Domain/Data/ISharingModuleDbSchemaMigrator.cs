using System.Threading.Tasks;

namespace SharingModule.Data;

public interface ISharingModuleDbSchemaMigrator
{
    Task MigrateAsync();
}
