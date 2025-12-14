using Volo.Abp.Settings;

namespace SharingModule.Settings;

public class SharingModuleSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(SharingModuleSettings.MySetting1));
    }
}
