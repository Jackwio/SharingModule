using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace SharingModule.Models;

/// <summary>
/// Repository interface for ShareLink aggregate root
/// </summary>
public interface IShareLinkRepository : IRepository<ShareLink, Guid>
{
    /// <summary>
    /// Find a share link by token (ignores workspace filtering)
    /// </summary>
    Task<ShareLink> FindByTokenAsync(
        string token, 
        bool includeDetails = false,
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Get list of active (not revoked and not expired) share links
    /// </summary>
    Task<List<ShareLink>> GetActiveListAsync(
        bool includeDetails = false,
        CancellationToken cancellationToken = default);
}
