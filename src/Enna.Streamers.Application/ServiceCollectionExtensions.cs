using Enna.Streamers.Application.Contracts;
using Enna.Streamers.Application.Workers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Enna.Streamers.Application
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddStreamerApplicationServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services
                .AddOptions<WorkerOptions>()
                .Bind(configuration.GetSection(nameof(WorkerOptions)));

            services
                .AddTransient<IWorker, FindLiveStreamersWorker>();
                // .AddHostedService<WorkerService>();

            return services;
        }
    }
}
