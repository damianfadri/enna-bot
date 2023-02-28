using Discord.WebSocket;
using Enna.Bot;
using Enna.Bot.Infrastructure;
using Enna.Bot.Infrastructure.Mssql;
using Enna.Bot.Interactions;
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
                    .AddHostedService<LoginService>()
                    .AddHostedService<LogService>()
                    .AddHostedService<CommandService>()
                    .AddSingleton<DiscordSocketClient>();

                services
                    .AddMediatR(config =>
                        config.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));

                services
                    .AddInteractions()
                    .AddTextChannelFeedServices()
                    .AddStreamerInfrastructureServices()
                    .AddStreamerApplicationServices(configuration)
                    .AddDatabaseServices(configuration);
            })
            .RunConsoleAsync();
    }
}