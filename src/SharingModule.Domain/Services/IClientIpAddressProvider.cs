namespace SharingModule.Services;

/// <summary>
/// Service for retrieving the real client IP address from HTTP context,
/// handling reverse proxies, load balancers, and Kubernetes deployments
/// </summary>
public interface IClientIpAddressProvider
{
    /// <summary>
    /// Gets the real client IP address from the current HTTP context
    /// </summary>
    /// <returns>The client IP address, or null if not available</returns>
    string? GetClientIpAddress();
}
