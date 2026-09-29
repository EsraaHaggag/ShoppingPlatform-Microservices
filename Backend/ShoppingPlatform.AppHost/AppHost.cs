var builder = DistributedApplication.CreateBuilder(args);

var redis = builder.AddRedis("redis");

var kafka = builder.AddKafka("kafka");

var productService =
    builder.AddProject<Projects.ProductService_API>("product-service")
           .WithReference(redis)
           .WithReference(kafka)
           .WaitFor(kafka);

var orderService =
    builder.AddProject<Projects.OrderService_API>("order-service")
           .WithReference(kafka)
           .WaitFor(kafka);

var paymentService =
    builder.AddProject<Projects.PaymentService_API>("payment-service")
           .WithReference(kafka)
           .WaitFor(kafka);

builder.Build().Run();