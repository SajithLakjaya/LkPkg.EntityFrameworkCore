using LkPkg.EntityFrameworkCore.Abstractions.Interfaces;
using LkPkg.EntityFrameworkCore.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace LkPkg.EntityFrameworkCore.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddUnitOfWork(this IServiceCollection services, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        {
            Guard.IsNotNull(services, nameof(services));

            switch (serviceLifetime)
            {
                case ServiceLifetime.Singleton:
                    {
                        services.TryAddSingleton<IUnitOfWork, UnitOfWork>();
                    }
                    break;
                case ServiceLifetime.Scoped:
                    {
                        services.TryAddScoped<IUnitOfWork, UnitOfWork>();
                    }
                    break;
                case ServiceLifetime.Transient:
                    {
                        services.TryAddTransient<IUnitOfWork, UnitOfWork>();
                    }
                    break;
                default:
                    break;
            }

            return services;
        }

    }
}
