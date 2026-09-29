using BuildingBlocks.Interfaces;
using BuildingBlocks.Messaging;
using Confluent.Kafka;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using ProductService.Infrastructure;
using ProductService.Infrastructure.Messaging;
using ProductService.Infrastructure.Persistence;
using StackExchange.Redis;
using System.Text.Json;

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

//builder.Services.AddHealthChecks()
//    .AddSqlServer(
//        builder.Configuration.GetConnectionString("ProductDb")!)
//    .AddRedis(
//        builder.Configuration.GetConnectionString("Redis")!);

builder.Services.AddHealthChecks()
    .AddSqlServer(
        builder.Configuration.GetConnectionString("ProductDb")!,
        name: "sqlserver",
        tags: new[] { "ready" })
    .AddRedis(
        builder.Configuration.GetConnectionString("Redis")!,
        name: "redis",
        tags: new[] { "ready" });

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration =
        builder.Configuration.GetConnectionString("Redis");
});

builder.Services.AddSingleton<IConnectionMultiplexer>(
    _ => ConnectionMultiplexer.Connect(
        builder.Configuration.GetConnectionString("Redis")!));
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
builder.Services.AddHostedService<ReleaseStockConsumer>();
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

app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";

        var response = new
        {
            status = report.Status.ToString(),
            checks = report.Entries.ToDictionary(
                entry => entry.Key,
                entry => entry.Value.Status.ToString())
        };

        await context.Response.WriteAsync(
            JsonSerializer.Serialize(response));
    }
});
app.MapHealthChecks("/health/live", new HealthCheckOptions
{
    Predicate = _ => false,

    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";

        var response = new
        {
            status = report.Status.ToString(),
            checks = report.Entries.ToDictionary(
                entry => entry.Key,
                entry => entry.Value.Status.ToString())
        };

        await context.Response.WriteAsync(
            JsonSerializer.Serialize(response));
    }
});

app.MapHealthChecks("/health/ready", new HealthCheckOptions
{
    Predicate = check =>
        check.Tags.Contains("ready"),

    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";

        var response = new
        {
            status = report.Status.ToString(),
            checks = report.Entries.ToDictionary(
                entry => entry.Key,
                entry => entry.Value.Status.ToString())
        };

        await context.Response.WriteAsync(
            JsonSerializer.Serialize(response));
    }
});
app.Run();
