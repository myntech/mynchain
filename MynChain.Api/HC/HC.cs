using Microsoft.Extensions.HealthChecks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MynChain.Data;
using System.Threading;
using Microsoft.Extensions.Configuration;
using System.IO;
using MongoDB.Driver;

/*DEVELOPED BY MYNTECH https://myntech.it*/
/*AUTHOR: MARCO AMATO*/

namespace MynChain.Api.HC
{
    public class HC : IHealthCheck
    {
        public string Name => "HC";

        public ValueTask<IHealthCheckResult> CheckAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            bool _isDbRunning = CheckDbRunning();
            CheckStatus status = _isDbRunning == true ? CheckStatus.Healthy : CheckStatus.Unhealthy;
            return new ValueTask<IHealthCheckResult>(HealthCheckResult.FromStatus(status, ""));
        }

        private bool CheckDbRunning()
        {
            try
            {
                var builder = new ConfigurationBuilder();
                builder.SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false);
                var configuration = builder.Build();

                var settings = new MongoClientSettings
                {
                    Server = new MongoServerAddress(configuration.GetValue<string>("ConnectionStrings:MongoDb:Host"), configuration.GetValue<int>("ConnectionStrings:MongoDb:Port")),
                    UseSsl = configuration.GetValue<bool>("ConnectionStrings:MongoDb:UseSSL")
                };

                MongoClient _client = new MongoClient(settings);
                IMongoDatabase db = _client.GetDatabase(configuration.GetValue<string>("ConnectionStrings:MongoDb:DbName"));
                
                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }
    }
}
