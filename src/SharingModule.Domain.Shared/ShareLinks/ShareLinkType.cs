namespace SharingModule.ShareLinks;

/// <summary>
/// Defines the type of share link
/// </summary>
public enum ShareLinkType
{
    /// <summary>
    /// Private - resource not shared
    /// </summary>
    Private = 0,
    
    /// <summary>
    /// Single use share link
    /// </summary>
    SingleUse = 1,
    
    /// <summary>
    /// Multiple use share link
    /// </summary>
    MultipleUse = 2
}
