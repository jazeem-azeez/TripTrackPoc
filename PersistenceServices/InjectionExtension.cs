
using Microsoft.Extensions.DependencyInjection;

using PersistenceServices.Implementataions;
using PersistenceServices.Interfaces;

namespace PersistenceServices
{
    public static class InjectionExtension
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(InjectionExtension).Assembly);

            services.AddSingleton(typeof(IDataStoreRepository<,>), typeof(DataStoreRepository<,>));
            return services;
        }

    }
}