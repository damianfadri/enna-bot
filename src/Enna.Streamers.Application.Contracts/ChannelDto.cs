namespace Enna.Streamers.Application.Contracts
{
    public record ChannelDto(
        Guid Id,
        string Link,
        string? StreamLink);
}
