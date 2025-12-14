namespace SharingModule.Permissions;

public static class SharingModulePermissions
{
    public const string GroupName = "SharingModule";

    //Add your own permission names. Example:
    //public const string MyPermission1 = GroupName + ".MyPermission1";
    
    public static class ShareLinks
    {
        public const string Default = GroupName + ".ShareLinks";
        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
        public const string Revoke = Default + ".Revoke";
    }
}
