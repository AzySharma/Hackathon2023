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

        [HttpGet]
        public async Task<List<Customer>> Get()
        {
            return await _customerRepository.GetAllCustomer();
        }

        [HttpPost]
        public async Task<List<Customer>> CreateCustomer([FromBody] Customer customer)
        {
            await _customerRepository.AddCustomer(customer);
            return await _customerRepository.GetAllCustomer();
        }

        [HttpDelete("{id}")]
        public async Task DeleteCustomer(string id)
        {
            await _customerRepository.DeleteCustomer(id);            
        }

        [HttpPut]
        public async Task<Customer> UpdateReward([FromBody] Customer customer)
        {
            return await _customerRepository.EditCustomer(customer);
        }


        [HttpGet]
        [Route("id/{id}")]
        public async Task<Customer> GetById(string id)
        {
            return await _customerRepository.GetCustomerById(id);
        }


        [HttpGet]
        [Route("name/{name}")]
        public async Task<Customer> GetCustomerByName(string name)
        {
            return await _customerRepository.GetCustomerByName(name);
        }
    }
}
