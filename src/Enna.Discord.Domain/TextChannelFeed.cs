using Enna.Core.Domain;
using Enna.Streamers.Domain;

namespace Enna.Discord.Domain
{
    public class TextChannelFeed : TenantEntity
    {
        public ulong Guild { get; init; }
        public ulong Channel { get; init; }

        #region Navigation Properties
#pragma warning disable
        public Feed Feed { get; set; }
#pragma warning enable
        #endregion

        public TextChannelFeed(Guid id, ulong guild, ulong channel) : base(id)
        {
            Guild = guild;
            Channel = channel;
        }
    }
}
