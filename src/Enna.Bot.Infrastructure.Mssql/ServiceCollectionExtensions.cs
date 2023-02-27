using Enna.Bot.SeedWork;
using Enna.Discord.Domain;
using Enna.Streamers.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Enna.Bot.Infrastructure.Mssql
{
    public static class ServiceCollectionExtensions
    {
        private const string DEFAULT_CONNECTION_STRING = "EnnaDatabase";

        public static IServiceCollection AddDatabaseServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services
                .AddSingleton<IStreamerRepository, StreamerRepository>()
                .AddSingleton<IFeedRepository, FeedRepository>()
                .AddSingleton<IChannelRepository, ChannelRepository>()
                .AddSingleton<ITextChannelFeedRepository, TextChannelFeedRepository>()
                .AddDbContext<StreamerContext>(options =>
                {
                    options.UseSqlServer(
                        configuration
                            .GetConnectionString(DEFAULT_CONNECTION_STRING),
                        options => options.UseQuerySplittingBehavior(
                            QuerySplittingBehavior.SplitQuery));
                })
                .AddSingleton<IDomainEventDispatcher, DomainEventDispatcher>()
                .AddSingleton<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
