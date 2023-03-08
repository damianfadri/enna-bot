namespace Enna.Streamers.Application.Contracts
{
    public record StreamerDto(
        Guid Id,
        string Name,
        List<ChannelDto> Channels,
        List<FeedDto> Feeds);
}
