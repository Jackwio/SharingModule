using AutoMapper;
using SharingModule.Models;
using SharingModule.ShareLinks;

namespace SharingModule
{
    public class SharingModuleApplicationAutoMapperProfile : Profile
    {
        public SharingModuleApplicationAutoMapperProfile()
        {
            // Mapping for DTOs used by Application services
            CreateMap<ShareLink, ShareLinkWithDetailsDto>();
            CreateMap<ShareLink, ShareLinkDto>();
            CreateMap<ShareLinkAccessLog, ShareLinkAccessLogDto>();
        }
    }
}