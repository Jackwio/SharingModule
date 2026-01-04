using System;
using Volo.Abp.Application.Dtos;

namespace SharingModule.ShareLinks;

[Serializable]
public class GetShareLinksInput : PagedAndSortedResultRequestDto
{
    public bool? IsRevoked { get; set; }
    
    public bool? IncludeExpired { get; set; }
}
