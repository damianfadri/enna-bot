using Enna.Core.Domain;
using Enna.Streamers.Domain;

namespace Enna.Discord.Domain
{
    public class TextChannelFeed : TenantEntity
    {
        public ulong Guild { get; init; }
        public ulong Channel { get; init; }
        public Feed Feed { get; set; }

        public TextChannelFeed(Guid id, ulong guild, ulong channel) : this(id, Feed.Default, guild, channel)
        {
        }

        public TextChannelFeed(Guid id, Feed feed, ulong guild, ulong channel) : base(id)
        {
            Feed = feed;
            Channel = channel;
            Guild = guild;
        }
    }
}
