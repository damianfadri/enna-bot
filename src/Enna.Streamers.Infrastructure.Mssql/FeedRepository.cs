using Enna.Streamers.Domain;
using Microsoft.EntityFrameworkCore;

namespace Enna.Streamers.Infrastructure.Mssql
{
    public class FeedRepository : IFeedRepository
    {
        private readonly StreamerContext _context;

        public FeedRepository(StreamerContext context)
        {
            _context = context;
        }

        public async Task Add(Feed entity)
        {
            await _context.Feeds.AddAsync(entity);
        }

        public async Task<IEnumerable<Feed>> FindAll()
        {
            return await _context.Feeds.ToListAsync();
        }

        public async Task<Feed?> FindById(Guid id)
        {
            return await _context.Feeds
                .FirstOrDefaultAsync(feed => feed.Id == id);
        }

        public async Task Remove(Feed entity)
        {
            _context.Feeds.Remove(entity);
            await Task.CompletedTask;
        }
    }
}
