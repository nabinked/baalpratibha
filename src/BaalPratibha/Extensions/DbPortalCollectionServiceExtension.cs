using BaalPratibha.DbPortal;
using Microsoft.Extensions.DependencyInjection;

namespace BaalPratibha.Extensions
{
    public static class DbPortalCollectionServiceExtension
    {
        public static IServiceCollection AddDbPortal(this IServiceCollection services)
        {

            services.AddScoped<IConnectionFactory, ConnectionFactory>();
            services.AddScoped<ContestantDb>();
            services.AddScoped<UserDb>();
            services.AddScoped<VoteDb>();
            services.AddScoped<ShareDb>();

            return services;
        }
    }
}
