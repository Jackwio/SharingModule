using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using SharingModule.ShareLinks;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;


namespace SharingModule.Models;

/// <summary>
/// Represents a share link for a resource
/// </summary>
public class ShareLink : FullAuditedAggregateRoot<Guid>, IMultiWorkspace
{
    /// <summary>
    /// The workspace ID that this entity belongs to
    /// </summary>
    public virtual Guid WorkspaceId { get; private set; }
    
    /// <summary>
    /// The unique token for accessing the shared resource
    /// </summary>
    public virtual string Token { get; private set; }
    
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
    /// The expiration date/time of the share link (null = never expires). Uses DateTimeOffset to preserve timezone/offset info.
    /// </summary>
    public virtual DateTimeOffset? ExpiresAt { get; private set; }
    
    /// <summary>
    /// Whether the link has been revoked
    /// </summary>
    public virtual bool IsRevoked { get; private set; }
    
    /// <summary>
    /// When the link was revoked
    /// </summary>
    public virtual DateTime? RevokedAt { get; private set; }

    /// <summary>
    /// Concurrency token (rowversion) to prevent race conditions on single-use links
    /// </summary>
    [System.ComponentModel.DataAnnotations.Timestamp]
    public virtual byte[]? RowVersion { get; protected set; }
    
    /// <summary>
    /// Access logs for this share link
    /// </summary>
    public virtual ICollection<ShareLinkAccessLog> AccessLogs { get; protected set; }
    
    protected ShareLink()
    {
        // For ORM
        Token = string.Empty;
        AccessLogs = new List<ShareLinkAccessLog>();
    }
    
    public ShareLink(
        Guid id,
        [NotNull] string token,
        Guid workspaceId,
        ShareLinkType linkType = ShareLinkType.MultipleUse,
        bool isReadOnly = true,
        bool allowComments = false,
        bool allowAnonymous = true,
        DateTimeOffset? expiresAt = null)
    {
        Id = id;
        Token = Check.NotNullOrWhiteSpace(token, nameof(token), ShareLinkConsts.MaxTokenLength);
        WorkspaceId = workspaceId;
        LinkType = linkType;
        IsReadOnly = isReadOnly;
        AllowComments = allowComments;
        AllowAnonymous = allowAnonymous;
        ExpiresAt = expiresAt;
        IsRevoked = false;
        
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
    
    public virtual ShareLink SetExpiresAt(DateTimeOffset? expiresAt)
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
        
        if (ExpiresAt.HasValue && ExpiresAt.Value < DateTimeOffset.UtcNow)
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
