using System.Text;
using System.Text.Json;
using RabbitMQ.Client;

namespace Messaging.Inventory;

public class RabbitMqMessageConsumerService(ILogger<RabbitMqMessageConsumerService> logger,IModel model): DefaultBasicConsumer
{
    public override void HandleBasicDeliver(string consumerTag, ulong deliveryTag, bool redelivered, string exchange, string routingKey,
        IBasicProperties properties, ReadOnlyMemory<byte> body)
    {
      logger.LogWarning("Message Received");
      logger.LogWarning("Message Delivery Tag {messageTag}",consumerTag);
      logger.LogWarning("Exchange: {exchange}",exchange);

     logger.LogWarning("Correlation ID :{correlationId}",properties.CorrelationId);


     var message = Encoding.UTF8.GetString(body.ToArray());

     logger.LogWarning("Message Content: {messageContent}",message);


     try
     {
         var userOrder = JsonSerializer.Deserialize<UserOrderModel>(message);

         if (userOrder is not null)
         {
             //Process User Order From Inventory 

             logger.LogInformation("Processing User Order with Product Id {productId} , user Id {userId} ,amount {amount} , product name: {productName}"
                 ,userOrder.ProductId
                 ,userOrder.UserId
                 ,userOrder.Amount
                 ,userOrder.ProductName);
         }

         model.BasicAck(deliveryTag,false);
     }
     catch (Exception e)
     {
         logger.LogCritical(e,"Could not proccess order");
     }

    }
}