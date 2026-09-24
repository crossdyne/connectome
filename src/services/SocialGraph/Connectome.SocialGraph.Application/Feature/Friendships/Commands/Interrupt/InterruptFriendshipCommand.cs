using Crossdyne.Toolkit.Results;
using MediatR;
using Unit = Crossdyne.Toolkit.Primitives.Unit;

namespace Connectome.SocialGraph.Application.Feature.Friendships.Commands.Interrupt
{
    public sealed record InterruptFriendshipCommand(Guid UserId, Guid FriendId) : IRequest<Result<Unit>>;
}