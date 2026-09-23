using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Connectome.SocialGraph.Application.Extensions
{
    public static class MediatorCollectionExtensions
    {
        public static IServiceCollection UseMediator(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            return services;
        }
    }
}