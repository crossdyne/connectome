using Crossdyne.Toolkit.Results;
using MediatR;
using Unit = Crossdyne.Toolkit.Primitives.Unit;

namespace Connectome.SocialGraph.Application.Feature.FriendRequests.Commands.Send
{
    public sealed record SendFriendRequestCommand(Guid FromUserId, string ToUserInviteCode) : IRequest<Result<Unit>>;
}