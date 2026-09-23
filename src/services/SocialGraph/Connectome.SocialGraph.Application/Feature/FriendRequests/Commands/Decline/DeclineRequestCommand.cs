using Crossdyne.Toolkit.Results;
using MediatR;
using Unit = Crossdyne.Toolkit.Primitives.Unit;

namespace Connectome.SocialGraph.Application.Feature.FriendRequests.Commands.Decline
{
    public sealed record DeclineRequestCommand(Guid RequesterUserId, Guid RecipientUserId) : IRequest<Result<Unit>>;
}