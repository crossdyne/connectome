using Shared.Abstractions.Messaging;

namespace Shared.IntegrationEvents
{
    public sealed record UserCreatedIntegrationEvent(Guid IdEvent, DateTime OccurredOnUtc, Guid UserId, string UserName) : IIntegrationEvent;
}