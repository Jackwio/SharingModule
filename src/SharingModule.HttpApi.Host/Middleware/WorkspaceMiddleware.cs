using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SharingModule.Data;

namespace SharingModule.Middleware;

/// <summary>
/// Resolves workspace id from request and applies it to ICurrentWorkspace scope.
/// </summary>
public class WorkspaceMiddleware
{
    private const string HeaderName = "X-Workspace-Id";
    private const string QueryName = "workspaceId";

    private readonly RequestDelegate _next;
    private readonly ILogger<WorkspaceMiddleware> _logger;

    public WorkspaceMiddleware(RequestDelegate next, ILogger<WorkspaceMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, ICurrentWorkspace currentWorkspace)
    {
        var rawWorkspaceId = context.Request.Headers[HeaderName].FirstOrDefault()
                             ?? context.Request.Query[QueryName].FirstOrDefault();

        if (string.IsNullOrWhiteSpace(rawWorkspaceId))
        {
            await _next(context);
            return;
        }

        if (!Guid.TryParse(rawWorkspaceId, out var workspaceId))
        {
            _logger.LogWarning("Invalid workspace id value: {WorkspaceId}", rawWorkspaceId);
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsync("Invalid workspaceId");
            return;
        }

        using (currentWorkspace.Change(workspaceId))
        {
            await _next(context);
        }
    }
}

public static class WorkspaceMiddlewareExtensions
{
    public static IApplicationBuilder UseWorkspaceContext(this IApplicationBuilder app)
    {
        return app.UseMiddleware<WorkspaceMiddleware>();
    }
}
