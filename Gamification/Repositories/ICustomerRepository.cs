using Gamification.Model;

namespace Gamification.Repositories
{
    public interface ICustomerRepository
    {
        public Task<Customer> AddCustomer(Customer customer);
        public Task DeleteCustomer(Customer customer);
        //public Task<Customer> EditCustomer(Customer customer);
        public Task<List<Customer>> GetAllCustomer();
        public Task<List<Customer>> GetCustomerByCity(string city);
    }
}
