using BuildingBlocks.Common;
using BuildingBlocks.Implementation;
using BuildingBlocks.Interfaces;
using BuildingBlocks.Messaging;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using OrderService.Application.Bases;
using OrderService.Application.Interfaces;
using OrderService.Application.Sagas;
using OrderService.Infrastructure.Repositories;

namespace OrderService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDependencyInjection(
            this IServiceCollection services)
        {
            var applicationAssembly =
                typeof(ApplicationAssemblyMarker).Assembly;

            services.AddValidatorsFromAssembly(
                applicationAssembly);

            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(
                    applicationAssembly);

                cfg.AddBehavior(
                    typeof(IPipelineBehavior<,>),
                    typeof(ValidationBehavior<,>));
            });

            services.AddAutoMapper(
                cfg => { },
                applicationAssembly);

            services.AddScoped<
                IOrderRepository,
                OrderRepository>();

            services.AddScoped<
                ICartRepository,
                CartRepository>();

            services.AddScoped<
                ICurrentUserService,
                CurrentUserService>();

            services.AddScoped<
                IProcessedEventRepository,
                ProcessedEventRepository>();

            services.AddScoped<
                IOutboxRepository,
                OutboxRepository>();

            services.AddScoped<
                IOrderSagaRepository,
                OrderSagaRepository>();

            services.AddScoped<
                IOrderSagaOrchestrator,
                OrderSagaOrchestrator>();
            services.AddSingleton<
                IEventPublisher,
                KafkaEventPublisher>();
            return services;
        }
    }
}
