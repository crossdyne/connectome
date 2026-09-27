using Connectome.SocialGraph.Application.Abstractions.Repositories;
using Connectome.SocialGraph.Domain.Models;
using Connectome.SocialGraph.Domain.ValueObjects.Common;
using Crossdyne.Toolkit.Results;
using MediatR;
using Unit = Crossdyne.Toolkit.Primitives.Unit;

namespace Connectome.SocialGraph.Application.Feature.FriendRequests.Commands.Send
{
    public class SendFriendRequestCommandHandler(
        IFriendRequestRepository repository) : IRequestHandler<SendFriendRequestCommand, Result<Unit>>
    {
        public async Task<Result<Unit>> Handle(SendFriendRequestCommand request, CancellationToken cancellationToken)
        {
            var friendRequest = FriendRequest.Create(UserId.Create(request.FromUserId), UserId.Create(request.ToUserId));

            Result<Unit> result = await repository.Send(friendRequest); 

            return result;
        }
    }
}