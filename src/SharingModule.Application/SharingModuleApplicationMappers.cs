using Riok.Mapperly.Abstractions;
using SharingModule.Models;
using SharingModule.ShareLinks;
using Volo.Abp.Mapperly;

namespace SharingModule;

[Mapper]
public partial class SharingModuleApplicationMappers
{
    /* You can configure your Mapperly mapping configuration here.
     * Alternatively, you can split your mapping configurations
     * into multiple mapper classes for a better organization. */
     
    public partial ShareLinkDto Map(ShareLink source);
    public partial ShareLinkWithDetailsDto MapWithDetails(ShareLink source);
    public partial ShareLinkAccessLogDto Map(ShareLinkAccessLog source);
}
