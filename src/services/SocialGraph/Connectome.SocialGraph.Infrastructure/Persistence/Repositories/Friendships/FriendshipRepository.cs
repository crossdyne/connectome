using Connectome.SocialGraph.Application.Abstractions.Repositories;
using Connectome.SocialGraph.Domain.Models;
using Connectome.SocialGraph.Infrastructure.Constants;
using Connectome.SocialGraph.Infrastructure.Helpers;
using Crossdyne.Toolkit.Primitives;
using Crossdyne.Toolkit.Results;
using Microsoft.Extensions.Logging;
using Neo4j.Driver;
using Shared.Contracts.SocialGraph.Responses;

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

        public async Task<List<FriendResponse>> Friends(Guid userId)
        {
            var query = CypherLoader.Load<FriendshipRepository>("GetFriends.cypher");

            logger.LogInformation("Создание сессии для получение списка друзей пользователя userId={userId}", userId);

            await using IAsyncSession session = driver.AsyncSession();

            return await session.ExecuteReadAsync(async tx =>
            {
                var parameters = new
                {
                    projectId = Neo4jConstants.ProjectIdentifier,
                    userId = userId.ToString()
                };

                IResultCursor result = await tx.RunAsync(query, parameters);
                List<string> userIds = await result.ToListAsync(record => record["userId"].As<string>());

                logger.LogInformation("Успешно получен список друзей пользователя userId={userId}. Количество: {Count}", userId, userIds.Count);

                return userIds.Select(id => new FriendResponse(id)).ToList();
            });
        }
    }
}