using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SharingModule.ShareLinks;

/// <summary>
/// Application service interface for managing share links
/// </summary>
public interface IShareLinkAppService : IApplicationService
{
    /// <summary>
    /// Get a share link by ID
    /// </summary>
    Task<ShareLinkWithDetailsDto> GetAsync(Guid id);
    
    /// <summary>
    /// Get a list of share links
    /// </summary>
    Task<PagedResultDto<ShareLinkDto>> GetListAsync(GetShareLinksInput input);
    
    /// <summary>
    /// Create a new share link
    /// </summary>
    Task<ShareLinkWithDetailsDto> CreateAsync(CreateShareLinkDto input);
    
    /// <summary>
    /// Update an existing share link
    /// </summary>
    Task<ShareLinkWithDetailsDto> UpdateAsync(Guid id, UpdateShareLinkDto input);
    
    /// <summary>
    /// Delete a share link
    /// </summary>
    Task DeleteAsync(Guid id);
    
    /// <summary>
    /// Revoke a share link
    /// </summary>
    Task<ShareLinkWithDetailsDto> RevokeAsync(Guid id);
    
    /// <summary>
    /// Validate a share link token and record access
    /// </summary>
    Task<ShareLinkWithDetailsDto> ValidateAndRecordAccessAsync(ValidateShareLinkDto input);
}
