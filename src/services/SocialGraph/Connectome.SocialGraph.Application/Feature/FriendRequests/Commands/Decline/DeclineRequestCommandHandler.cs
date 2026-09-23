using Connectome.SocialGraph.Application.Abstractions.Repositories;
using Connectome.SocialGraph.Domain.ValueObjects.Common;
using Crossdyne.Toolkit.Results;
using MediatR;
using Unit = Crossdyne.Toolkit.Primitives.Unit;

namespace Connectome.SocialGraph.Application.Feature.FriendRequests.Commands.Decline
{
        public sealed class DeclineRequestCommandHandler(
            IFriendRequestRepository repository) : IRequestHandler<DeclineRequestCommand, Result<Unit>>
        {
            public async Task<Result<Unit>> Handle(DeclineRequestCommand request, CancellationToken cancellationToken)
                => await repository.Decline(UserId.Create(request.RequesterUserId), UserId.Create(request.RecipientUserId));
        }
}