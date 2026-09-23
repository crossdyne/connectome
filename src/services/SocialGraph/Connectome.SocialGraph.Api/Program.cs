using Connectome.SocialGraph.Api.Extensions;
using Connectome.SocialGraph.Application.Extensions;
using Connectome.SocialGraph.Infrastructure.Extensions;
using Connectome.SocialGraph.Infrastructure.Persistence.Migrations;
using Serilog;
using Shared.Logging;

var builder = WebApplication.CreateBuilder(args);

IConfiguration configuration = builder.Configuration;

builder.Host.AddSerilogLogger();

builder.Services.AddControllers();
builder.Services
    .AddOpenApi()
    .RegisterAuthentication(configuration)
    .AddDataBase(configuration)
    .AddEventHandlers(configuration)
    .UseMediator()
    .UseHttpService(configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
    app.MapOpenApi();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.UseSerilogRequestLogging();

app.Logger.LogInformation("Приложение успешно запустилось и готово к работе! 🚀");

using (var scope = app.Services.CreateScope())
{
    var migrationService = scope.ServiceProvider.GetRequiredService<Neo4jMigrationService>();
    await migrationService.MigrateAsync();
}

app.Run();