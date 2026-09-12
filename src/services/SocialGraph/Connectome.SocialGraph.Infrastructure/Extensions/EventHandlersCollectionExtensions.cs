using Confluent.Kafka;
using Connectome.SocialGraph.Application.Feature.Persons.Events;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Abstractions.Messaging;
using Shared.IntegrationEvents;
using Shared.Messaging;

namespace Connectome.SocialGraph.Infrastructure.Extensions
{
    public static class EventHandlersCollectionExtensions
    {
        public static IServiceCollection AddEventHandlers(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ConsumerConfig>(configuration.GetSection("Kafka:Consumer"));
            
            services.AddScoped<IIntegrationEventHandler<UserCreatedIntegrationEvent>, CreatePersonIntegrationEventHandler>();
            services.AddKafkaConsumer<UserCreatedIntegrationEvent>("user-management.user.account-created");

            return services;
        }
    }
}