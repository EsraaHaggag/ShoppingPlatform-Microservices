using BuildingBlocks.OutBox;
using Confluent.Kafka;
using Microsoft.EntityFrameworkCore;
using OrderService.Application.Interfaces;
using OrderService.Infrastructure;
using OrderService.Infrastructure.Messaging;
using OrderService.Infrastructure.Persistence;
using OrderService.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDependencyInjection();

builder.Services.AddDbContext<OrderDbContext>(options =>
    options.UseSqlServer(
    builder.Configuration.GetConnectionString("OrderDb")));
//builder.Services.AddSingleton<IProducer<string, string>>(sp =>
//{
//    var config = new ProducerConfig
//    {
//        BootstrapServers =
//            builder.Configuration["Kafka:BootstrapServers"]
//    };

//    return new ProducerBuilder<string, string>(config)
//        .Build();
//});
var kafkaConnection =
    builder.Configuration.GetConnectionString("kafka");

builder.Services.AddSingleton<IProducer<string, string>>(_ =>
{
    var config = new ProducerConfig
    {
        BootstrapServers = kafkaConnection,
        MessageTimeoutMs = 10000,
        SocketTimeoutMs = 10000
    };

    return new ProducerBuilder<string, string>(config).Build();
});
builder.Services
    .AddHttpClient<
        IProductServiceClient,
        ProductServiceClient>(client =>
        {
            client.BaseAddress = new Uri(
                builder.Configuration["Services:ProductService"]!);
        })
    .AddStandardResilienceHandler(options =>
    {
        // Timeout
        options.TotalRequestTimeout.Timeout =
            TimeSpan.FromSeconds(50);

        // Retry
        options.Retry.MaxRetryAttempts = 3;

        options.Retry.OnRetry = args =>
        {
            Console.WriteLine(
                $"🔄 Retry attempt: {args.AttemptNumber + 1}");

            return default;
        };

        options.CircuitBreaker.SamplingDuration =
            TimeSpan.FromSeconds(30);

        options.CircuitBreaker.OnOpened = args =>
        {

            return default;
        };
        options.CircuitBreaker.OnClosed = args =>
        {
            Console.WriteLine("🟢 Circuit Breaker CLOSED");
            return default;
        };
    });
builder.Services.AddHttpClient<
    IPaymentServiceClient,
    PaymentServiceClient>(client =>
    {
        client.BaseAddress = new Uri(
            builder.Configuration["Services:PaymentService"]!);
    });
builder.Services.AddHostedService<PaymentStatusChangedConsumer>();
builder.Services.AddHostedService<OutboxPublisher>();
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
