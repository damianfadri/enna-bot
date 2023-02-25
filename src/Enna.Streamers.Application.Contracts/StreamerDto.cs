namespace Enna.Streamers.Application.Contracts
{
    public record StreamerDto(
        Guid Id,
        string Name,
        List<string> ChannelLinks);
}
