using System;
using Volo.Abp.Application.Dtos;

namespace SharingModule.ShareLinks;

[Serializable]
public class ShareLinkAccessLogDto : EntityDto<Guid>
{
    public Guid ShareLinkId { get; set; }
    public DateTime AccessedAt { get; set; }
    public string AccessedBy { get; set; } = string.Empty;
    public bool IsAnonymous { get; set; }
    public string? IpAddress { get; set; }
    public string? UserAgent { get; set; }
}
