using BuildingBlocks.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BuildingBlocks.OutBox
{
    public class OutboxPublisher : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IEventPublisher _eventPublisher;

        public OutboxPublisher(
            IServiceScopeFactory scopeFactory,
            IEventPublisher eventPublisher)
        {
            _scopeFactory = scopeFactory;
            _eventPublisher = eventPublisher;
        }

        protected override async Task ExecuteAsync(
            CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope =
                        _scopeFactory.CreateScope();

                    var outboxRepository =
                        scope.ServiceProvider
                            .GetRequiredService<IOutboxRepository>();

                    var messages =
                        await outboxRepository.GetPendingAsync(20, stoppingToken);

                    foreach (var message in messages)
                    {
                        try
                        {
                            await _eventPublisher.PublishAsync(
                                message.Topic,
                                message.Key,
                                message.Payload,
                                stoppingToken);

                            message.MarkAsProcessed();
                        }
                        catch (Exception ex)
                        {
                            message.MarkAsFailed(ex.Message);
                        }
                    }

                    await outboxRepository.CompleteAsync(
                        stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(
                        $"Outbox publisher error: {ex}");
                }

                await Task.Delay(
                    TimeSpan.FromSeconds(2),
                    stoppingToken);
            }
        }
    }
}
