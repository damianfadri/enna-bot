using Enna.Streamers.Domain;
using System.Text.RegularExpressions;

namespace Enna.Bot.Infrastructure
{
    public class YoutubeLivestreamFetcher : ILinkFetcher
    {
        private const string YT_CHANNEL_LINK_REGEX
            = @"^(?:https?:\/\/)?(?:www\.)?youtube.com(?:\/(?:user|c(?:hannel)?))?\/(@?[a-zA-Z0-9_-]+)\/?.*$";
        private const string YT_CANONICAL_LINK_REGEX
            = @"<link rel=""canonical"" href=""(.*?)"">";
        private const string YT_WATCH_LINK_REGEX
            = @"https://www\.youtube\.com/watch\?v=.{11}";
        private const string YT_WATCHING_NOW_REGEX
            = @"(""isLiveNow"":true)|(""isLive"":true)";

        private readonly HttpClient _httpClient;

        public YoutubeLivestreamFetcher(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public bool CanFetch(Channel channel)
        {
            if (string.IsNullOrEmpty(channel.Link))
            {
                return false;
            }

            var matchResult = Regex.Match(channel.Link, YT_CHANNEL_LINK_REGEX);
            if (!matchResult.Success)
            {
                return false;
            }

            return true;
        }

        public async Task<string?> Fetch(Channel channel)
        {
            var streamLink = $"{channel.Link}/live";

            var httpResponse = await _httpClient.GetAsync(streamLink);
            if (!httpResponse.IsSuccessStatusCode)
            {
                // Stream link is inaccessible. Probably privated or removed.
                // In any case, streamer is not live.
                return null;
            }

            var httpResponseBody
                = await httpResponse.Content.ReadAsStringAsync();

            var matchResult = Regex.Match(
                httpResponseBody, YT_CANONICAL_LINK_REGEX);

            if (!matchResult.Success)
            {
                // Canonical link cannot be found from the response body.
                // Either the page being visited does not exist or they changed things up.
                // Streamer is not live.
                return null;
            }

            var canonicalLink = matchResult.Groups[1].Value;
            if (!Regex.IsMatch(canonicalLink, YT_WATCH_LINK_REGEX))
            {
                // Canonical link is not a YouTube video link.
                // Therefore, streamer is not live.
                return null;
            }

            if (!Regex.IsMatch(httpResponseBody, YT_WATCHING_NOW_REGEX))
            {
                // No "watching now" text in the video page.
                // Maybe the streamer just scheduled a live stream.
                // Streamer is not live yet.
                return null;
            }

            return canonicalLink;
        }
    }
}
