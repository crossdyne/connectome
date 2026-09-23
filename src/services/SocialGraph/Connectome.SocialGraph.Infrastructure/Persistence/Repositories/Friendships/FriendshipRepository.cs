using Connectome.SocialGraph.Application.Abstractions.Repositories;
using Connectome.SocialGraph.Domain.Models;
using Connectome.SocialGraph.Infrastructure.Constants;
using Connectome.SocialGraph.Infrastructure.Helpers;
using Crossdyne.Toolkit.Primitives;
using Crossdyne.Toolkit.Results;
using Microsoft.Extensions.Logging;
using Neo4j.Driver;

namespace Connectome.SocialGraph.Infrastructure.Persistence.Repositories.Friendships
{
    public sealed class FriendshipRepository(
        IDriver driver,
        ILogger<FriendshipRepository> logger) : IFriendshipRepository
    {
        public async Task<Result<Unit>> Accept(Friendship friendship)
        {
            var query = CypherLoader.Load<FriendshipRepository>("AcceptFriend.cypher");

            logger.LogInformation("Создание сессии для принятия запроса дружбы между отправителем userId={from} и получателем userId={to}", friendship.RequesterUserId.Value, friendship.AcceptorUserId.Value);

            await using var session = driver.AsyncSession();

            await session.ExecuteWriteAsync(async tx =>
            {
                var parameters = new
                {
                    projectId = Neo4jConstants.ProjectIdentifier,
                    requesterUserId = friendship.RequesterUserId.Value.ToString(),
                    acceptorUserId = friendship.AcceptorUserId.Value.ToString(),
                    createdAt = friendship.CreatedAt
                };

                var result = await tx.RunAsync(query, parameters);
                var summary = await result.ConsumeAsync();

                logger.LogInformation("Запрос выполнен. Создано отношений: {RelationshipsCreated}, удалено отношений: {RelationshipsDeleted}", summary.Counters.RelationshipsCreated, summary.Counters.RelationshipsDeleted);
               
                if (summary.Counters.RelationshipsDeleted == 0)
                    logger.LogWarning("Запрос дружбы от пользователя userId={from} к пользователю userId={to} не найден или уже был обработан", friendship.RequesterUserId.Value, friendship.AcceptorUserId.Value);
            });

            logger.LogInformation("Успешное выполнение запроса на принятие дружбы между отправителем userId={from} и получателем userId={to}", friendship.RequesterUserId.Value, friendship.AcceptorUserId.Value);

            return Unit.Value;
        }
    }
}