﻿using Enna.Core.Domain;

namespace Enna.Discord.Domain
{
    public class TextChannelFeed : TenantEntity
    {
        public ulong Guild { get; init; }
        public ulong Channel { get; init; }
        public TextChannelFeed(Guid id, ulong guild, ulong channel) : base(id)
        {
            Guild = guild;
            Channel = channel;
        }
    }
}
