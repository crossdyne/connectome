using Connectome.SocialGraph.Application.Abstractions.Repositories;
using Crossdyne.Toolkit.Results;
using MediatR;
using Unit = Crossdyne.Toolkit.Primitives.Unit;

namespace Connectome.SocialGraph.Application.Feature.Friendships.Commands.Interrupt
{
    public sealed class InterruptFriendshipCommandHandler(IFriendshipRepository repository) : IRequestHandler<InterruptFriendshipCommand, Result<Unit>>
    {
        public async Task<Result<Unit>> Handle(InterruptFriendshipCommand request, CancellationToken cancellationToken)
            => await repository.DeleteFriend(request.UserId, request.FriendId);
    }
}