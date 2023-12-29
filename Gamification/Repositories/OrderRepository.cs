using Gamification.Config;
using Gamification.Model;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;
using Container = Microsoft.Azure.Cosmos.Container;

namespace Gamification.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly Container _ordercontainer;
        private readonly IRewardRepository _rewardRepository;

        public OrderRepository(IOptions<CosmosDbConfig> config, CosmosClient client, IRewardRepository rewardRepository)
        {
            _ordercontainer = client.GetContainer(config.Value.DatabaseName, config.Value.OrdersContainer);
            _rewardRepository = rewardRepository;
        }

        public async Task<Order> AddOrder(Order order)
        {
            if (!string.IsNullOrEmpty(order.RewardCode))
            {
                var reward = await _rewardRepository.GetRewardByCode(order.RewardCode);
                if(reward != null)
                {
                    if(reward.MinOrderAmount <= order.TotalAmount)
                    {
                        order.PaybleAmount = order.TotalAmount - order.TotalAmount * (reward.DiscountInPercent / 100) ;
                        order.Discount = order.TotalAmount * (reward.DiscountInPercent / 100) ;
                    }
                }
            }
            return await _ordercontainer.CreateItemAsync<Order>(order, new PartitionKey(order.Id));
        }

        public async Task DeleteOrder(string id)
        {
            await _ordercontainer.DeleteItemAsync<Order>(id.ToString(), new PartitionKey(id));            
        }

        public async Task<List<Order>> GetAllOrders()
        {
            using FeedIterator<Order> feed = _ordercontainer.GetItemQueryIterator<Order>(
                    queryText: "SELECT * FROM orders"
            );

            var orders = new List<Order>();
            while (feed.HasMoreResults)
            {
                FeedResponse<Order> response = await feed.ReadNextAsync();

                // Iterate query results
                foreach (Order order in response)
                {
                    orders.Add(order);
                    Console.WriteLine($"Found order:\t{order.Id}");
                }
            }
            return orders;
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

        public async Task<Order> EditOrder(Order order)
        {
            var patchOperations = new List<PatchOperation>();            
            //PatchIfNotNull(patchOperations, "/name", order.Name);
            //PatchIfNotNull(patchOperations, "/email", order.Email);
            //PatchIfNotNull(patchOperations, "/address", order.Address);
            //PatchIfNotNull(patchOperations, "/phonenumber", order.PhoneNumber);

            var result = await _ordercontainer.PatchItemAsync<dynamic>(
                order.Id,
                new PartitionKey(order.Id),
                patchOperations
            );

            return await GetOrderById(order.Id);
        }

        public async Task<Order> GetOrderById(string id)
        {
            var query = $"SELECT * FROM orders where orders.id = '{id}'";

            using FeedIterator<Order> feed = _ordercontainer.GetItemQueryIterator<Order>(
                    queryText: query
            );

            var orders = new List<Order>();
            while (feed.HasMoreResults)
            {
                FeedResponse<Order> response = await feed.ReadNextAsync();

                // Iterate query results
                foreach (Order item in response)
                {
                    orders.Add(item);
                    Console.WriteLine($"Found item:\t{item.Id}");
                }
            }
            return orders?.FirstOrDefault();
        }

        public async Task<List<Order>> GetOrderByCustomerId(string id)
        {
            var query = $"SELECT * FROM orders where orders.customerId = '{id}'";

            using FeedIterator<Order> feed = _ordercontainer.GetItemQueryIterator<Order>(
                    queryText: query
            );

            var orders = new List<Order>();
            while (feed.HasMoreResults)
            {
                FeedResponse<Order> response = await feed.ReadNextAsync();

                // Iterate query results
                foreach (Order item in response)
                {
                    orders.Add(item);
                    Console.WriteLine($"Found item:\t{item.Id}");
                }
            }
            return orders;
        }

        public async Task<List<Order>> GetOrderByRewardCode(string code)
        {
            var query = $"SELECT * FROM orders where orders.rewardCode = '{code}'";

            using FeedIterator<Order> feed = _ordercontainer.GetItemQueryIterator<Order>(
                    queryText: query
            );

            var orders = new List<Order>();
            while (feed.HasMoreResults)
            {
                FeedResponse<Order> response = await feed.ReadNextAsync();

                // Iterate query results
                foreach (Order item in response)
                {
                    orders.Add(item);
                    Console.WriteLine($"Found item:\t{item.Id}");
                }
            }
            return orders;
        }
    }
}
