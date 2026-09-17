using Connectome.SocialGraph.Application.Abstractions.Repositories;
using Connectome.SocialGraph.Domain.Models;
using Connectome.SocialGraph.Infrastructure.Constants;
using Connectome.SocialGraph.Infrastructure.Helpers;
using Crossdyne.Toolkit.Primitives;
using Crossdyne.Toolkit.Results;
using Microsoft.Extensions.Logging;
using Neo4j.Driver;

namespace Connectome.SocialGraph.Infrastructure.Persistence.Repositories.FriendRequests
{
    internal sealed class FriendRequestRepository(
        IDriver driver,
        ILogger<FriendRequestRepository> logger) : IFriendRequestRepository
    {
        public async Task<Result<Unit>> Send(FriendRequest request)
        {
            var query = CypherLoader.Load<FriendRequestRepository>("SendFriendRequest.cypher");

            logger.LogInformation("Создание сессии для отправки запроса дружбы между отправителем userId={from} и получателем userId={to}", request.FromUserId.Value, request.ToUserId.Value);
            await using var session = driver.AsyncSession();

            await session.ExecuteWriteAsync(async tx =>
            {
                var parameters = new
                {
                    projectId = Neo4jConstants.ProjectIdentifier,
                    fromUserId = request.FromUserId.Value.ToString(),
                    toUserId = request.ToUserId.Value.ToString(),
                    friendRequestId = request.Id.Value.ToString(),
                    status = request.Status.Name,
                    createdAt = request.CreatedAt
                };

                var result = await tx.RunAsync(query, parameters);
                await result.ConsumeAsync(); 
            });

            logger.LogInformation("Успешное выполнение запроса на дружбу между отправителем userId={from} и получателем userId={to}", request.FromUserId.Value, request.ToUserId.Value);

            return Unit.Value;
        }
    }
}