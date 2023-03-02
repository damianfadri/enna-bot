using Enna.Core.Domain;
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
                .AddTransient<IStreamerRepository, StreamerRepository>()
                .AddTransient<IFeedRepository, FeedRepository>()
                .AddTransient<IChannelRepository, ChannelRepository>()
                .AddTransient<ITextChannelFeedRepository, TextChannelFeedRepository>()
                .AddDbContext<StreamerContext>(options =>
                {
                    options.UseSqlServer(
                        configuration
                            .GetConnectionString(DEFAULT_CONNECTION_STRING),
                        options => options.UseQuerySplittingBehavior(
                            QuerySplittingBehavior.SplitQuery));
                })
                .AddDbContext<TenantContext>(options =>
                {
                    options.UseSqlServer(
                        configuration
                            .GetConnectionString(DEFAULT_CONNECTION_STRING),
                        options => options.UseQuerySplittingBehavior(
                            QuerySplittingBehavior.SplitQuery));
                })
                .AddTransient<IDomainEventDispatcher, DomainEventDispatcher>()
                .AddTransient<IUnitOfWork, UnitOfWork>()

                .AddScoped<ITenantProvider, TenantProvider>()
                .AddScoped<IGuildTenantRepository, GuildTenantRepository>()
                .AddTransient<ITenantAssigner, TenantAssigner>();

            return services;
        }
    }
}
