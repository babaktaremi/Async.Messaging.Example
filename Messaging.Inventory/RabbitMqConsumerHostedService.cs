using RabbitMQ.Client;

namespace Messaging.Inventory;

public class RabbitMqConsumerHostedService(RabbitMqMessageConsumerService messageConsumer,IModel rabbitMqModel,IConfiguration configuration):BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            rabbitMqModel.BasicConsume(messageConsumer, configuration["QueueName"]);
            await Task.Delay(1000, stoppingToken);
        }
    }
}