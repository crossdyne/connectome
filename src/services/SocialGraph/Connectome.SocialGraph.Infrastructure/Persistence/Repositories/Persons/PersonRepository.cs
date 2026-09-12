using Connectome.SocialGraph.Application.Abstractions.Repositories;
using Connectome.SocialGraph.Domain.Models;
using Connectome.SocialGraph.Infrastructure.Constants;
using Connectome.SocialGraph.Infrastructure.Helpers;
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

            logger.LogInformation("Создание сессии");
            await using var session = driver.AsyncSession(); 

            logger.LogInformation("Выполнение запроса на создание {person}", nameof(Person));
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

            logger.LogInformation("Успешное выполнение запроса на создание {person}", nameof(Person));

            return new NewRecordResponse(person.Id.Value.ToString());
        } 
    }
}