namespace Enna.Streamers.Application.Contracts
{
    public record FeedDto(
        Guid Id, 
        string Type,
        string? MessageTemplate);
}
