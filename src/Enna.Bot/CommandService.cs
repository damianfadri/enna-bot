using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.Hosting;

namespace Enna.Bot
{
    public class CommandService : IHostedService
    {
        private readonly DiscordSocketClient _client;
        private readonly InteractionService _interaction;
        private readonly IServiceProvider _provider;

        public CommandService(
            DiscordSocketClient client,
            IServiceProvider provider)
        {
            ArgumentNullException.ThrowIfNull(client);
            ArgumentNullException.ThrowIfNull(provider);

            _client = client;
            _provider = provider;
            _interaction = new InteractionService(client.Rest);
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _client.Ready += OnReady;
            _client.InteractionCreated += OnInteractionCreated;

            await Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _client.Ready -= OnReady;
            _client.InteractionCreated -= OnInteractionCreated;

            await Task.CompletedTask;
        }

        private async Task OnReady()
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => a.FullName!.Contains("Enna")))
            {
                await _interaction.AddModulesAsync(assembly, _provider);
            }

            await _interaction.RegisterCommandsGloballyAsync(true);
        }

        private async Task OnInteractionCreated(SocketInteraction arg)
        {
            var context = new SocketInteractionContext(_client, arg);
            await _interaction.ExecuteCommandAsync(context, _provider);
        }
    }
}
