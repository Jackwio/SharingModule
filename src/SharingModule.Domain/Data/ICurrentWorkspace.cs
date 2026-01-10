using System;

namespace SharingModule.Data;

/// <summary>
/// Service to get the current workspace ID
/// </summary>
public interface ICurrentWorkspace
{
    /// <summary>
    /// Gets the current workspace ID
    /// </summary>
    Guid? Id { get; }
    
    /// <summary>
    /// Changes the current workspace ID
    /// </summary>
    IDisposable Change(Guid? workspaceId);
}
