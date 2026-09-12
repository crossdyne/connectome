using Connectome.SocialGraph.Application.Abstractions.Repositories;
using Connectome.SocialGraph.Domain.Models;
using Connectome.SocialGraph.Domain.ValueObjects.Common;
using Connectome.SocialGraph.Domain.ValueObjects.Person;
using Shared.Abstractions.Messaging;
using Shared.IntegrationEvents;

namespace Connectome.SocialGraph.Application.Feature.Persons.Events
{
    public class CreatePersonIntegrationEventHandler(
        IPersonRepository personRepository) : IIntegrationEventHandler<UserCreatedIntegrationEvent>
    {
        public async Task HandleAsync(UserCreatedIntegrationEvent @event, CancellationToken cancellationToken)
        {
            var person = Person.Create(
                UserId.Create(@event.UserId), 
                UserName.Create(@event.UserName));

            await personRepository.CreateAsync(person, cancellationToken);
        }
    }
}