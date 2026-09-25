using BuildingBlocks.Interfaces;
using BuildingBlocks.Messaging;
using BuildingBlocks.OutBox;
using Confluent.Kafka;
using Microsoft.EntityFrameworkCore;
using PaymentService.Application.Interfaces;
using PaymentService.Infrastructure;
using PaymentService.Infrastructure.Persistence;
using PaymentService.Infrastructure.Services.Paymob;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDependencyInjection();

builder.Services.AddDbContext<PaymentDbContext>(options =>
    options.UseSqlServer(
    builder.Configuration.GetConnectionString("PaymentDb")));
///Paymob
builder.Services.Configure<PaymobOptions>(
    builder.Configuration.GetSection("Paymob"));

//Kafka
builder.Services.AddSingleton<IProducer<string, string>>(
    _ =>
    {
        var config = new ProducerConfig
        {
            BootstrapServers =
                builder.Configuration["Kafka:BootstrapServers"]
        };
        return new ProducerBuilder<string, string>(config).Build();
    });

builder.Services.AddSingleton<IEventPublisher, KafkaEventPublisher>();

builder.Services.AddHttpClient<
    IPaymentProvider,
    PaymobPaymentProvider>(client =>
    {
        client.BaseAddress = new Uri(
            builder.Configuration["Paymob:BaseUrl"]!);
    });
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
