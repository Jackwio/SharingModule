using System;
using System.Diagnostics.CodeAnalysis;
using SharingModule.ShareLinks;
using Volo.Abp;
using Volo.Abp.Domain.Entities;


namespace SharingModule.Models;

/// <summary>
/// Represents an access log entry for a share link
/// </summary>
public class ShareLinkAccessLog : Entity<Guid>, IMultiWorkspace
{
    /// <summary>
    /// The workspace ID that this entity belongs to
    /// </summary>
    public virtual Guid WorkspaceId { get; private set; }
    
    /// <summary>
    /// The share link ID
    /// </summary>
    public virtual Guid ShareLinkId { get; private set; }
    
    /// <summary>
    /// When the link was accessed
    /// </summary>
    public virtual DateTime AccessedAt { get; private set; }
    
    /// <summary>
    /// Who accessed the link (user ID or identifier)
    /// </summary>
    public virtual string AccessedBy { get; private set; }
    
    /// <summary>
    /// Whether the access was anonymous
    /// </summary>
    public virtual bool IsAnonymous { get; private set; }
    
    /// <summary>
    /// IP address of the accessor
    /// </summary>
    public virtual string? IpAddress { get; private set; }
    
    /// <summary>
    /// User agent of the accessor
    /// </summary>
    public virtual string? UserAgent { get; private set; }
    
    protected ShareLinkAccessLog()
    {
        // For ORM
        AccessedBy = string.Empty;
    }
    
    public ShareLinkAccessLog(
        Guid id,
        Guid shareLinkId,
        DateTime accessedAt,
        [NotNull] string accessedBy,
        Guid workspaceId,
        bool isAnonymous,
        string ipAddress = null,
        string userAgent = null)
    {
        Id = id;
        ShareLinkId = shareLinkId;
        AccessedAt = accessedAt;
        AccessedBy = Check.NotNullOrWhiteSpace(accessedBy, nameof(accessedBy), ShareLinkConsts.MaxAccessedByLength);
        WorkspaceId = workspaceId;
        IsAnonymous = isAnonymous;
        IpAddress = ipAddress?.Length > ShareLinkConsts.MaxIpAddressLength 
            ? ipAddress.Substring(0, ShareLinkConsts.MaxIpAddressLength) 
            : ipAddress;
        UserAgent = userAgent?.Length > ShareLinkConsts.MaxUserAgentLength
            ? userAgent.Substring(0, ShareLinkConsts.MaxUserAgentLength)
            : userAgent;
    }
}
