using System;
using Microsoft.AspNetCore.Http;
using SharingModule.Services;
using Volo.Abp.DependencyInjection;

namespace SharingModule.Application.Services;

/// <summary>
/// Implementation of IClientIpAddressProvider that extracts the real client IP address
/// from HTTP context, handling X-Forwarded-For headers set by reverse proxies and load balancers
/// </summary>
public class ClientIpAddressProvider : IClientIpAddressProvider, ITransientDependency
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ClientIpAddressProvider(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    /// <summary>
    /// Gets the real client IP address from the current HTTP context.
    /// Checks X-Forwarded-For header first (for proxies/load balancers),
    /// then falls back to RemoteIpAddress.
    /// </summary>
    /// <returns>The client IP address, or null if not available</returns>
    public string? GetClientIpAddress()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext == null)
        {
            return null;
        }

        // First, check for X-Forwarded-For header (set by reverse proxies, load balancers, K8s ingress)
        // The X-Forwarded-For header contains a comma-separated list of IP addresses
        // The leftmost IP is the original client, subsequent IPs are proxies
        if (httpContext.Request.Headers.TryGetValue("X-Forwarded-For", out var forwardedFor))
        {
            var forwardedIps = forwardedFor.ToString().Split(',', StringSplitOptions.RemoveEmptyEntries);
            if (forwardedIps.Length > 0)
            {
                // Get the first (leftmost) IP address, which is the original client
                var clientIp = forwardedIps[0].Trim();
                if (!string.IsNullOrWhiteSpace(clientIp))
                {
                    return clientIp;
                }
            }
        }

        // Fallback to RemoteIpAddress if X-Forwarded-For is not present
        // This will be the direct connection IP (could be a proxy if not configured)
        return httpContext.Connection.RemoteIpAddress?.ToString();
    }
}
