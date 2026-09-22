using Connectome.SocialGraph.Api.Extensions;
using Connectome.SocialGraph.Api.Models;
using Connectome.SocialGraph.Application.Feature.FriendRequests.Commands.Reject;
using Connectome.SocialGraph.Application.Feature.FriendRequests.Commands.Send;
using Connectome.SocialGraph.Application.Feature.FriendRequests.Queries.IncomingRequests;
using Connectome.SocialGraph.Application.Feature.FriendRequests.Queries.OutgoingRequests;
using Crossdyne.Toolkit.Results;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Contracts.SocialGraph.Requests;
using Shared.Contracts.SocialGraph.Responses;
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

        [HttpGet("incoming")]
        public async Task<IActionResult> IncomingRequests()
        {
            Result<ExtractData> extractResult = this.ExtractCredentials(User, out IActionResult actionResult);

            if (extractResult.IsFailure)
                return actionResult;

            var query = new IncomingRequestsQuery(extractResult.Value.UserId);
            Result<List<IncomingFriendResponse>> result = await mediator.Send(query);

            return result.Match<IActionResult>(
                onSuccess: () => Ok(result.Value),
                onFailure: errors => BadRequest(errors));
        }

        [HttpGet("outgoing")]
        public async Task<IActionResult> OutgoingRequests()
        {
            Result<ExtractData> extractResult = this.ExtractCredentials(User, out IActionResult actionResult);

            if (extractResult.IsFailure)
                return actionResult;

            var query = new OutgoingRequestsQuery(extractResult.Value.UserId);
            Result<List<OutgoingFriendResponse>> result = await mediator.Send(query);

            return result.Match<IActionResult>(
                onSuccess: () => Ok(result.Value),
                onFailure: errors => BadRequest(errors));
        }
    }
}