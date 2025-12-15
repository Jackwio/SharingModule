using System;
using System.ComponentModel.DataAnnotations;

namespace SharingModule.ShareLinks;

[Serializable]
public class ValidateShareLinkDto
{
    [Required]
    [StringLength(ShareLinkConsts.MaxTokenLength)]
    public string Token { get; set; } = string.Empty;
    
    public string? AccessedBy { get; set; }
    
    public bool IsAnonymous { get; set; } = true;
    
    public string? IpAddress { get; set; }
    
    public string? UserAgent { get; set; }
}
