using Enna.Streamers.Domain;

namespace Enna.Bot
{
    public interface ILinkFetcher
    {
        bool CanFetch(string channelLink);
        Task<string?> Fetch(string channelLink);
    }
}