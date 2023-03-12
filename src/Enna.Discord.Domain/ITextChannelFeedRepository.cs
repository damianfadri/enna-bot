using Enna.Core.Domain;

namespace Enna.Discord.Domain
{
    public interface ITextChannelFeedRepository : IRepository<TextChannelFeed>
    {
        Task<TextChannelFeed?> FindByFeedId(Guid feedId);
    }
}
