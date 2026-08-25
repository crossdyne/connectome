using Connectome.SocialGraph.Infrastructure.Extensions;
using Serilog;
using Shared.Logging;

var builder = WebApplication.CreateBuilder(args);

IConfiguration configuration = builder.Configuration;

builder.Host.AddSerilogLogger();
builder.Services
    .AddOpenApi()
    .AddDataBase(configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
    app.MapOpenApi();

app.UseHttpsRedirection();
app.UseSerilogRequestLogging();

app.Logger.LogInformation("Приложение успешно запущено и готово к работе!");

app.Run();