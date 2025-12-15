using System;
using Volo.Abp.Application.Dtos;

namespace SharingModule.ShareLinks;

[Serializable]
public class ShareLinkDto : FullAuditedEntityDto<Guid>
{
    public string Token { get; set; } = string.Empty;
    public ResourceType ResourceType { get; set; }
    public string ResourceId { get; set; } = string.Empty;
    public ShareLinkType LinkType { get; set; }
    public bool IsReadOnly { get; set; }
    public bool AllowComments { get; set; }
    public bool AllowAnonymous { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public bool IsRevoked { get; set; }
    public DateTime? RevokedAt { get; set; }
}
