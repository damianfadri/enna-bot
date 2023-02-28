using Microsoft.Extensions.DependencyInjection;

namespace Enna.Bot.Interactions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInteractions(
            this IServiceCollection services)
        {
            return services;
        }
    }
}
