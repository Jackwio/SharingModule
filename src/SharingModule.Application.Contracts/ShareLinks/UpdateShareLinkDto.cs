using System;
using Volo.Abp.ObjectExtending;

namespace SharingModule.ShareLinks;

[Serializable]
public class UpdateShareLinkDto : ExtensibleObject
{
    public bool IsReadOnly { get; set; }
    
    public bool AllowComments { get; set; }
    
    public bool AllowAnonymous { get; set; }
    
    public DateTimeOffset? ExpiresAt { get; set; }
}
