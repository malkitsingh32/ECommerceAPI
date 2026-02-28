using Application.Abstraction.DataBase;
using Microsoft.Extensions.DependencyInjection;
using Persistence.DataBase;

namespace Persistence
{
    public static class DependencyInjection
    {
        public static void AddPersistence(this IServiceCollection services)
        {
            services.AddScoped<IDbContext, DbContext>();
            services.AddScoped<IParameterManager, ParameterManager>();
        }
    }
}
