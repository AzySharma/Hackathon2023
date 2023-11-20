using Gamification.Config;
using Gamification.Repositories;
using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.Extensions.Options;

namespace Gamification.IoC
{
    public static class CosmosRegistrator
    {
        public static IServiceCollection RegisterCosmosClient(this IServiceCollection services)
        {
            services.AddSingleton(provider =>
            {
                var config = provider.GetRequiredService<IOptions<CosmosDbConfig>>().Value;

                return new CosmosClientBuilder(config.Endpoint, config.Key)
                    .WithCustomSerializer(new CosmosSerialiser())
                    .WithConnectionModeDirect(TimeSpan.FromMinutes(20))
                    .WithBulkExecution(true)
                    .Build();
            });
            
            return services;
        }
    }
}
