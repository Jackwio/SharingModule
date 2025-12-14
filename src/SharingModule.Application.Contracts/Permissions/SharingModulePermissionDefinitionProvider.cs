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
        
        var shareLinkPermission = myGroup.AddPermission(SharingModulePermissions.ShareLinks.Default, L("Permission:ShareLinks"));
        shareLinkPermission.AddChild(SharingModulePermissions.ShareLinks.Create, L("Permission:ShareLinks.Create"));
        shareLinkPermission.AddChild(SharingModulePermissions.ShareLinks.Update, L("Permission:ShareLinks.Update"));
        shareLinkPermission.AddChild(SharingModulePermissions.ShareLinks.Delete, L("Permission:ShareLinks.Delete"));
        shareLinkPermission.AddChild(SharingModulePermissions.ShareLinks.Revoke, L("Permission:ShareLinks.Revoke"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<SharingModuleResource>(name);
    }
}
