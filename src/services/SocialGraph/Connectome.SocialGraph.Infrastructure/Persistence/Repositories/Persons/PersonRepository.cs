using Connectome.SocialGraph.Application.Abstractions.Repositories;
using Connectome.SocialGraph.Domain.Models;
using Connectome.SocialGraph.Domain.ValueObjects.Common;
using Connectome.SocialGraph.Domain.ValueObjects.Person;
using Connectome.SocialGraph.Infrastructure.Constants;
using Connectome.SocialGraph.Infrastructure.Helpers;
using Crossdyne.Toolkit.Primitives;
using Crossdyne.Toolkit.Results;
using Microsoft.Extensions.Logging;
using Neo4j.Driver;
using Shared.Contracts.Common;

namespace Connectome.SocialGraph.Infrastructure.Persistence.Repositories.Persons
{
    internal class PersonRepository(
        IDriver driver, 
        ILogger<PersonRepository> logger) : IPersonRepository
    {
        public async Task<Result<NewRecordResponse>> CreateAsync(Person person, CancellationToken cl)
        {
            var query = CypherLoader.Load<PersonRepository>("CreatePerson.cypher");

            logger.LogInformation("Создание сессии для создание сущности {Person} с userId={UserId}", nameof(Person), person.UserId.Value);
            await using var session = driver.AsyncSession(); 

            logger.LogInformation("Выполнение запроса на создание {Person}", nameof(Person));
            await session.ExecuteWriteAsync(async tx =>
            {
                var parameters = new
                {
                    projectId = Neo4jConstants.ProjectIdentifier,
                    personId = person.Id.Value.ToString(),
                    userId = person.UserId.Value.ToString(),
                    userName = person.UserName.Value
                };

                await tx.RunAsync(query, parameters);
            });

            logger.LogInformation("Успешное выполнение запроса на создание {Person}", nameof(Person));

            return new NewRecordResponse(person.Id.Value.ToString());
        }

        public async Task<Result<Unit>> RemoveAsync(UserId userId)
        {
            var query = CypherLoader.Load<PersonRepository>("RemovePerson.cypher");

            logger.LogInformation("Создание сессии для удаление сущности {Person} с userId={userId}", nameof(Person), userId.Value);
            await using var session = driver.AsyncSession();

            logger.LogInformation("Выполнение запроса на удаление {person}", nameof(Person));
            await session.ExecuteWriteAsync(async tx =>
            {
                var parameters = new
                {
                    projectId = Neo4jConstants.ProjectIdentifier,
                    userId = userId.Value.ToString()
                };

                var result = await tx.RunAsync(query, parameters);
                var summary = await result.ConsumeAsync();

                logger.LogInformation("Запрос выполнен. Удалено узлов: {NodesDeleted}, удалено связей: {RelationshipsDeleted}", summary.Counters.NodesDeleted, summary.Counters.RelationshipsDeleted);

                if (summary.Counters.NodesDeleted == 0)
                    logger.LogWarning("Person с userId {UserId} не найден в Neo4j", userId.Value);
            });

            logger.LogInformation("Успешное выполнение запроса на удаление {person}", nameof(Person));

            return Unit.Value;
        }
    }
}