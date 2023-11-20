using Gamification.Config;
using Gamification.Model;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.Azure.Cosmos.Serialization.HybridRow.RecordIO;
using Microsoft.Extensions.Options;
using System.ComponentModel;
using Container = Microsoft.Azure.Cosmos.Container;

namespace Gamification.Repositories
{
    public class RewardRepository : IRewardRepository
    {

        private readonly Container _rewardsContainer;

        public RewardRepository(IOptions<CosmosDbConfig> config, CosmosClient client)
        {
            _rewardsContainer = client.GetContainer(config.Value.DatabaseName, config.Value.RewardsContainer);
        }


        public async Task<Reward> AddReward(Reward reward)
        {
            return await _rewardsContainer.CreateItemAsync<Reward>(reward, new PartitionKey(reward.id));
        }

        public async Task DeleteReward(Reward reward)
        {
            await _rewardsContainer.DeleteItemAsync<Reward>(reward.id.ToString(), new PartitionKey(reward.Name));
        }

        public async Task<Reward> EditReward(Reward reward)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Reward>> GetAllReward()
        {
            using FeedIterator<Reward> feed = _rewardsContainer.GetItemQueryIterator<Reward>(
                    queryText: "SELECT * FROM Rewards"
            );

            var rewards = new List<Reward>();
            while (feed.HasMoreResults)
            {
                FeedResponse<Reward> response = await feed.ReadNextAsync();

                // Iterate query results
                foreach (Reward item in response)
                {
                    rewards.Add(item);
                    Console.WriteLine($"Found item:\t{item.Name}");
                }
            }
            return rewards;
        }

        public async Task<Reward> GetRewardByCode(string code)
        {
            return await _rewardsContainer.ReadItemAsync<Reward>(code, new PartitionKey(code));
        }
    }
}
