using Enna.Streamers.Domain;
using System.Text.RegularExpressions;

namespace Enna.Bot.Infrastructure
{
    public class TwitchLivestreamFetcher : ILinkFetcher
    {
        private const string TWITCH_LIVE_STATUS_REGEX 
            = @"""isLiveBroadcast"":true";

        public const string TWITCH_CHANNEL_LINK_REGEX 
            = @"^(?:https?:\/\/)?(?:www.)?twitch.tv\/([a-zA-Z0-9_-]+)\/?.*$";

        private readonly HttpClient _httpClient;

        public TwitchLivestreamFetcher(HttpClient httpClient)
        {
            ArgumentNullException.ThrowIfNull(httpClient);

            _httpClient = httpClient;
        }

        public bool CanFetch(string channelLink)
        {
            if (string.IsNullOrEmpty(channelLink))
            {
                return false;
            }

            var matchResult = Regex.Match(channelLink, TWITCH_CHANNEL_LINK_REGEX);
            if (!matchResult.Success)
            {
                return false;
            }

            return true;
        }

        public async Task<string?> Fetch(string channelLink)
        {
            var streamLink = channelLink;

            var httpResponse = await _httpClient.GetAsync(streamLink);
            if (!httpResponse.IsSuccessStatusCode)
            {
                // Stream link is inaccessible. Maybe it got deleted or Twitch is acting up.
                // In any case, streamer is not live.
                return null;
            }

            var httpResponseBody = await httpResponse.Content.ReadAsStringAsync();
            if (!Regex.IsMatch(httpResponseBody, TWITCH_LIVE_STATUS_REGEX))
            {
                // isLiveBroadcast indicator is not in the response body.
                // Streamer is not live.
                return null;
            }

            return streamLink;
        }
    }
}
