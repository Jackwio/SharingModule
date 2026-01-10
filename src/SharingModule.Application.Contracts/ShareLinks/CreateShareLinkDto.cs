using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.ObjectExtending;

namespace SharingModule.ShareLinks;

[Serializable]
public class CreateShareLinkDto : ExtensibleObject
{
    [Required]
    public Guid ResourceId { get; set; }
    
    public ShareLinkType LinkType { get; set; } = ShareLinkType.MultipleUse;
    
    public bool IsReadOnly { get; set; } = true;
    
    public bool AllowComments { get; set; } = false;
    
    public bool AllowAnonymous { get; set; } = true;
    
    public DateTimeOffset? ExpiresAt { get; set; }
}
