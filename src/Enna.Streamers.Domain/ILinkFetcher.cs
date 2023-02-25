namespace Enna.Streamers.Domain
{
    public interface ILinkFetcher
    {
        bool CanFetch(Channel channel);
        Task<string?> Fetch(Channel channel);
    }
}