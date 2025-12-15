using System;
using Volo.Abp.Application.Dtos;

namespace SharingModule.ShareLinks;

[Serializable]
public class GetShareLinksInput : PagedAndSortedResultRequestDto
{
    public ResourceType? ResourceType { get; set; }
    
    public string? ResourceId { get; set; }
    
    public bool? IsRevoked { get; set; }
    
    public bool? IncludeExpired { get; set; }
}
