using System;
using SharingModule.Data;

namespace SharingModule;

/// <summary>
/// Test implementation of ICurrentWorkspace that returns a fixed workspace ID
/// </summary>
public class TestCurrentWorkspace : ICurrentWorkspace
{
    private Guid? _workspaceId = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6");

    public Guid? Id => _workspaceId;

    public IDisposable Change(Guid? workspaceId)
    {
        var previousId = _workspaceId;
        _workspaceId = workspaceId;
        
        return new DisposeAction(() =>
        {
            _workspaceId = previousId;
        });
    }
    
    private class DisposeAction : IDisposable
    {
        private readonly Action _action;

        public DisposeAction(Action action)
        {
            _action = action;
        }

        public void Dispose()
        {
            _action();
        }
    }
}
