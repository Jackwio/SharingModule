using SharingModule.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace SharingModule.Permissions;

public class SharingModulePermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(SharingModulePermissions.GroupName);
        //Define your own permissions here. Example:
        //myGroup.AddPermission(SharingModulePermissions.MyPermission1, L("Permission:MyPermission1"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<SharingModuleResource>(name);
    }
}
