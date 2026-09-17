using Connectome.SocialGraph.Api.Extensions;
using Connectome.SocialGraph.Api.Models;
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
    [Route("api/v1/friends")]
    [Authorize]
    public class FriendRequestController(IMediator mediator) : Controller
    {
        [HttpPost]
        public async Task<IActionResult> SendRequest([FromBody] SendFriendRequest request)
        {
            Result<ExtractData> extractResult = this.ExtractCredentials(User, out IActionResult actionResult);
            
            if (extractResult.IsFailure)
                return actionResult;

            var command = new SendFriendRequestCommand(extractResult.Value.UserId, request.InviteCode);
            Result<Unit> result = await mediator.Send(command);

            return result.Match<IActionResult>(
                onSuccess: () => Ok(),
                onFailure: errors => BadRequest(errors));
        }
    }
}