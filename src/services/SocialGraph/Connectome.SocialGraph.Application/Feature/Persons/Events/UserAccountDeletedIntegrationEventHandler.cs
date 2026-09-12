using Connectome.SocialGraph.Application.Abstractions.Repositories;
using Connectome.SocialGraph.Domain.ValueObjects.Common;
using Shared.Abstractions.Messaging;
using Shared.IntegrationEvents;

namespace Connectome.SocialGraph.Application.Feature.Persons.Events
{
    public sealed class UserAccountDeletedIntegrationEventHandler(
        IPersonRepository personRepository) : IIntegrationEventHandler<UserAccountDeletedIntegrationEvent>
    {
        public async Task HandleAsync(UserAccountDeletedIntegrationEvent @event, CancellationToken cancellationToken)
            => await personRepository.RemoveAsync(UserId.Create(@event.UserId));
    }
}