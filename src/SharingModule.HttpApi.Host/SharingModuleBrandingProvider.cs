using Microsoft.Extensions.Localization;
using SharingModule.Localization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace SharingModule;

[Dependency(ReplaceServices = true)]
public class SharingModuleBrandingProvider : DefaultBrandingProvider
{
    private IStringLocalizer<SharingModuleResource> _localizer;

    public SharingModuleBrandingProvider(IStringLocalizer<SharingModuleResource> localizer)
    {
        _localizer = localizer;
    }

    public override string AppName => _localizer["AppName"];
}
