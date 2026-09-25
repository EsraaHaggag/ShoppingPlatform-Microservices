using BuildingBlocks.Interfaces;
using BuildingBlocks.Messaging;
using Confluent.Kafka;
using Microsoft.EntityFrameworkCore;
using ProductService.Infrastructure;
using ProductService.Infrastructure.Messaging;
using ProductService.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDependencyInjection();

builder.Services.AddDbContext<ProductDbContext>(options =>
    options.UseSqlServer(
      builder.Configuration.GetConnectionString("ProductDb")));
builder.Services.AddSingleton<IProducer<string, string>>(sp =>
{
    var config = new ProducerConfig
    {
        BootstrapServers =
            builder.Configuration["Kafka:BootstrapServers"]
    };

    return new ProducerBuilder<string, string>(config)
        .Build();
});
builder.Services.AddHostedService<OrderPaidConsumer>();
builder.Services.AddSingleton<IEventPublisher, KafkaEventPublisher>();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
