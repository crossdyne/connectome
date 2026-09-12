using Connectome.SocialGraph.Infrastructure.Extensions;
using Connectome.SocialGraph.Infrastructure.Persistence.Migrations;
using Serilog;
using Shared.Logging;

var builder = WebApplication.CreateBuilder(args);

IConfiguration configuration = builder.Configuration;

builder.Host.AddSerilogLogger();
builder.Services
    .AddOpenApi()
    .AddDataBase(configuration)
    .AddEventHandlers(configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
    app.MapOpenApi();

app.UseHttpsRedirection();
app.UseSerilogRequestLogging();

app.Logger.LogInformation("Приложение успешно запустилось и готово к работе! 🚀");

using (var scope = app.Services.CreateScope())
{
    var migrationService = scope.ServiceProvider.GetRequiredService<Neo4jMigrationService>();
    await migrationService.MigrateAsync();
}

app.Run();