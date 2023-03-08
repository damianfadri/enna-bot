using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Enna.Bot.HostedServices
{
    public class WorkerService : IHostedService
    {
        private readonly DiscordSocketClient _client;
        private readonly WorkerOptions _options;
        private readonly IServiceScopeFactory _scopeFactory;

        public WorkerService(
            DiscordSocketClient client,
            IOptions<WorkerOptions> options,
            IServiceScopeFactory scopeFactory)
        {
            ArgumentNullException.ThrowIfNull(client);
            ArgumentNullException.ThrowIfNull(options?.Value);
            ArgumentNullException.ThrowIfNull(scopeFactory);

            _client = client;
            _options = options.Value;
            _scopeFactory = scopeFactory;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var timer = new PeriodicTimer(
                TimeSpan.FromMilliseconds(_options.PollingMs));

            while (await timer.WaitForNextTickAsync(cancellationToken))
            {
                foreach (var guild in _client.Guilds)
                {
                    await PerformWorkPerGuild(guild.Id);
                }
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
        }

        private async Task PerformWorkPerGuild(ulong guildId)
        {
            using var scope = _scopeFactory.CreateScope();

            var workers = scope.ServiceProvider.GetServices<IWorker>();
            foreach (var worker in workers)
            {
                await worker.DoWork(guildId);
            }
        }
    }
}
