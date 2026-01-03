using System;
using System.Threading;
using Volo.Abp.DependencyInjection;

namespace SharingModule.Data;

public class CurrentWorkspace : ICurrentWorkspace, ITransientDependency
{
    private static readonly AsyncLocal<Guid?> _currentWorkspaceId = new AsyncLocal<Guid?>();

    public Guid? Id => _currentWorkspaceId.Value;

    public IDisposable Change(Guid? workspaceId)
    {
        var previousValue = _currentWorkspaceId.Value;
        _currentWorkspaceId.Value = workspaceId;
        return new DisposeAction(() => _currentWorkspaceId.Value = previousValue);
    }
}

internal class DisposeAction : IDisposable
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
