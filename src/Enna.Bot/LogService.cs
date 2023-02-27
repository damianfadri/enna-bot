using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Enna.Bot
{
    public class LogService : IHostedService
    {
        private readonly DiscordSocketClient _client;
        private readonly ILogger<LogService> _logger;

        public LogService(
            DiscordSocketClient client, 
            ILogger<LogService> logger)
        {
            ArgumentNullException.ThrowIfNull(client);
            ArgumentNullException.ThrowIfNull(logger);

            _client = client;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _client.Log += OnLog;
            await Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _client.Log -= OnLog;
            await Task.CompletedTask;
        }

        private async Task OnLog(LogMessage arg)
        {
            switch (arg.Severity)
            {
                default:
                    _logger.LogInformation(arg.Message);
                    break;
            }

            await Task.CompletedTask;
        }
    }
}
