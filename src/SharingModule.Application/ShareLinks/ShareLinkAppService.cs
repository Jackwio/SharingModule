using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using SharingModule.Managers;
using SharingModule.Models;
using SharingModule.Permissions;
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
    
    public ShareLinkAppService(
        IShareLinkRepository shareLinkRepository,
        ShareLinkManager shareLinkManager)
    {
        _shareLinkRepository = shareLinkRepository;
        _shareLinkManager = shareLinkManager;
    }
    
    // [Authorize(SharingModulePermissions.ShareLinks.Default)]
    public virtual async Task<ShareLinkWithDetailsDto> GetAsync(Guid id)
    {
        var shareLink = await _shareLinkRepository.GetAsync(id, includeDetails: true);
        return ObjectMapper.Map<ShareLink, ShareLinkWithDetailsDto>(shareLink);
    }
    
    // [Authorize(SharingModulePermissions.ShareLinks.Default)]
    public virtual async Task<PagedResultDto<ShareLinkDto>> GetListAsync(GetShareLinksInput input)
    {
        var queryable = await _shareLinkRepository.WithDetailsAsync();
        
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
            var now = DateTimeOffset.UtcNow;
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
            items.Select(x => ObjectMapper.Map<ShareLink, ShareLinkDto>(x)).ToList()
        );
    }
    
    // [Authorize(SharingModulePermissions.ShareLinks.Create)]
    public virtual async Task<ShareLinkWithDetailsDto> CreateAsync(CreateShareLinkDto input)
    {
        var shareLink = await _shareLinkManager.CreateAsync(
            input.ResourceType,
            input.ResourceId,
            input.LinkType,
            input.IsReadOnly,
            input.AllowComments,
            input.AllowAnonymous,
            input.ExpiresAt
        );
        
        return ObjectMapper.Map<ShareLink, ShareLinkWithDetailsDto>(shareLink);
    }
    
    // [Authorize(SharingModulePermissions.ShareLinks.Update)]
    public virtual async Task<ShareLinkWithDetailsDto> UpdateAsync(Guid id, UpdateShareLinkDto input)
    {
        var shareLink = await _shareLinkRepository.GetAsync(id);
        
        shareLink
            .SetReadOnly(input.IsReadOnly)
            .SetAllowComments(input.AllowComments)
            .SetAllowAnonymous(input.AllowAnonymous)
            .SetExpiresAt(input.ExpiresAt);
        
        await _shareLinkRepository.UpdateAsync(shareLink);
        
        return ObjectMapper.Map<ShareLink, ShareLinkWithDetailsDto>(shareLink);
    }
    
    // [Authorize(SharingModulePermissions.ShareLinks.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _shareLinkRepository.DeleteAsync(id);
    }
    
    // [Authorize(SharingModulePermissions.ShareLinks.Revoke)]
    public virtual async Task<ShareLinkWithDetailsDto> RevokeAsync(Guid id)
    {
        var shareLink = await _shareLinkRepository.GetAsync(id);
        shareLink.Revoke();
        await _shareLinkRepository.UpdateAsync(shareLink);
        
        return ObjectMapper.Map<ShareLink, ShareLinkWithDetailsDto>(shareLink);
    }
    
    public virtual async Task<ShareLinkWithDetailsDto> ValidateAndRecordAccessAsync(ValidateShareLinkDto input)
    {
        // Use a single manager call that validates and records access atomically and enforces non-anonymous rules
        var shareLink = await _shareLinkManager.ValidateAndRecordAccessByTokenAsync(
            input.Token,
            CurrentUser.Id,
            input.IsAnonymous,
            input.AccessedBy,
            input.IpAddress,
            input.UserAgent
        );

        return ObjectMapper.Map<ShareLink, ShareLinkWithDetailsDto>(shareLink);
    }
    
    // [Authorize(SharingModulePermissions.ShareLinks.Default)]
    public virtual async Task<ListResultDto<ShareLinkDto>> GetByResourceAsync(ResourceType resourceType, string resourceId)
    {
        var shareLinks = await _shareLinkRepository.GetListByResourceAsync(resourceType, resourceId);
        
        return new ListResultDto<ShareLinkDto>(
            shareLinks.Select(x => ObjectMapper.Map<ShareLink, ShareLinkDto>(x)).ToList()
        );
    }
}
