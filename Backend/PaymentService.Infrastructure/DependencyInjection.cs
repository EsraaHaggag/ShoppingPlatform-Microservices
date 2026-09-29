
using BuildingBlocks.Common;
using BuildingBlocks.Implementation;
using BuildingBlocks.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using PaymentService.Application.Bases;
using PaymentService.Application.Interfaces;
using PaymentService.Infrastructure.Repositories;

namespace PaymentService.Infrastructure
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

            services.AddTransient<IPaymentRepository, PaymentRepository>();
            services.AddTransient<ICurrentUserService, CurrentUserService>();
            services.AddTransient<IOutboxRepository, OutboxRepository>();
            return services;
        }
    }
}
