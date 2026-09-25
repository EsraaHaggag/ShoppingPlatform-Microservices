using BuildingBlocks.Common;
using BuildingBlocks.Interfaces;
using BuildingBlocks.Repositories;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using ProductService.Application.Bases;
using ProductService.Application.Interfaces;
using ProductService.Infrastructure.Repositories;

namespace ProductService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDependencyInjection(
        this IServiceCollection services)
        {
            var applicationAssembly =
                typeof(ApplicationAssemblyMarker).Assembly;

            // FluentValidation
            services.AddValidatorsFromAssembly(applicationAssembly);

            // MediatR
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(applicationAssembly);

                cfg.AddBehavior(
                   typeof(IPipelineBehavior<,>),
                   typeof(ValidationBehavior<,>));
            });

            // AutoMapper
            services.AddAutoMapper(
                cfg => { },
                applicationAssembly);

            // Generic Repository
            services.AddTransient(
                typeof(IGenericRepositoryAsync<>),
                typeof(GenericRepositoryAsync<>));

            // Product Repository
            services.AddTransient<
                IProductRepository,
                ProductRepository>();

            services.AddScoped<
                IProcessedEventRepository,
                ProcessedEventRepository>();

            return services;
        }
    }
}

