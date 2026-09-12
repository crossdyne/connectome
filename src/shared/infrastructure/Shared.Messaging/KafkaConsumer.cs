using System.Text.Json;
using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Shared.Abstractions.Messaging;

namespace Shared.Messaging
{
    public sealed class KafkaConsumer<TEvent>(
        IOptions<ConsumerConfig> options, 
        string topic, 
        IServiceProvider serviceProvider,
        ILogger<KafkaConsumer<TEvent>> logger) : BackgroundService where TEvent : class, IIntegrationEvent
    {
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true,
            WriteIndented = false
        };

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
            => Task.Run(() => RunConsumerLoop(stoppingToken), stoppingToken);

        private async Task RunConsumerLoop(CancellationToken stoppingToken)
        {
            var config = options.Value;
            config.EnableAutoCommit = false;
            config.EnableAutoOffsetStore = false;

            var builder = new ConsumerBuilder<string, string>(config);
            
            builder.SetPartitionsRevokedHandler((consumer, partitions) =>
            {
                logger.LogWarning("Консьюмер теряет партиции: {Partitions}", string.Join(", ", partitions));
            });

            builder.SetPartitionsAssignedHandler((consumer, partitions) =>
            {
                logger.LogInformation("Консьюмер получил партиции: {Partitions}", string.Join(", ", partitions));
            });

            using var consumer = builder.Build();
            consumer.Subscribe(topic);
            
            logger.LogInformation("Consumer запущен. Подписка на топик: {Topic}", topic);

            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    try
                    {
                        var consumeResult = consumer.Consume(stoppingToken);

                        if (consumeResult?.Message?.Value == null)
                            continue;

                        TEvent? integrationEvent;
                        try
                        {
                            integrationEvent = JsonSerializer.Deserialize<TEvent>(consumeResult.Message.Value, JsonOptions);
                            logger.LogInformation("Начало обработки события {event}", integrationEvent?.GetType().Name);
                        }
                        catch (JsonException ex)
                        {
                            logger.LogError(ex, "Ошибка десериализации сообщения из топика {Topic}. Сообщение пропущено. Offset: {Offset}", topic, consumeResult.Offset.Value);
                            consumer.Commit(consumeResult); 
                            continue;
                        }

                        if (integrationEvent == null)
                        {
                            consumer.Commit(consumeResult);
                            continue;
                        }

                        try
                        {
                            await ProcessEventAsync(integrationEvent, stoppingToken);
                            consumer.Commit(consumeResult);
                            logger.LogInformation("Успешная обработка события {event}", integrationEvent.GetType().Name);
                        }
                        catch (Exception ex)
                        {
                            logger.LogError(ex, "Ошибка обработки события {EventType} из топика {Topic}. Offset: {Offset}. Сообщение НЕ закоммичено, будет повторено.", typeof(TEvent).Name, topic, consumeResult.Offset.Value);
                            
                            await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken); 
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        logger.LogInformation("Consumer {Topic} корректно остановлен.", topic);
                        break;
                    }
                    catch (ConsumeException ex)
                    {
                        logger.LogError(ex, "Ошибка чтения из Kafka. Топик: {Topic}. Ошибка: {Reason}", topic, ex.Error.Reason);
                        await Task.Delay(TimeSpan.FromSeconds(2), stoppingToken);
                    }
                }
            }
            finally
            {
                try
                {
                    consumer.Unsubscribe();
                    logger.LogInformation("Consumer {Topic} отписался от топика.", topic);
                }
                catch (Exception ex)
                {
                    logger.LogWarning(ex, "Ошибка при отписке от топика {Topic}.", topic);
                }
                finally
                {
                    consumer.Close();
                }
            }
        }

        private async Task ProcessEventAsync(TEvent @event, CancellationToken cancellationToken)
        {
            using var scope = serviceProvider.CreateScope();
            var handler = scope.ServiceProvider.GetRequiredService<IIntegrationEventHandler<TEvent>>();
            await handler.HandleAsync(@event, cancellationToken);
        }
    }
}