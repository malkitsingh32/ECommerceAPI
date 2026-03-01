using Application.Abstraction.Repositories;
using Application.Abstraction.Services;
using Infrastructure.Implementation.Repositories;
using Infrastructure.Implementation.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static void AddInfrastructure(this IServiceCollection services, bool useDistributedCache = false)
        {
            #region Service
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IPasswordHasher, PasswordHasher>();
            
            // Configure token caching strategy
            if (useDistributedCache)
            {
                // Use distributed cache (Redis) for multi-server deployments
                services.AddScoped<ITokenCache, DistributedTokenCache>();
            }
            else
            {
                // Use in-memory cache for single-server deployments (default)
                services.AddSingleton<ITokenCache, InMemoryTokenCache>();
            }
            #endregion

            #region Repository
            services.AddTransient<IUserRepository, UserRepository>();
            #endregion
        }

    }
}
