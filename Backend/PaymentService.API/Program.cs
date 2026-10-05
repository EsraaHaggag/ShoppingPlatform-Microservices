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
Console.WriteLine(
    $"Kafka Connection: {builder.Configuration.GetConnectionString("kafka")}");
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

builder.Services.AddCors(options =>
{
    options.AddPolicy("AngularPolicy", policy =>
    {
        policy
            .WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

//Kafka
//builder.Services.AddSingleton<IProducer<string, string>>(
//    _ =>
//    {
//        var config = new ProducerConfig
//        {
//            BootstrapServers = "localhost:54632"
//        };

//        return new ProducerBuilder<string, string>(config).Build();
//    });


var kafkaConnection =
    builder.Configuration.GetConnectionString("kafka");
Console.WriteLine($"🔥 Kafka Connection: {kafkaConnection}");
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
//builder.AddKafkaProducer<string, string>("kafka");
builder.Services.AddSingleton<IEventPublisher, KafkaEventPublisher>();

builder.Services.AddHttpClient<
    IPaymentProvider,
    PaymobPaymentProvider>(client =>
    {
        client.BaseAddress = new Uri(
            builder.Configuration["Paymob:BaseUrl"]!);
    });

//builder.Services.AddHostedService<KafkaTopicInitializer>();
builder.Services.AddHostedService<OutboxPublisher>();

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AngularPolicy");
app.UseAuthorization();

app.MapControllers();

app.Run();
