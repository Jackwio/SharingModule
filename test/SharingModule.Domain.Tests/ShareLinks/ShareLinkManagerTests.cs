using System;
using System.Threading.Tasks;
using SharingModule.Managers;
using SharingModule.Models;
using SharingModule.ShareLinks;
using Shouldly;
using Volo.Abp;
using Volo.Abp.Modularity;
using Xunit;

namespace SharingModule.ShareLinks;

public abstract class ShareLinkManagerTests<TStartupModule> : SharingModuleDomainTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    private readonly ShareLinkManager _shareLinkManager;
    private readonly IShareLinkRepository _shareLinkRepository;

    protected ShareLinkManagerTests()
    {
        _shareLinkManager = GetRequiredService<ShareLinkManager>();
        _shareLinkRepository = GetRequiredService<IShareLinkRepository>();
    }

    [Fact]
    public async Task Should_Create_ShareLink_With_Valid_Token()
    {
        // Act
        var shareLink = await _shareLinkManager.CreateAsync(
            ResourceType.Page,
            "test-page-id",
            ShareLinkType.MultipleUse,
            isReadOnly: true,
            allowComments: false,
            allowAnonymous: true
        );

        // Assert
        shareLink.ShouldNotBeNull();
        shareLink.Token.ShouldNotBeNullOrEmpty();
        shareLink.ResourceType.ShouldBe(ResourceType.Page);
        shareLink.ResourceId.ShouldBe("test-page-id");
        shareLink.IsReadOnly.ShouldBeTrue();
        shareLink.AllowComments.ShouldBeFalse();
        shareLink.AllowAnonymous.ShouldBeTrue();
        shareLink.IsRevoked.ShouldBeFalse();
    }

    [Fact]
    public async Task Should_Validate_Valid_ShareLink()
    {
        // Arrange
        var shareLink = await _shareLinkManager.CreateAsync(
            ResourceType.Page,
            "test-page-id"
        );

        // Act
        var validatedLink = await _shareLinkManager.ValidateAndGetAsync(shareLink.Token);

        // Assert
        validatedLink.ShouldNotBeNull();
        validatedLink.Id.ShouldBe(shareLink.Id);
    }

    [Fact]
    public async Task Should_Throw_Exception_For_Invalid_Token()
    {
        // Act & Assert
        await Should.ThrowAsync<BusinessException>(async () =>
        {
            await _shareLinkManager.ValidateAndGetAsync("invalid-token");
        });
    }

    [Fact]
    public async Task Should_Throw_Exception_For_Revoked_ShareLink()
    {
        // Arrange
        var shareLink = await _shareLinkManager.CreateAsync(
            ResourceType.Page,
            "test-page-id"
        );
        
        shareLink.Revoke();
        await _shareLinkRepository.UpdateAsync(shareLink);

        // Act & Assert
        await Should.ThrowAsync<BusinessException>(async () =>
        {
            await _shareLinkManager.ValidateAndGetAsync(shareLink.Token);
        });
    }

    [Fact]
    public async Task Should_Throw_Exception_For_Expired_ShareLink()
    {
        // Arrange
        var shareLink = await _shareLinkManager.CreateAsync(
            ResourceType.Page,
            "test-page-id",
            expiresAt: DateTime.UtcNow.AddMinutes(-10) // Already expired
        );

        // Act & Assert
        await Should.ThrowAsync<BusinessException>(async () =>
        {
            await _shareLinkManager.ValidateAndGetAsync(shareLink.Token);
        });
    }

    [Fact]
    public async Task Should_Record_Access_To_ShareLink()
    {
        ShareLink? updatedLink = null;
        
        await WithUnitOfWorkAsync(async () =>
        {
            // Arrange
            var shareLink = await _shareLinkManager.CreateAsync(
                ResourceType.Page,
                "test-page-id"
            );

            // Act
            await _shareLinkManager.RecordAccessAsync(
                shareLink,
                "test-user",
                isAnonymous: false,
                ipAddress: "127.0.0.1",
                userAgent: "Test User Agent"
            );
        });

        await WithUnitOfWorkAsync(async () =>
        {
            // Assert
            var shareLinks = await _shareLinkRepository.GetListAsync(includeDetails: true);
            updatedLink = shareLinks[0];
        });
        
        updatedLink.ShouldNotBeNull();
        updatedLink.AccessLogs.Count.ShouldBe(1);
        updatedLink.AccessLogs.ShouldContain(log => 
            log.AccessedBy == "test-user" && 
            log.IpAddress == "127.0.0.1"
        );
    }

    [Fact]
    public async Task Should_Revoke_SingleUse_ShareLink_After_First_Access()
    {
        ShareLink? updatedLink = null;
        
        await WithUnitOfWorkAsync(async () =>
        {
            // Arrange
            var shareLink = await _shareLinkManager.CreateAsync(
                ResourceType.Page,
                "test-page-id",
                ShareLinkType.SingleUse
            );

            // Act
            await _shareLinkManager.RecordAccessAsync(
                shareLink,
                "test-user",
                isAnonymous: false
            );
        });

        await WithUnitOfWorkAsync(async () =>
        {
            // Assert
            var shareLinks = await _shareLinkRepository.GetListAsync();
            updatedLink = shareLinks[0];
        });
        
        updatedLink.ShouldNotBeNull();
        updatedLink.IsRevoked.ShouldBeTrue();
        updatedLink.RevokedAt.ShouldNotBeNull();
    }
}
