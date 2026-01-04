using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SharingModule.Models;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace SharingModule.EntityFrameworkCore.Repositories;

/// <summary>
/// Repository implementation for ShareLink
/// </summary>
public class ShareLinkRepository : EfCoreRepository<SharingModuleDbContext, ShareLink, Guid>, IShareLinkRepository
{
    public ShareLinkRepository(IDbContextProvider<SharingModuleDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
    
    public virtual async Task<ShareLink> FindByTokenAsync(
        string token, 
        bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        var dbSet = await GetDbSetAsync();
        
        // IgnoreQueryFilters() 禁用所有已配置的查詢過濾器 (包括 HasQueryFilter)
        return await dbSet
            .IgnoreQueryFilters()
            .IncludeDetails(includeDetails)
            .FirstOrDefaultAsync(x => x.Token == token, GetCancellationToken(cancellationToken));
    }
    
    public virtual async Task<List<ShareLink>> GetActiveListAsync(
        bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        var dbSet = await GetDbSetAsync();
        var now = DateTimeOffset.UtcNow;
        
        return await dbSet
            .IncludeDetails(includeDetails)
            .Where(x => !x.IsRevoked && (x.ExpiresAt == null || x.ExpiresAt > now))
            .ToListAsync(GetCancellationToken(cancellationToken));
    }
    
    public override async Task<IQueryable<ShareLink>> WithDetailsAsync()
    {
        return (await GetQueryableAsync()).IncludeDetails();
    }
}

/// <summary>
/// Extension methods for ShareLink queryable
/// </summary>
public static class ShareLinkRepositoryExtensions
{
    public static IQueryable<ShareLink> IncludeDetails(this IQueryable<ShareLink> queryable, bool include = true)
    {
        if (!include)
        {
            return queryable;
        }

        return queryable
            .Include(x => x.AccessLogs);
    }
}
