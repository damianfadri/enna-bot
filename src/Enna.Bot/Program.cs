using Discord.WebSocket;
using Enna.Bot;
using Enna.Streamers.Application;
using Enna.Streamers.Application.Discord;
using Enna.Streamers.Infrastructure;
using Enna.Streamers.Infrastructure.Mssql;
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
                    .AddTextChannelFeedServices()
                    .AddStreamerInfrastructureServices()
                    .AddStreamerApplicationServices(configuration)
                    .AddDatabaseServices(configuration);
            })
            .RunConsoleAsync();
    }
}