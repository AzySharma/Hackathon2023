using Gamification.Repositories;
using Microsoft.Azure.Cosmos;
using System.Configuration;
using static Azure.Core.HttpHeader;
using ConfigurationManager = System.Configuration.ConfigurationManager;

namespace Gamification.Config
{
    public class DBContext : IDbContext
    {
        public string ConnectionString { get; set; }
        public string PrimaryKey { get; set; }

        public DBContext()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json").Build();
            var section = config.GetSection("ConnectionStrings");
            ConnectionString = section.Get<CosmosConfig>().Cosmos;
            PrimaryKey = section.Get<CosmosConfig>().PrimaryKey;
        }

        public CosmosClient GetCosmosClient()
        {
            return new CosmosClient(ConnectionString, PrimaryKey, new CosmosClientOptions() { ApplicationName = "Gamification" });
        }


    }
    class CosmosConfig
    {
        public string Cosmos { get; set; }
        public string PrimaryKey { get; set; }
    }
}
