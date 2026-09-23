using Connectome.SocialGraph.Application.Abstractions.Repositories;
using Connectome.SocialGraph.Domain.Models;
using Connectome.SocialGraph.Domain.ValueObjects.Common;
using Connectome.UserManagement.Service.Client;
using Crossdyne.Toolkit.Results;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared.Contracts.UserManagement.Responses;
using Unit = Crossdyne.Toolkit.Primitives.Unit;

namespace Connectome.SocialGraph.Application.Feature.FriendRequests.Commands.Send
{
    public class SendFriendRequestCommandHandler(
        IFriendRequestRepository repository, 
        IUserManagementService userManagementService) : IRequestHandler<SendFriendRequestCommand, Result<Unit>>
    {
        public async Task<Result<Unit>> Handle(SendFriendRequestCommand request, CancellationToken cancellationToken)
        {
            Result<ByInviteCodeResponse> byInviteCodeResult = await userManagementService.GetByInviteCode(request.ToUserInviteCode);
            
            if (byInviteCodeResult.IsFailure)
                return byInviteCodeResult.Map(unit => Unit.Value);

            var friendRequest = FriendRequest.Create(UserId.Create(request.FromUserId), UserId.Create(Guid.Parse(byInviteCodeResult.Value.UserId)));

            Result<Unit> result = await repository.Send(friendRequest); 

            return result;
        }
    }
}