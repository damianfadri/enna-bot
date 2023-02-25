﻿using Enna.Streamers.Domain.Discord;
using Microsoft.EntityFrameworkCore;

namespace Enna.Streamers.Infrastructure.Mssql
{
    public class TextChannelFeedRepository : ITextChannelFeedRepository
    {
        private readonly StreamerContext _context;

        public TextChannelFeedRepository(StreamerContext context)
        {
            _context = context;
        }

        public async Task Add(TextChannelFeed entity)
        {
            await _context.TextChannelFeeds.AddAsync(entity);
        }

        public async Task<IEnumerable<TextChannelFeed>> FindAll()
        {
            return await _context.TextChannelFeeds.ToListAsync();
        }

        public async Task<TextChannelFeed?> FindById(Guid id)
        {
            return await _context.TextChannelFeeds
                .FirstOrDefaultAsync(feed => feed.Id == id);
        }
    }
}
