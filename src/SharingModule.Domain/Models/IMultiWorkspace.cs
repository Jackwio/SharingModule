using System;

namespace SharingModule.Models;

/// <summary>
/// Interface for entities that belong to a specific workspace
/// </summary>
public interface IMultiWorkspace
{
    /// <summary>
    /// The workspace ID that this entity belongs to
    /// </summary>
    Guid WorkspaceId { get; }
}
