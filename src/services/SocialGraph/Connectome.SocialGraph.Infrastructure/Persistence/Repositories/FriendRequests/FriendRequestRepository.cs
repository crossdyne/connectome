using Connectome.SocialGraph.Application.Abstractions.Repositories;
using Connectome.SocialGraph.Domain.Models;
using Connectome.SocialGraph.Domain.ValueObjects.Common;
using Connectome.SocialGraph.Infrastructure.Constants;
using Connectome.SocialGraph.Infrastructure.Helpers;
using Crossdyne.Toolkit.Primitives;
using Crossdyne.Toolkit.Results;
using Microsoft.Extensions.Logging;
using Neo4j.Driver;
using Shared.Contracts.SocialGraph.Responses;

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
                    createdAt = request.CreatedAt
                };

                var result = await tx.RunAsync(query, parameters);
                await result.ConsumeAsync(); 
            });

            logger.LogInformation("Успешное выполнение запроса на дружбу между отправителем userId={from} и получателем userId={to}", request.FromUserId.Value, request.ToUserId.Value);

            return Unit.Value;
        }

        public async Task<Result<Unit>> Decline(UserId fromUserId, UserId toUserId)
        {
            var query = CypherLoader.Load<FriendRequestRepository>("DeclineFriendRequest.cypher");

            logger.LogInformation("Создание сессии для удаления запроса дружбы от пользователя userId={from} к пользователю userId={to}", fromUserId.Value, toUserId.Value);

            await using var session = driver.AsyncSession();

            await session.ExecuteWriteAsync(async tx =>
            {
                var parameters = new
                {
                    projectId = Neo4jConstants.ProjectIdentifier,
                    fromUserId = fromUserId.Value.ToString(),
                    toUserId = toUserId.Value.ToString()
                };

                var result = await tx.RunAsync(query, parameters);
                var summary = await result.ConsumeAsync();

                logger.LogInformation("Запрос выполнен. Удалено отношений: {RelationshipsDeleted}", summary.Counters.RelationshipsDeleted);

                if (summary.Counters.RelationshipsDeleted == 0)
                    logger.LogWarning("Запрос дружбы от пользователя userId={from} к пользователю userId={to} не найден в Neo4j", fromUserId.Value, toUserId.Value);
            });

            logger.LogInformation("Успешное выполнение запроса на удаление запроса дружбы от пользователя userId={from} к пользователю userId={to}", fromUserId.Value, toUserId.Value);

            return Unit.Value;
        }

        public async Task<Result<Unit>> Cancel(UserId fromUserId, UserId toUserId)
        {
            var query = CypherLoader.Load<FriendRequestRepository>("CancelFriendRequest.cypher");

            logger.LogInformation("Создание сессии для отмены запроса дружбы от пользователя userId={from} к пользователю userId={to}", fromUserId.Value, toUserId.Value);

            await using var session = driver.AsyncSession();

            await session.ExecuteWriteAsync(async tx =>
            {
                var parameters = new
                {
                    projectId = Neo4jConstants.ProjectIdentifier,
                    fromUserId = fromUserId.Value.ToString(),
                    toUserId = toUserId.Value.ToString()
                };

                var result = await tx.RunAsync(query, parameters);
                var summary = await result.ConsumeAsync();

                logger.LogInformation("Запрос выполнен. Удалено отношений: {RelationshipsDeleted}", summary.Counters.RelationshipsDeleted);

                if (summary.Counters.RelationshipsDeleted == 0)
                    logger.LogWarning("Запрос отмены запроса дружбы от пользователя userId={from} к пользователю userId={to} не найден в Neo4j", fromUserId.Value, toUserId.Value);
            });

            logger.LogInformation("Успешное выполнение запроса на отмену запроса дружбы от пользователя userId={from} к пользователю userId={to}", fromUserId.Value, toUserId.Value);

            return Unit.Value;
        }

        public async Task<List<IncomingFriendResponse>> IncomingFriendRequests(Guid userId)
        {
            var query = CypherLoader.Load<FriendRequestRepository>("GetIncomingFriendRequests.cypher");

            logger.LogInformation("Создание сессии для пучение списка входящих запросов дружбы для пользователя userId={userId}", userId);

            await using var session = driver.AsyncSession();

            return await session.ExecuteReadAsync(async tx =>
            {
                var parameters = new
                {
                    projectId = Neo4jConstants.ProjectIdentifier,
                    userId = userId.ToString()
                };

                IResultCursor result = await tx.RunAsync(query, parameters);

                List<string> userIds = await result.ToListAsync(record => record["userId"].As<string>());
                
                logger.LogInformation("Успешно получено входящих запросов дружбы для пользователя userId={userId}. Количество: {Count}", userId, userIds.Count);
            
                return userIds.Select(id => new IncomingFriendResponse(id)).ToList();
            });
        }

        public async Task<List<OutgoingFriendResponse>> OutgoingFriendRequests(Guid userId)
        {
            var query = CypherLoader.Load<FriendRequestRepository>("GetOutgoingFriendRequests.cypher");

            logger.LogInformation("Создание сессии для пучение списка входящих запросов дружбы для пользователя userId={userId}", userId);

            await using var session = driver.AsyncSession();

            return await session.ExecuteReadAsync(async tx =>
            {
                var parameters = new
                {
                    projectId = Neo4jConstants.ProjectIdentifier,
                    userId = userId.ToString()
                };

                IResultCursor result = await tx.RunAsync(query, parameters);

                List<string> userIds = await result.ToListAsync(record => record["userId"].As<string>());
                
                logger.LogInformation("Успешно получено входящих запросов дружбы для пользователя userId={userId}. Количество: {Count}", userId, userIds.Count);
            
                return userIds.Select(id => new OutgoingFriendResponse(id)).ToList();
            });
        }
    }
}