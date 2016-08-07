using System.Data;
using System.Data.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using Npgsql;
using static Microsoft.AspNetCore.Hosting.EnvironmentName;

namespace BaalPratibha.DbPortal
{
    public class ConnectionFactory : IConnectionFactory
    {
        private readonly string _connectionString;

        public ConnectionFactory(IOptions<AppSettings> appOptions, IHostingEnvironment environment)
        {
            _connectionString = EnvironmentName == Development ? appOptions.Value.ConnectionStringDev : appOptions.Value.ConnectionStringProd;
            EnvironmentName = environment.EnvironmentName;
        }

        public string EnvironmentName { get; set; }

        public IDbConnection CreateConnection()
        {
            return new NpgsqlConnection(_connectionString);
        }
    }
}
