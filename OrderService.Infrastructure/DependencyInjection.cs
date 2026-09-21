using BuildingBlocks.Common;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using OrderService.Application.Bases;
using OrderService.Application.Interfaces;
using OrderService.Infrastructure.Repositories;
using OrderService.Infrastructure.Services;

namespace OrderService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDependencyInjection(
        this IServiceCollection services)
        {
            var applicationAssembly =
             typeof(ApplicationAssemblyMarker).Assembly;

            services.AddValidatorsFromAssembly(applicationAssembly);

            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(applicationAssembly);

                cfg.AddBehavior(
                   typeof(IPipelineBehavior<,>),
                   typeof(ValidationBehavior<,>));
            });

            services.AddAutoMapper(
                cfg => { },
                applicationAssembly);
            services.AddTransient<IOrderRepository, OrderRepository>();
            services.AddTransient<ICartRepository, CartRepository>();
            services.AddTransient<IProductServiceClient, ProductServiceClient>();
            return services;
        }
    }
}
