using Connectome.SocialGraph.Api.Extensions;
using Connectome.SocialGraph.Api.Models;
using Connectome.SocialGraph.Application.Feature.Friendships.Commands.Accept;
using Crossdyne.Toolkit.Results;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Contracts.SocialGraph.Requests;
using Unit = Crossdyne.Toolkit.Primitives.Unit;

namespace Connectome.SocialGraph.Api.Controllers
{
    [ApiController]
    [Route("api/v1/friendship")]
    [Authorize]
    public class FriendshipsController(IMediator mediator) : Controller
    {
        [HttpPost("accept")]
        public async Task<IActionResult> AcceptFriend([FromBody] AcceptFriendRequest request)
        {
            Result<ExtractData> extractResult = this.ExtractCredentials(User, out IActionResult actionResult);

            if (extractResult.IsFailure)
                return actionResult;

            var command = new AcceptFriendCommand(
                RequesterUserId: Guid.Parse(request.RequesterUserId) , 
                AcceptorUserId: extractResult.Value.UserId);
                
            Result<Unit> result = await mediator.Send(command);

            return result.Match<IActionResult>(
                onSuccess: () => Ok(),
                onFailure: errors => BadRequest(errors));
        }
    }
}