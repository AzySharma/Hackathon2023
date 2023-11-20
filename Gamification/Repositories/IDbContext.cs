using Microsoft.AspNetCore.SignalR;
using Microsoft.Azure.Cosmos;

namespace Gamification.Repositories
{
    public interface IDbContext
    {
        public CosmosClient GetCosmosClient();
    }
}
