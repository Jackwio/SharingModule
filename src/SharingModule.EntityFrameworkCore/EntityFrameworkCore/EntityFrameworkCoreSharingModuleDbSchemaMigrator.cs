using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SharingModule.Data;
using Volo.Abp.DependencyInjection;

namespace SharingModule.EntityFrameworkCore;

public class EntityFrameworkCoreSharingModuleDbSchemaMigrator
    : ISharingModuleDbSchemaMigrator, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public EntityFrameworkCoreSharingModuleDbSchemaMigrator(
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolve the SharingModuleDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<SharingModuleDbContext>()
            .Database
            .MigrateAsync();
    }
}
