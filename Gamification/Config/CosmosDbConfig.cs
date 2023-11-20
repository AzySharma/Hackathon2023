using System;

namespace Gamification.Config
{
    public class CosmosDbConfig
    {
        public const string ConfigSection = "cosmos";

        public string Endpoint { get; set; }
        public string Key { get; set; }
        public string DatabaseName { get; set; }
        public string RewardsContainer { get; set; }

        public string CustomersContainer { get; set; }

    }
}
