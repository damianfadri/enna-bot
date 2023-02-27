using Enna.Bot.SeedWork;

namespace Enna.Discord.Domain
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
