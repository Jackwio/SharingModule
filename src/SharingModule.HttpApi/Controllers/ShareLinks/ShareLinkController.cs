using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SharingModule.ShareLinks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace SharingModule.Controllers.ShareLinks;

[RemoteService(Name = "SharingModule")]
[Route("api/share-links")]
public class ShareLinkController : SharingModuleController, IShareLinkAppService
{
    private readonly IShareLinkAppService _shareLinkAppService;
    
    public ShareLinkController(IShareLinkAppService shareLinkAppService)
    {
        _shareLinkAppService = shareLinkAppService;
    }
    
    [HttpGet("{id}")]
    public virtual Task<ShareLinkWithDetailsDto> GetAsync(Guid id)
    {
        return _shareLinkAppService.GetAsync(id);
    }
    
    [HttpGet]
    public virtual Task<PagedResultDto<ShareLinkDto>> GetListAsync(GetShareLinksInput input)
    {
        return _shareLinkAppService.GetListAsync(input);
    }
    
    [HttpPost]
    public virtual Task<ShareLinkWithDetailsDto> CreateAsync(CreateShareLinkDto input)
    {
        return _shareLinkAppService.CreateAsync(input);
    }
    
    [HttpPut("{id}")]
    public virtual Task<ShareLinkWithDetailsDto> UpdateAsync(Guid id, UpdateShareLinkDto input)
    {
        return _shareLinkAppService.UpdateAsync(id, input);
    }
    
    [HttpDelete("{id}")]
    public virtual Task DeleteAsync(Guid id)
    {
        return _shareLinkAppService.DeleteAsync(id);
    }
    
    [HttpPost("{id}/revoke")]
    public virtual Task<ShareLinkWithDetailsDto> RevokeAsync(Guid id)
    {
        return _shareLinkAppService.RevokeAsync(id);
    }
    
    [HttpPost("validate")]
    public virtual Task<ShareLinkWithDetailsDto> ValidateAndRecordAccessAsync(ValidateShareLinkDto input)
    {
        return _shareLinkAppService.ValidateAndRecordAccessAsync(input);
    }
    
    [HttpGet("by-resource")]
    public virtual Task<ListResultDto<ShareLinkDto>> GetByResourceAsync([FromQuery] ResourceType resourceType, [FromQuery] string resourceId)
    {
        return _shareLinkAppService.GetByResourceAsync(resourceType, resourceId);
    }
}
