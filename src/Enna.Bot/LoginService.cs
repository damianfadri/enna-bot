using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Enna.Bot
{
    public class LoginService : IHostedService
    {
        private readonly DiscordSocketClient _client;
        private readonly BotOptions _botOptions;

        public LoginService(
            DiscordSocketClient client, 
            IOptions<BotOptions> botOptions)
        {
            ArgumentNullException.ThrowIfNull(client);
            ArgumentNullException.ThrowIfNull(botOptions?.Value);

            _botOptions = botOptions.Value;
            _client = client;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _client.StartAsync();
            await _client.LoginAsync(TokenType.Bot, _botOptions.Token);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _client.LogoutAsync();
        }
    }
}