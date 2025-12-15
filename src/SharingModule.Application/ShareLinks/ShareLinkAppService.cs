using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using SharingModule.Managers;
using SharingModule.Models;
using SharingModule.Permissions;
using SharingModule.Services;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace SharingModule.ShareLinks;

/// <summary>
/// Application service for managing share links
/// </summary>
public class ShareLinkAppService : ApplicationService, IShareLinkAppService
{
    private readonly IShareLinkRepository _shareLinkRepository;
    private readonly ShareLinkManager _shareLinkManager;
    private readonly SharingModuleApplicationMappers _mappers;
    private readonly IClientIpAddressProvider _clientIpAddressProvider;
    
    public ShareLinkAppService(
        IShareLinkRepository shareLinkRepository,
        ShareLinkManager shareLinkManager,
        SharingModuleApplicationMappers mappers,
        IClientIpAddressProvider clientIpAddressProvider)
    {
        _shareLinkRepository = shareLinkRepository;
        _shareLinkManager = shareLinkManager;
        _mappers = mappers;
        _clientIpAddressProvider = clientIpAddressProvider;
    }
    
    [Authorize(SharingModulePermissions.ShareLinks.Default)]
    public virtual async Task<ShareLinkWithDetailsDto> GetAsync(Guid id)
    {
        var shareLink = await _shareLinkRepository.GetAsync(id, includeDetails: true);
        return _mappers.MapWithDetails(shareLink);
    }
    
    [Authorize(SharingModulePermissions.ShareLinks.Default)]
    public virtual async Task<PagedResultDto<ShareLinkDto>> GetListAsync(GetShareLinksInput input)
    {
        var queryable = await _shareLinkRepository.GetQueryableAsync();
        
        // Apply filters
        if (input.ResourceType.HasValue)
        {
            queryable = queryable.Where(x => x.ResourceType == input.ResourceType.Value);
        }
        
        if (!string.IsNullOrWhiteSpace(input.ResourceId))
        {
            queryable = queryable.Where(x => x.ResourceId == input.ResourceId);
        }
        
        if (input.IsRevoked.HasValue)
        {
            queryable = queryable.Where(x => x.IsRevoked == input.IsRevoked.Value);
        }
        
        if (!input.IncludeExpired.GetValueOrDefault())
        {
            var now = DateTime.UtcNow;
            queryable = queryable.Where(x => x.ExpiresAt == null || x.ExpiresAt > now);
        }
        
        // Apply sorting
        if (!string.IsNullOrWhiteSpace(input.Sorting))
        {
            queryable = queryable.OrderBy(input.Sorting);
        }
        else
        {
            queryable = queryable.OrderByDescending(x => x.CreationTime);
        }
        
        var totalCount = await AsyncExecuter.CountAsync(queryable);
        
        var items = await AsyncExecuter.ToListAsync(
            queryable.Skip(input.SkipCount).Take(input.MaxResultCount)
        );
        
        return new PagedResultDto<ShareLinkDto>(
            totalCount,
            items.Select(x => _mappers.Map(x)).ToList()
        );
    }
    
    [Authorize(SharingModulePermissions.ShareLinks.Create)]
    public virtual async Task<ShareLinkWithDetailsDto> CreateAsync(CreateShareLinkDto input)
    {
        var shareLink = await _shareLinkManager.CreateAsync(
            input.ResourceType,
            input.ResourceId,
            input.LinkType,
            input.IsReadOnly,
            input.AllowComments,
            input.AllowAnonymous,
            input.ExpiresAt,
            CurrentTenant.Id
        );
        
        return _mappers.MapWithDetails(shareLink);
    }
    
    [Authorize(SharingModulePermissions.ShareLinks.Update)]
    public virtual async Task<ShareLinkWithDetailsDto> UpdateAsync(Guid id, UpdateShareLinkDto input)
    {
        var shareLink = await _shareLinkRepository.GetAsync(id);
        
        shareLink
            .SetReadOnly(input.IsReadOnly)
            .SetAllowComments(input.AllowComments)
            .SetAllowAnonymous(input.AllowAnonymous)
            .SetExpiresAt(input.ExpiresAt);
        
        await _shareLinkRepository.UpdateAsync(shareLink);
        
        return _mappers.MapWithDetails(shareLink);
    }
    
    [Authorize(SharingModulePermissions.ShareLinks.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _shareLinkRepository.DeleteAsync(id);
    }
    
    [Authorize(SharingModulePermissions.ShareLinks.Revoke)]
    public virtual async Task<ShareLinkWithDetailsDto> RevokeAsync(Guid id)
    {
        var shareLink = await _shareLinkRepository.GetAsync(id);
        shareLink.Revoke();
        await _shareLinkRepository.UpdateAsync(shareLink);
        
        return _mappers.MapWithDetails(shareLink);
    }
    
    public virtual async Task<ShareLinkWithDetailsDto> ValidateAndRecordAccessAsync(ValidateShareLinkDto input)
    {
        var shareLink = await _shareLinkManager.ValidateAndGetAsync(input.Token);
        
        var accessedBy = input.AccessedBy ?? (input.IsAnonymous ? "Anonymous" : CurrentUser.Id?.ToString() ?? "Unknown");
        
        // Automatically capture the real client IP address from HTTP context
        // This will work correctly behind reverse proxies, load balancers, and in K8s
        var ipAddress = input.IpAddress ?? _clientIpAddressProvider.GetClientIpAddress();
        
        await _shareLinkManager.RecordAccessAsync(
            shareLink,
            accessedBy,
            input.IsAnonymous,
            ipAddress,
            input.UserAgent
        );
        
        return _mappers.MapWithDetails(shareLink);
    }
    
    [Authorize(SharingModulePermissions.ShareLinks.Default)]
    public virtual async Task<ListResultDto<ShareLinkDto>> GetByResourceAsync(ResourceType resourceType, string resourceId)
    {
        var shareLinks = await _shareLinkRepository.GetListByResourceAsync(resourceType, resourceId);
        
        return new ListResultDto<ShareLinkDto>(
            shareLinks.Select(x => _mappers.Map(x)).ToList()
        );
    }
}
