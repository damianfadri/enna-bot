using Enna.Streamers.Domain;
using Microsoft.EntityFrameworkCore;

namespace Enna.Streamers.Infrastructure.Mssql
{
    public class ChannelRepository : IChannelRepository
    {
        private readonly StreamerContext _context;

        public ChannelRepository(StreamerContext context)
        {
            _context = context;
        }

        public async Task Add(Channel entity)
        {
            await _context.Channels.AddAsync(entity);
        }

        public async Task<IEnumerable<Channel>> FindAll()
        {
            return await _context.Channels.ToListAsync();
        }

        public async Task<Channel?> FindById(Guid id)
        {
            return await _context.Channels
                .FirstOrDefaultAsync(channel => channel.Id == id);
        }

        public async Task Remove(Channel entity)
        {
            _context.Channels.Remove(entity);
            await Task.CompletedTask;
        }
    }
}
