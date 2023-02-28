using Enna.Bot.SeedWork;

namespace Enna.Streamers.Domain
{
    public interface IFeedRepository : IRepository<Feed>
    {
        Task<IEnumerable<Feed>> FindByStreamerId(Guid streamerId);
    }
}
