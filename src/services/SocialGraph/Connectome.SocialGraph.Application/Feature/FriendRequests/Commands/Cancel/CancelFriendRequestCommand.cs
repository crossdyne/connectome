using Crossdyne.Toolkit.Results;
using MediatR;
using Unit = Crossdyne.Toolkit.Primitives.Unit;

namespace Connectome.SocialGraph.Application.Feature.FriendRequests.Commands.Cancel
{
    public sealed record CancelFriendRequestCommand(Guid RequesterUserId, Guid RecipientUserId) : IRequest<Result<Unit>>;
}