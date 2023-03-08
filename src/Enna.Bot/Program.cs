using Discord.WebSocket;
using Enna.Bot;
using Enna.Bot.HostedServices;
using Enna.Bot.Infrastructure;
using Enna.Bot.Infrastructure.Mssql;
using Enna.Bot.Interactions;
using Enna.Bot.Workers;
using Enna.Core.Application;
using Enna.Discord.Application;
using Enna.Streamers.Application;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

public class Program
{
    private static async Task Main(string[] args)
    {
        await Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration(configuration =>
            {
                configuration
                    .AddUserSecrets(Assembly.GetExecutingAssembly())
                    .AddJsonFile("appsettings.json", false);
            })
            .ConfigureServices((ctx, services) =>
            {
                var configuration = ctx.Configuration;

                services
                    .AddOptions<BotOptions>()
                    .Bind(configuration.GetSection(nameof(BotOptions)));

                services
                    .AddOptions<WorkerOptions>()
                    .Bind(configuration.GetSection(nameof(WorkerOptions)));

                services
                    .AddHostedService<LoginService>()
                    .AddHostedService<LogService>()
                    .AddHostedService<CommandService>()
                    .AddHostedService<WorkerService>()
                    .AddSingleton<DiscordSocketClient>();

                services
                    .AddTransient<IWorker, FindLiveStreamersWorker>()
                    .AddTransient<ILinkFetcher, YoutubeLivestreamFetcher>()
                    .AddHttpClient<YoutubeLivestreamFetcher>();

                services
                    .AddMediatR(config =>
                        config.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));

                services
                    .AddInteractions()
                    .AddCoreApplicationServices()
                    .AddTextChannelFeedServices()
                    .AddStreamerInfrastructureServices()
                    .AddStreamerApplicationServices(configuration)
                    .AddDatabaseServices(configuration);
            })
            .RunConsoleAsync();
    }
}