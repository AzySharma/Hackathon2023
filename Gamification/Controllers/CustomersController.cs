using Gamification.Model;
using Gamification.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Gamification.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomersController : Controller
    {
        private ICustomerRepository _customerRepository { get; set; }
        public CustomersController(ICustomerRepository customerRepository) {
            _customerRepository = customerRepository;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet(Name ="GetCustomers")]
        public async Task<List<Customer>> Get()
        {
            return await _customerRepository.GetAllCustomer();
        }

        [HttpPost(Name ="CreateCustomer")]
        public async Task<List<Customer>> CreateCustomer([FromBody] Customer customer)
        {
            await _customerRepository.AddCustomer(customer);
            return await _customerRepository.GetAllCustomer();
        }

        [HttpDelete(Name = "DeleteCustomer")]
        public async Task<List<Customer>> DeleteCustomer([FromBody] Customer customer)
        {
            await _customerRepository.DeleteCustomer(customer);
            return await _customerRepository.GetAllCustomer();
        }
    }
}
