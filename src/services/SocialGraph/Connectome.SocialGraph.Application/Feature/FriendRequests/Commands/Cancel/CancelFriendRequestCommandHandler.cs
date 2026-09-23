using Connectome.SocialGraph.Application.Abstractions.Repositories;
using Connectome.SocialGraph.Domain.ValueObjects.Common;
using Crossdyne.Toolkit.Results;
using MediatR;
using Unit = Crossdyne.Toolkit.Primitives.Unit;

namespace Connectome.SocialGraph.Application.Feature.FriendRequests.Commands.Cancel
{
    public sealed class CancelFriendRequestCommandHandler(
        IFriendRequestRepository repository) : IRequestHandler<CancelFriendRequestCommand, Result<Unit>>
    {
        public async Task<Result<Unit>> Handle(CancelFriendRequestCommand request, CancellationToken cancellationToken)
            => await repository.Cancel(UserId.Create(request.RequesterUserId), UserId.Create(request.RecipientUserId));
    }
}