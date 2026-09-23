using Connectome.SocialGraph.Application.Abstractions.Repositories;
using Connectome.SocialGraph.Infrastructure.Models.Settings;
using Connectome.SocialGraph.Infrastructure.Persistence.Migrations;
using Connectome.SocialGraph.Infrastructure.Persistence.Repositories.FriendRequests;
using Connectome.SocialGraph.Infrastructure.Persistence.Repositories.Friendships;
using Connectome.SocialGraph.Infrastructure.Persistence.Repositories.Persons;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Neo4j.Driver;

namespace Connectome.SocialGraph.Infrastructure.Extensions
{
    public static class DataBaseCollectionExtensions
    {
        public static IServiceCollection AddDataBase(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<Neo4jSettings>(configuration.GetSection("Neo4j"));
            services.AddSingleton<IDriver>(sp =>
            {
               var settings = sp.GetRequiredService<IOptions<Neo4jSettings>>().Value;

               return GraphDatabase.Driver(settings.Uri, AuthTokens.Basic(settings.Username, settings.Password)); 
            });

            services.AddSingleton<Neo4jMigrationService>();

            services.AddScoped<IPersonRepository, PersonRepository>();
            services.AddScoped<IFriendRequestRepository, FriendRequestRepository>();
            services.AddScoped<IFriendshipRepository, FriendshipRepository>();

            return services;
        }
    }
}