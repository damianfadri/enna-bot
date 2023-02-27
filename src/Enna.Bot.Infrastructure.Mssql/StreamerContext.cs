using Enna.Discord.Domain;
using Enna.Streamers.Domain;
using Microsoft.EntityFrameworkCore;

namespace Enna.Bot.Infrastructure.Mssql
{
    public class StreamerContext : DbContext
    {
        public DbSet<Streamer> Streamers { get; set; }
        public DbSet<Channel> Channels { get; set; }
        public DbSet<Feed> Feeds { get; set; }
        public DbSet<TextChannelFeed> TextChannelFeeds { get; set; }

        public StreamerContext(
            DbContextOptions<StreamerContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
