using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SharingModule.Models;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;

namespace SharingModule.BackgroundJobs;

public class ShareLinkCleanupJob : AsyncBackgroundJob<ShareLinkCleanupJobArgs>, ITransientDependency
{
    private readonly IShareLinkRepository _shareLinkRepository;

    public ShareLinkCleanupJob(IShareLinkRepository shareLinkRepository)
    {
        _shareLinkRepository = shareLinkRepository;
    }

    public override async Task ExecuteAsync(ShareLinkCleanupJobArgs args)
    {
        Logger.LogInformation("Starting ShareLink cleanup job...");
        
        var deletedCount = await _shareLinkRepository.CleanupInvalidShareLinksAsync();
        
        Logger.LogInformation("ShareLink cleanup job completed. Deleted {Count} share links.", deletedCount);
    }
}

public class ShareLinkCleanupJobArgs
{
}
