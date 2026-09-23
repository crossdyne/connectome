using Crossdyne.Toolkit.Results;
using MediatR;
using Unit = Crossdyne.Toolkit.Primitives.Unit;

namespace Connectome.SocialGraph.Application.Feature.Friendships.Commands.Accept
{
    public sealed record AcceptFriendCommand(Guid RequesterUserId, Guid AcceptorUserId) : IRequest<Result<Unit>>;
}