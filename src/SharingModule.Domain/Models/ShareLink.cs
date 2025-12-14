using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using SharingModule.ShareLinks;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace SharingModule.Models;

/// <summary>
/// Represents a share link for a resource
/// </summary>
public class ShareLink : FullAuditedAggregateRoot<Guid>, IMultiTenant
{
    public virtual Guid? TenantId { get; protected set; }
    
    /// <summary>
    /// The unique token for accessing the shared resource
    /// </summary>
    public virtual string Token { get; private set; }
    
    /// <summary>
    /// The type of resource being shared
    /// </summary>
    public virtual ResourceType ResourceType { get; private set; }
    
    /// <summary>
    /// The ID of the resource being shared
    /// </summary>
    public virtual string ResourceId { get; private set; }
    
    /// <summary>
    /// The type of share link
    /// </summary>
    public virtual ShareLinkType LinkType { get; private set; }
    
    /// <summary>
    /// Whether the link is read-only
    /// </summary>
    public virtual bool IsReadOnly { get; private set; }
    
    /// <summary>
    /// Whether comments are allowed
    /// </summary>
    public virtual bool AllowComments { get; private set; }
    
    /// <summary>
    /// Whether anonymous access is allowed
    /// </summary>
    public virtual bool AllowAnonymous { get; private set; }
    
    /// <summary>
    /// The expiration date/time of the share link (null = never expires)
    /// </summary>
    public virtual DateTime? ExpiresAt { get; private set; }
    
    /// <summary>
    /// Whether the link has been revoked
    /// </summary>
    public virtual bool IsRevoked { get; private set; }
    
    /// <summary>
    /// When the link was revoked
    /// </summary>
    public virtual DateTime? RevokedAt { get; private set; }
    
    /// <summary>
    /// Access logs for this share link
    /// </summary>
    public virtual ICollection<ShareLinkAccessLog> AccessLogs { get; protected set; }
    
    protected ShareLink()
    {
        // For ORM
        Token = string.Empty;
        ResourceId = string.Empty;
        AccessLogs = new List<ShareLinkAccessLog>();
    }
    
    public ShareLink(
        Guid id,
        [NotNull] string token,
        ResourceType resourceType,
        [NotNull] string resourceId,
        ShareLinkType linkType = ShareLinkType.MultipleUse,
        bool isReadOnly = true,
        bool allowComments = false,
        bool allowAnonymous = true,
        DateTime? expiresAt = null,
        Guid? tenantId = null)
    {
        Id = id;
        Token = Check.NotNullOrWhiteSpace(token, nameof(token), ShareLinkConsts.MaxTokenLength);
        ResourceType = resourceType;
        ResourceId = Check.NotNullOrWhiteSpace(resourceId, nameof(resourceId), ShareLinkConsts.MaxResourceIdLength);
        LinkType = linkType;
        IsReadOnly = isReadOnly;
        AllowComments = allowComments;
        AllowAnonymous = allowAnonymous;
        ExpiresAt = expiresAt;
        IsRevoked = false;
        TenantId = tenantId;
        
        AccessLogs = new List<ShareLinkAccessLog>();
    }
    
    public virtual ShareLink SetReadOnly(bool isReadOnly)
    {
        IsReadOnly = isReadOnly;
        return this;
    }
    
    public virtual ShareLink SetAllowComments(bool allowComments)
    {
        AllowComments = allowComments;
        return this;
    }
    
    public virtual ShareLink SetAllowAnonymous(bool allowAnonymous)
    {
        AllowAnonymous = allowAnonymous;
        return this;
    }
    
    public virtual ShareLink SetExpiresAt(DateTime? expiresAt)
    {
        ExpiresAt = expiresAt;
        return this;
    }
    
    public virtual ShareLink Revoke()
    {
        IsRevoked = true;
        RevokedAt = DateTime.UtcNow;
        return this;
    }
    
    public virtual bool IsValid()
    {
        if (IsRevoked)
        {
            return false;
        }
        
        if (ExpiresAt.HasValue && ExpiresAt.Value < DateTime.UtcNow)
        {
            return false;
        }
        
        return true;
    }
    
    public virtual ShareLink AddAccessLog(ShareLinkAccessLog log)
    {
        AccessLogs.Add(log);
        return this;
    }
}
