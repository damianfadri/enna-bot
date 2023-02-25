using Enna.Streamers.Domain.SeedWork;

namespace Enna.Streamers.Domain.Discord
{
    public class TextChannelFeed : Entity
    {
        public ulong Guild { get; init; }
        public ulong Channel { get; init; }
        public string? Template { get; }

        public TextChannelFeed(Guid id, ulong guild, ulong channel) : base(id)
        {
            Guild = guild;
            Channel = channel;
        }
    }
}
