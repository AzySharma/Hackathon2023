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
            return await _customercontainer.CreateItemAsync<Customer>(customer, new PartitionKey(customer.id));
        }

        public async Task DeleteCustomer(string id)
        {
            await _customercontainer.DeleteItemAsync<Customer>(id.ToString(), new PartitionKey(id));            
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

        public async Task<Customer> EditCustomer(Customer customer)
        {
            var patchOperations = new List<PatchOperation>();            
            PatchIfNotNull(patchOperations, "/name", customer.Name);
            PatchIfNotNull(patchOperations, "/email", customer.Email);
            PatchIfNotNull(patchOperations, "/address", customer.Address);
            PatchIfNotNull(patchOperations, "/phonenumber", customer.PhoneNumber);

            var result = await _customercontainer.PatchItemAsync<dynamic>(
                customer.id,
                new PartitionKey(customer.id),
                patchOperations
            );

            return await GetCustomerById(customer.id);
        }

        public async Task<Customer> GetCustomerById(string id)
        {
            var query = $"SELECT * FROM Customers where Customers.id = '{id}'";

            using FeedIterator<Customer> feed = _customercontainer.GetItemQueryIterator<Customer>(
                    queryText: query
            );

            var customers = new List<Customer>();
            while (feed.HasMoreResults)
            {
                FeedResponse<Customer> response = await feed.ReadNextAsync();

                // Iterate query results
                foreach (Customer item in response)
                {
                    customers.Add(item);
                    Console.WriteLine($"Found item:\t{item.Name}");
                }
            }
            return customers?.FirstOrDefault();
        }

        public async Task<Customer> GetCustomerByName(string name)
        {
            var query = $"SELECT * FROM Customers where Customers.name = '{name}'";

            using FeedIterator<Customer> feed = _customercontainer.GetItemQueryIterator<Customer>(
                    queryText: query
            );

            var customers = new List<Customer>();
            while (feed.HasMoreResults)
            {
                FeedResponse<Customer> response = await feed.ReadNextAsync();

                // Iterate query results
                foreach (Customer item in response)
                {
                    customers.Add(item);
                    Console.WriteLine($"Found item:\t{item.Name}");
                }
            }
            return customers?.FirstOrDefault();
        }
    }
}
