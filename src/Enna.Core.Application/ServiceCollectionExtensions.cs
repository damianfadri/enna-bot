using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Enna.Core.Application
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCoreApplicationServices(
            this IServiceCollection services)
        {
            return services;
        }
    }
}
