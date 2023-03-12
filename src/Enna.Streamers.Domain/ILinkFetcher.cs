namespace Enna.Streamers.Domain
{
    public interface ILinkFetcher
    {
        bool CanFetch(string channelLink);
        Task<string?> Fetch(string channelLink);
    }
}