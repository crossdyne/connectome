using Connectome.UserManagement.Service.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Connectome.SocialGraph.Infrastructure.Extensions
{
    public static class HttpServiceCollectionExtensions
    {
        public static IServiceCollection UseHttpService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient<IUserManagementService, UserManagementService>(client => client.BaseAddress = new Uri(configuration["Urls:UserManagement"]!));

            return services;
        }
    }
}