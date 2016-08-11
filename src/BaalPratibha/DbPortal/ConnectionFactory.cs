using System;
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
            _connectionString = environment.EnvironmentName == Development ? appOptions.Value.ConnectionString : GetProductionConString(appOptions.Value.ConnectionString);

        }

        private string GetProductionConString(string connectionStringProd)
        {
            var uri = new Uri(connectionStringProd);
            var db = uri.AbsolutePath.Trim('/');
            var user = uri.UserInfo.Split(':')[0];
            var passwd = uri.UserInfo.Split(':')[1];
            var port = uri.Port > 0 ? uri.Port : 5432;
            var connStr = $"Server={uri.Host};Database={db};User Id={user};Password={passwd};Port={port}";
            return connStr;
        }


        public IDbConnection CreateConnection()
        {
            return new NpgsqlConnection(_connectionString);
        }
    }
}
