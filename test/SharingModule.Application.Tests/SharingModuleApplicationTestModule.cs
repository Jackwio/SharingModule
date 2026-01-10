using Microsoft.Extensions.DependencyInjection;
using SharingModule.Services;
using Volo.Abp.Modularity;

namespace SharingModule;

[DependsOn(
    typeof(SharingModuleApplicationModule),
    typeof(SharingModuleDomainTestModule)
)]
public class SharingModuleApplicationTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        // Register a stub IClientIpAddressProvider for testing
        // In test environment, HttpContext is not available
        context.Services.AddTransient<IClientIpAddressProvider, StubClientIpAddressProvider>();
    }
}

/// <summary>
/// Stub implementation of IClientIpAddressProvider for testing
/// </summary>
public class StubClientIpAddressProvider : IClientIpAddressProvider
{
    public string? GetClientIpAddress()
    {
        // In test environment, return null as HttpContext is not available
        return null;
    }
}
