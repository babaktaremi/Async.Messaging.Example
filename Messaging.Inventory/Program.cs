using Messaging.Inventory;
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
        ClientProvidedName = "InventoryService",
        Uri = new Uri(builder.Configuration["RabbitmqConnection"]!),
    };

    var rabbitMqConnection = connectionFactory.CreateConnection();

    return rabbitMqConnection.CreateModel();
});

builder.Services.AddSingleton<RabbitMqMessageConsumerService>();

builder.Services.AddHostedService<RabbitMqConsumerHostedService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();



app.Run();


