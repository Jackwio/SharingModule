namespace SharingModule;

public static class SharingModuleDomainErrorCodes
{
    /* You can add your business exception error codes here, as constants */
    
    public const string ShareLinkNotFound = "SharingModule:ShareLink:001";
    public const string ShareLinkRevoked = "SharingModule:ShareLink:002";
    public const string ShareLinkExpired = "SharingModule:ShareLink:003";
    public const string ShareLinkAnonymousNotAllowed = "SharingModule:ShareLink:004";
    public const string ShareLinkRequiresAuthentication = "SharingModule:ShareLink:005";
    public const string ShareLinkAlreadyUsed = "SharingModule:ShareLink:006";
}
