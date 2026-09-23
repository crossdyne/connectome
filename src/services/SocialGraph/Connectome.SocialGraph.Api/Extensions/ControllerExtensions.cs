using System.Security.Claims;
using Connectome.SocialGraph.Api.Models;
using Crossdyne.Toolkit.Results;
using Microsoft.AspNetCore.Mvc;

namespace Connectome.SocialGraph.Api.Extensions
{
    public static class ControllerExtensions
    {
        public static Result<ExtractData> ExtractCredentials(this Controller controller, ClaimsPrincipal user, out IActionResult actionResult)
        {
            var extractData = new ExtractData();

            var userIdString = user.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdString))
            {
                actionResult = controller.Unauthorized("User ID не найден в токене.");
                return new Error(ErrorCode.Unauthorized, "");
            }
                
            if (!Guid.TryParse(userIdString, out var userIdGuid))
            {
                 actionResult = controller.BadRequest("Не верный User ID формат.");
                 return new Error(ErrorCode.BadRequest, "Формат идентификатора был не верный");
            }

            actionResult = controller.Ok();
            extractData.UserId = userIdGuid;

            return extractData;
        }
    }
}