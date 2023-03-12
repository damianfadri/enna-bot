using Enna.Streamers.Domain;
using Microsoft.EntityFrameworkCore;

namespace Enna.Bot.Infrastructure.Mssql
{
    public class StreamerRepository : IStreamerRepository
    {
        private readonly StreamerContext _context;

        public StreamerRepository(StreamerContext context)
        {
            _context = context;
        }

        public async Task Add(Streamer entity)
        {
            await _context.Streamers.AddAsync(entity);
        }

        public async Task<IEnumerable<Streamer>> FindAll()
        {
            return await _context.Streamers
                .Include(streamer => streamer.Channel)
                .Include(streamer => streamer.Feed)
                .ToListAsync();
        }

        public async Task<Streamer?> FindById(Guid id)
        {
            return await _context.Streamers
                .Include(streamer => streamer.Channel)
                .Include(streamer => streamer.Feed)
                .FirstOrDefaultAsync(streamer => streamer.Id == id);
        }

        public async Task Remove(Streamer entity)
        {
            _context.Streamers.Remove(entity);
            await Task.CompletedTask;
        }
    }
}
