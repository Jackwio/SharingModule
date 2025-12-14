using SharingModule.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace SharingModule.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class SharingModuleController : AbpControllerBase
{
    protected SharingModuleController()
    {
        LocalizationResource = typeof(SharingModuleResource);
    }
}
