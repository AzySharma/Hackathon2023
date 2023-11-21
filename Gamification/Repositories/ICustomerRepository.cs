using Gamification.Model;

namespace Gamification.Repositories
{
    public interface ICustomerRepository
    {
        public Task<Customer> AddCustomer(Customer customer);
        public Task DeleteCustomer(string id);
        public Task<Customer> EditCustomer(Customer customer);
        public Task<List<Customer>> GetAllCustomer();
        public Task<Customer> GetCustomerById(string id);

        public Task<Customer> GetCustomerByName(string name);
    }
}
