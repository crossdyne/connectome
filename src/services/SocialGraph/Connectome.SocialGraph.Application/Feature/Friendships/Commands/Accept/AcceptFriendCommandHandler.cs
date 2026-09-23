using Connectome.SocialGraph.Application.Abstractions.Repositories;
using Connectome.SocialGraph.Domain.Models;
using Connectome.SocialGraph.Domain.ValueObjects.Common;
using Crossdyne.Toolkit.Results;
using MediatR;
using Unit = Crossdyne.Toolkit.Primitives.Unit;

namespace Connectome.SocialGraph.Application.Feature.Friendships.Commands.Accept
{
    public sealed class AcceptFriendCommandHandler(
        IFriendshipRepository repository) : IRequestHandler<AcceptFriendCommand, Result<Unit>>
    {
        public async Task<Result<Unit>> Handle(AcceptFriendCommand request, CancellationToken cancellationToken)
        {
            var friendship = Friendship.Create(
                requesterUserId: UserId.Create(request.RequesterUserId),
                acceptorUserId: UserId.Create(request.AcceptorUserId));

            Result<Unit> result = await repository.Accept(friendship);

            return result;
        }
    }
}