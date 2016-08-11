using BaalPratibha.DbPortal;
using Microsoft.Extensions.DependencyInjection;

namespace BaalPratibha.Extensions
{
    public static class DbPortalCollectionServiceExtension
    {
        public static IServiceCollection AddDbPortal(this IServiceCollection services)
        {

            services.AddScoped<IConnectionFactory, ConnectionFactory>();
            services.AddSingleton<ContestantDb>();
            services.AddSingleton<UserDb>();
            services.AddSingleton<VoteDb>();
            services.AddSingleton<ShareDb>();

            return services;
        }
    }
}
