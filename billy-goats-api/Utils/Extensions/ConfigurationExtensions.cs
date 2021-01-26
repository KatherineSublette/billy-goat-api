using System;
using Microsoft.Extensions.Configuration;

namespace BillyGoats.Api.Utils.Extensions
{
    public static class ConfigurationExtensions
    {
        public static string GetDbConnectionString(this IConfiguration config, string dbName, bool keepAlive = false) 
        {
            var connString = $"Host={config[dbName + ":Host"]};" +
                $"Username=postgres;" +
                $"Password={config[dbName + ":Password"]};" +
                $"Database={config[dbName + ":Database"]}";
            if (keepAlive)
                connString += ";Keepalive=1";
            return connString;
        }
    }
}
