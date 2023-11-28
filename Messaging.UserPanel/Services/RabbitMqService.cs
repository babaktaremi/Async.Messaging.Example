using System.Text;
using RabbitMQ.Client;

namespace Messaging.UserPanel.Services;

public class RabbitMqService(IModel rabbitMqModel)
{
    public void PublishMessage(string queue, string exchange, string message)
    {
        rabbitMqModel.ExchangeDeclare(exchange,ExchangeType.Fanout,true,false);

        rabbitMqModel.QueueDeclare(queue, true, false, false);

        rabbitMqModel.QueueBind(queue,exchange,string.Empty);

        rabbitMqModel.BasicPublish(exchange,string.Empty,false,rabbitMqModel.CreateBasicProperties(),Encoding.UTF8.GetBytes(message));

    }
}