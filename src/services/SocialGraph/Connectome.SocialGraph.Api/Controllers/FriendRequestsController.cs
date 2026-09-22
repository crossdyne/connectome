using Connectome.SocialGraph.Api.Extensions;
using Connectome.SocialGraph.Api.Models;
using Connectome.SocialGraph.Application.Feature.FriendRequests.Commands.Reject;
using Connectome.SocialGraph.Application.Feature.FriendRequests.Commands.Send;
using Crossdyne.Toolkit.Results;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Contracts.SocialGraph.Requests;
using Unit = Crossdyne.Toolkit.Primitives.Unit;

namespace Connectome.SocialGraph.Api.Controllers
{
    [ApiController]
    [Route("api/v1/friends/request")]
    [Authorize]
    public class FriendRequestsController(IMediator mediator) : Controller
    {
        [HttpPost("send")]
        public async Task<IActionResult> SendRequest([FromBody] SendFriendRequest request)
        {
            Result<ExtractData> extractResult = this.ExtractCredentials(User, out IActionResult actionResult);
            
            if (extractResult.IsFailure)
                return actionResult;

            var command = new SendFriendRequestCommand(
                FromUserId: extractResult.Value.UserId, 
                ToUserInviteCode: request.InviteCode);

            Result<Unit> result = await mediator.Send(command);

            return result.Match<IActionResult>(
                onSuccess: () => Ok(),
                onFailure: errors => BadRequest(errors));
        }

        [HttpPost("reject")]
        public async Task<IActionResult> RejectFriend([FromBody] RejectFriendRequest request)
        {
            Result<ExtractData> extractResult = this.ExtractCredentials(User, out IActionResult actionResult);
            
            if (extractResult.IsFailure)
                return actionResult;

            var command = new RejectFriendCommand(
                RequesterUserId: Guid.Parse(request.RequesterUserId), 
                RecipientUserId: extractResult.Value.UserId);

            Result<Unit> result = await mediator.Send(command);

            return result.Match<IActionResult>(
                onSuccess: () => Ok(),
                onFailure: errors => BadRequest(errors));
        }
    }
}