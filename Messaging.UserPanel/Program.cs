using System.Text.Json;
using Messaging.UserPanel.Services;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton(_ =>
{
    var connectionFactory = new ConnectionFactory()
    {
        ClientProvidedName = "UserPanel",
        Uri = new Uri(builder.Configuration["RabbitmqConnection"]!),
    };

    var rabbitMqConnection = connectionFactory.CreateConnection();

    return new RabbitMqService(rabbitMqConnection.CreateModel());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/SendUserRequestToInventory",  (UserOrderModel model, RabbitMqService rabbitMqService,IConfiguration configuration) =>
{
    var message = JsonSerializer.Serialize(model);

    rabbitMqService.PublishMessage(configuration["QueueName"]!, configuration["ExchangeName"]!,message);

    return Results.Ok();
});

app.Run();



public record UserOrderModel(int UserId,int ProductId,string ProductName,int Amount);