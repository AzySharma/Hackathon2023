using Gamification.Config;
using Gamification.Model;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;
using Container = Microsoft.Azure.Cosmos.Container;

namespace Gamification.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly Container _customercontainer;

        public CustomerRepository(IOptions<CosmosDbConfig> config, CosmosClient client)
        {
            _customercontainer = client.GetContainer(config.Value.DatabaseName, config.Value.CustomersContainer);
        }

        public async Task<Customer> AddCustomer(Customer customer)
        {
            return await _customercontainer.CreateItemAsync<Customer>(customer, new PartitionKey(customer.CustomerId));
        }

        public async Task DeleteCustomer(Customer customer)
        {
            await _customercontainer.DeleteItemAsync<Customer>(customer.CustomerId.ToString(), new PartitionKey(customer.Name));
        }

        public async Task<List<Customer>> GetAllCustomer()
        {
            using FeedIterator<Customer> feed = _customercontainer.GetItemQueryIterator<Customer>(
                    queryText: "SELECT * FROM Customers"
            );

            var customers = new List<Customer>();
            while (feed.HasMoreResults)
            {
                FeedResponse<Customer> response = await feed.ReadNextAsync();

                // Iterate query results
                foreach (Customer customer in response)
                {
                    customers.Add(customer);
                    Console.WriteLine($"Found customer:\t{customer.Name}");
                }
            }
            return customers;
        }

        public async Task<List<Customer>> GetCustomerByCity(string city)
        {
            using FeedIterator<Customer> feed = _customercontainer.GetItemQueryIterator<Customer>(
                    queryText: "SELECT * FROM Customers"
            );

            var customers = new List<Customer>();
            while (feed.HasMoreResults)
            {
                FeedResponse<Customer> response = await feed.ReadNextAsync();

                // Iterate query results
                foreach (Customer customer in response)
                {
                    customers.Add(customer);
                    Console.WriteLine($"Found customer:\t{customer.Name}");
                }
            }
            return customers;
        }
    }
}
