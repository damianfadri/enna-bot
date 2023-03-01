using Enna.Bot.SeedWork;
using Enna.Discord.Domain;
using Enna.Streamers.Domain;
using Microsoft.EntityFrameworkCore;

namespace Enna.Bot.Infrastructure.Mssql
{
    public class StreamerContext : DbContext
    {
        public DbSet<Tenant<ulong>> Tenants { get; set; }
        public DbSet<Streamer> Streamers { get; set; }
        public DbSet<Channel> Channels { get; set; }
        public DbSet<Feed> Feeds { get; set; }
        public DbSet<TextChannelFeed> TextChannelFeeds { get; set; }

        private readonly ITenantProvider _tenantProvider;

        public StreamerContext(
            DbContextOptions<StreamerContext> options,
            ITenantProvider tenantProvider)
            : base(options)
        {
            ArgumentNullException.ThrowIfNull(tenantProvider);

            _tenantProvider = tenantProvider;
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Streamer>()
                .HasQueryFilter(
                    e => e.TenantId == _tenantProvider.TenantId)
                .Property(e => e.TenantId);

            modelBuilder
                .Entity<Channel>()
                .HasQueryFilter(
                    e => e.TenantId == _tenantProvider.TenantId)
                .Property(e => e.TenantId);

            modelBuilder
                .Entity<Feed>()
                .HasQueryFilter(
                    e => e.TenantId == _tenantProvider.TenantId)
                .Property(e => e.TenantId);

            modelBuilder
                .Entity<TextChannelFeed>()
                .HasQueryFilter(
                    e => e.TenantId == _tenantProvider.TenantId)
                .Property(e => e.TenantId);
        }
    }
}
