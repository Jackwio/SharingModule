using Microsoft.EntityFrameworkCore;
using SharingModule.Models;
using SharingModule.ShareLinks;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace SharingModule.EntityFrameworkCore;

public static class SharingModuleDbContextModelCreatingExtensions
{
    public static void ConfigureSharingModule(this ModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

        // Configure ShareLink aggregate root
        builder.Entity<ShareLink>(b =>
        {
            b.ToTable(SharingModuleConsts.DbTablePrefix + "ShareLinks", SharingModuleConsts.DbSchema);
            b.ConfigureByConvention();
            
            // Properties
            b.Property(x => x.WorkspaceId)
                .IsRequired();
            
            b.Property(x => x.Token)
                .IsRequired()
                .HasMaxLength(ShareLinkConsts.MaxTokenLength);
            
            b.Property(x => x.LinkType)
                .IsRequired();
            
            b.Property(x => x.IsReadOnly)
                .IsRequired();
            
            b.Property(x => x.AllowComments)
                .IsRequired();
            
            b.Property(x => x.AllowAnonymous)
                .IsRequired();
            
            b.Property(x => x.ExpiresAt);
            
            b.Property(x => x.IsRevoked)
                .IsRequired();
            
            b.Property(x => x.RevokedAt);
            
            // Indexes
            b.HasIndex(x => x.WorkspaceId);
            
            b.HasIndex(x => x.Token)
                .IsUnique();
            
            b.HasIndex(x => x.IsRevoked);
            
            // Relationships
            b.HasMany(x => x.AccessLogs)
                .WithOne()
                .HasForeignKey(x => x.ShareLinkId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        });
        
        // Configure ShareLinkAccessLog entity
        builder.Entity<ShareLinkAccessLog>(b =>
        {
            b.ToTable(SharingModuleConsts.DbTablePrefix + "ShareLinkAccessLogs", SharingModuleConsts.DbSchema);
            b.ConfigureByConvention();
            
            // Properties
            b.Property(x => x.WorkspaceId)
                .IsRequired();
                
            b.Property(x => x.ShareLinkId)
                .IsRequired();
            
            b.Property(x => x.AccessedAt)
                .IsRequired();
            
            b.Property(x => x.AccessedBy)
                .IsRequired()
                .HasMaxLength(ShareLinkConsts.MaxAccessedByLength);
            
            b.Property(x => x.IsAnonymous)
                .IsRequired();
            
            b.Property(x => x.IpAddress)
                .HasMaxLength(ShareLinkConsts.MaxIpAddressLength);
            
            b.Property(x => x.UserAgent)
                .HasMaxLength(ShareLinkConsts.MaxUserAgentLength);
            
            // Indexes
            b.HasIndex(x => x.WorkspaceId);
            
            b.HasIndex(x => x.ShareLinkId);
            
            b.HasIndex(x => x.AccessedAt);
        });
    }
}
