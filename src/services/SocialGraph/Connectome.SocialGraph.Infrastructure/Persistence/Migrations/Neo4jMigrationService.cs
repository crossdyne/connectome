using Connectome.SocialGraph.Infrastructure.Helpers;
using Microsoft.Extensions.Logging;
using Neo4j.Driver;

namespace Connectome.SocialGraph.Infrastructure.Persistence.Migrations
{
    public class Neo4jMigrationService(
        IDriver driver,
        ILogger<Neo4jMigrationService> logger)
    {
       public async Task MigrateAsync()
        {
            logger.LogInformation("Применение миграций Neo4j...");

            await ExecuteMigrationAsync("001_CreatePersonProjectPersonIdConstraint");
            await ExecuteMigrationAsync("002_CreatePersonProjectUserIdConstraint");
            await ExecuteMigrationAsync("003_CreatePersonProjectUserNameIndex");
            await ExecuteMigrationAsync("004_CreateFriendRequestCreatedAtIndex");
            await ExecuteMigrationAsync("005_CreateFriendshipCreatedAtIndex");
            await ExecuteMigrationAsync("006_CreateFriendshipBlockedAtIndex");
            await ExecuteMigrationAsync("007_CreateFriendRequestUniqueConstraint");
            await ExecuteMigrationAsync("008_CreateFriendshipUniqueConstraint");
            await ExecuteMigrationAsync("009_CreateFriendRequestToUserIdIndex");
            await ExecuteMigrationAsync("010_CreateFriendshipRequesterUserIdIndex");
            await ExecuteMigrationAsync("011_CreateFriendshipAcceptorUserIdIndex");

            logger.LogInformation("Все миграции применены");
        }

        private async Task ExecuteMigrationAsync(string migrationName)
        {
            try
            {
                var cypher = CypherLoader.Load<Neo4jMigrationService>($"{migrationName}.cypher");
                
                await using var session = driver.AsyncSession();
                await session.ExecuteWriteAsync(async tx =>
                {
                    await tx.RunAsync(cypher);
                });

                logger.LogInformation("Миграция {Name} применена", migrationName);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Ошибка применения миграции {Name}", migrationName);
                throw;
            }
        }
    }
}