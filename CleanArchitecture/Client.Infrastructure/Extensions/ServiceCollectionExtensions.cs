using Client.Infrastructure.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Client.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddClientInfrastrctureDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ApiClientConfig>(configuration.GetSection(nameof(ApiClientConfig)));
            services.Configure<AuthConfig>(configuration.GetSection(nameof(AuthConfig)));
        }
    }
}
