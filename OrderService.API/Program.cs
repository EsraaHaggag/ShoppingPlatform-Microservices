using Microsoft.EntityFrameworkCore;
using OrderService.Application.Interfaces;
using OrderService.Infrastructure;
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

builder.Services.AddHttpClient<
    IProductServiceClient,
    ProductServiceClient>(client =>
    {
        client.BaseAddress = new Uri(
            builder.Configuration["Services:ProductService"]!);
    });
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
