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

        public async Task DeleteReward(string id)
        {
            await _rewardsContainer.DeleteItemAsync<Reward>(id.ToString(), new PartitionKey(id));
        }

        public async Task<Reward> EditReward(Reward reward)
        {
            var patchOperations = new List<PatchOperation>();
            PatchIfNotNull(patchOperations, "/code", reward.Code);
            PatchIfNotNull(patchOperations, "/name", reward.Name);
            PatchIfNotNull(patchOperations, "/description", reward.Description);
            PatchIfNotNull(patchOperations, "/pointRequired", reward.PointRequired);

            var result = await _rewardsContainer.PatchItemAsync<dynamic>(
                reward.id,
                new PartitionKey(reward.id),
                patchOperations
            );

            return await GetRewardById(reward.id);
        }

        private void PatchIfNotNull<TVal>(
            List<PatchOperation> patchOperations,
            string fieldName,
            TVal val)
        {
            if (val != null)
            {
                patchOperations.Add(PatchOperation.Set(fieldName, val));
            }
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
            using FeedIterator<Reward> feed = _rewardsContainer.GetItemQueryIterator<Reward>(
                    queryText: $"SELECT * FROM Rewards where Rewards.code = '{code}'"
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
            return rewards?.FirstOrDefault();
        }

        public async Task<Reward> GetRewardById(string id)
        {
            var query = $"SELECT * FROM Rewards where Rewards.id = '{id}'";

            using FeedIterator<Reward> feed = _rewardsContainer.GetItemQueryIterator<Reward>(
                    queryText: query
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
            return rewards?.FirstOrDefault();
        }
    }
}
