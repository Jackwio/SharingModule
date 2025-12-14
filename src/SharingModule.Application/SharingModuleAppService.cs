using System;
using System.Collections.Generic;
using System.Text;
using SharingModule.Localization;
using Volo.Abp.Application.Services;

namespace SharingModule;

/* Inherit your application services from this class.
 */
public abstract class SharingModuleAppService : ApplicationService
{
    protected SharingModuleAppService()
    {
        LocalizationResource = typeof(SharingModuleResource);
    }
}
