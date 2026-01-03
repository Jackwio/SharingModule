# Workspace 自動過濾功能

## 概述

本專案實現了類似 ABP 的 `IMultiTenant` 的 `IMultiWorkspace` 介面，用於為每個 Entity 自動添加 Workspace 範圍的過濾功能。

## 核心組件

### 1. IMultiWorkspace 介面

位置：`src/SharingModule.Domain/Models/IMultiWorkspace.cs`

```csharp
public interface IMultiWorkspace
{
    Guid WorkspaceId { get; }
}
```

### 2. ICurrentWorkspace 服務

位置：`src/SharingModule.Domain/Data/ICurrentWorkspace.cs`

用於獲取和設置當前的 Workspace ID。

```csharp
public interface ICurrentWorkspace
{
    Guid? Id { get; }
    IDisposable Change(Guid? workspaceId);
}
```

### 3. 實體實現

所有需要 Workspace 過濾的實體都實現了 `IMultiWorkspace` 介面：

- `ShareLink`
- `ShareLinkAccessLog`

## 使用方式

### 1. 設置當前 Workspace

```csharp
public class MyAppService : ApplicationService
{
    private readonly ICurrentWorkspace _currentWorkspace;
    
    public MyAppService(ICurrentWorkspace currentWorkspace)
    {
        _currentWorkspace = currentWorkspace;
    }
    
    public async Task DoSomethingAsync(Guid workspaceId)
    {
        // 設置當前 Workspace
        using (_currentWorkspace.Change(workspaceId))
        {
            // 在這個範圍內，所有查詢都會自動過濾到指定的 Workspace
            // 所有新增的實體都會自動設置 WorkspaceId
        }
    }
}
```

### 2. 自動查詢過濾

當設置了 `ICurrentWorkspace.Id` 後，所有實現了 `IMultiWorkspace` 的實體查詢都會自動過濾：

```csharp
using (_currentWorkspace.Change(workspaceId))
{
    // 只會返回 WorkspaceId = workspaceId 的 ShareLink
    var shareLinks = await _shareLinkRepository.GetListAsync();
}
```

### 3. 創建實體

在創建新實體時，需要手動設置 WorkspaceId（通常從 `ICurrentWorkspace` 獲取）：

```csharp
var shareLink = await _shareLinkManager.CreateAsync(
    resourceType: ResourceType.Document,
    resourceId: "doc-123"
);
// ShareLinkManager 內部會自動從 ICurrentWorkspace 獲取當前 WorkspaceId
```

## 資料庫遷移

已創建 migration `AddWorkspaceIdToEntities`，添加了：

1. `WorkspaceId` 欄位到 `AppShareLinks` 和 `AppShareLinkAccessLogs` 表
2. 對應的索引以提升查詢性能

執行遷移：

```bash
dotnet ef database update
```

## 注意事項

1. **必須設置 WorkspaceId**: 創建實體前必須設置當前 Workspace，否則會拋出異常
2. **查詢過濾**: 查詢過濾是全局的，會自動應用到所有實現 `IMultiWorkspace` 的實體
3. **性能考慮**: WorkspaceId 已添加索引，查詢性能不會受影響
4. **避免跨 Workspace 操作**: 如需跨 Workspace 操作，可以使用 `Change(null)` 暫時禁用過濾

## 範例

```csharp
public class ShareLinkAppService : ApplicationService
{
    private readonly IShareLinkRepository _shareLinkRepository;
    private readonly ShareLinkManager _shareLinkManager;
    private readonly ICurrentWorkspace _currentWorkspace;
    
    public ShareLinkAppService(
        IShareLinkRepository shareLinkRepository,
        ShareLinkManager shareLinkManager,
        ICurrentWorkspace currentWorkspace)
    {
        _shareLinkRepository = shareLinkRepository;
        _shareLinkManager = shareLinkManager;
        _currentWorkspace = currentWorkspace;
    }
    
    public async Task<ShareLinkDto> CreateAsync(CreateShareLinkDto input, Guid workspaceId)
    {
        // 設置當前 Workspace
        using (_currentWorkspace.Change(workspaceId))
        {
            // 創建的 ShareLink 會自動關聯到 workspaceId
            var shareLink = await _shareLinkManager.CreateAsync(
                input.ResourceType,
                input.ResourceId,
                input.LinkType
            );
            
            return ObjectMapper.Map<ShareLink, ShareLinkDto>(shareLink);
        }
    }
    
    public async Task<List<ShareLinkDto>> GetListAsync(Guid workspaceId)
    {
        using (_currentWorkspace.Change(workspaceId))
        {
            // 只會返回該 Workspace 的 ShareLink
            var shareLinks = await _shareLinkRepository.GetListAsync();
            return ObjectMapper.Map<List<ShareLink>, List<ShareLinkDto>>(shareLinks);
        }
    }
}
```
