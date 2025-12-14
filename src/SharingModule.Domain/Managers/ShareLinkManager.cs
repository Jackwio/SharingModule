using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using SharingModule.Models;
using SharingModule.ShareLinks;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace SharingModule.Managers;

/// <summary>
/// Domain service for managing share links
/// </summary>
public class ShareLinkManager : DomainService
{
    private readonly IShareLinkRepository _shareLinkRepository;
    
    public ShareLinkManager(IShareLinkRepository shareLinkRepository)
    {
        _shareLinkRepository = shareLinkRepository;
    }
    
    /// <summary>
    /// Create a new share link with a unique token
    /// </summary>
    public virtual async Task<ShareLink> CreateAsync(
        ResourceType resourceType,
        string resourceId,
        ShareLinkType linkType = ShareLinkType.MultipleUse,
        bool isReadOnly = true,
        bool allowComments = false,
        bool allowAnonymous = true,
        DateTime? expiresAt = null,
        Guid? tenantId = null)
    {
        var token = GenerateUniqueToken();
        
        // Ensure token is unique
        var existingLink = await _shareLinkRepository.FindByTokenAsync(token);
        while (existingLink != null)
        {
            token = GenerateUniqueToken();
            existingLink = await _shareLinkRepository.FindByTokenAsync(token);
        }
        
        var shareLink = new ShareLink(
            GuidGenerator.Create(),
            token,
            resourceType,
            resourceId,
            linkType,
            isReadOnly,
            allowComments,
            allowAnonymous,
            expiresAt,
            tenantId
        );
        
        return await _shareLinkRepository.InsertAsync(shareLink);
    }
    
    /// <summary>
    /// Validate and get a share link by token
    /// </summary>
    public virtual async Task<ShareLink> ValidateAndGetAsync(string token)
    {
        var shareLink = await _shareLinkRepository.FindByTokenAsync(token, includeDetails: true);
        
        if (shareLink == null)
        {
            throw new BusinessException(SharingModuleDomainErrorCodes.ShareLinkNotFound)
                .WithData("Token", token);
        }
        
        if (!shareLink.IsValid())
        {
            if (shareLink.IsRevoked)
            {
                throw new BusinessException(SharingModuleDomainErrorCodes.ShareLinkRevoked)
                    .WithData("Token", token);
            }
            
            if (shareLink.ExpiresAt.HasValue && shareLink.ExpiresAt.Value < DateTime.UtcNow)
            {
                throw new BusinessException(SharingModuleDomainErrorCodes.ShareLinkExpired)
                    .WithData("Token", token)
                    .WithData("ExpiresAt", shareLink.ExpiresAt.Value);
            }
        }
        
        return shareLink;
    }
    
    /// <summary>
    /// Record access to a share link
    /// </summary>
    public virtual async Task<ShareLink> RecordAccessAsync(
        ShareLink shareLink,
        string accessedBy,
        bool isAnonymous,
        string ipAddress = null,
        string userAgent = null)
    {
        Check.NotNull(shareLink, nameof(shareLink));
        
        var accessLog = new ShareLinkAccessLog(
            GuidGenerator.Create(),
            shareLink.Id,
            DateTime.UtcNow,
            accessedBy,
            isAnonymous,
            ipAddress,
            userAgent,
            shareLink.TenantId
        );
        
        shareLink.AddAccessLog(accessLog);
        
        // If it's a single-use link, revoke it after first access
        if (shareLink.LinkType == ShareLinkType.SingleUse)
        {
            shareLink.Revoke();
        }
        
        await _shareLinkRepository.UpdateAsync(shareLink);
        
        return shareLink;
    }
    
    /// <summary>
    /// Generate a unique cryptographically secure token
    /// </summary>
    private string GenerateUniqueToken()
    {
        var bytes = new byte[32];
        RandomNumberGenerator.Fill(bytes);
        return Convert.ToBase64String(bytes)
            .Replace("+", "-")
            .Replace("/", "_")
            .Replace("=", "");
    }
}
