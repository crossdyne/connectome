using Shared.Abstractions.Messaging;

namespace Shared.IntegrationEvents
{
    public sealed record UserAccountDeletedIntegrationEvent(
        Guid IdEvent, 
        DateTime OccurredOnUtc, 
        Guid UserId) : IIntegrationEvent;
}