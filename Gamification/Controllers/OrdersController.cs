using Gamification.Model;
using Gamification.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Gamification.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrdersController : Controller
    {
        public List<Order> Orders { get; set; }
        private IOrderRepository _orderRepository { get; set; }

        public OrdersController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public IActionResult Index()
        {

            return View();
        }

        [HttpGet]
        public async Task<List<Order>> Get()
        {
            return await _orderRepository.GetAllOrders(); 
        }

        [HttpGet]
        [Route("id/{id}")]
        public async Task<Order> GetById(string id)
        {
            return await _orderRepository.GetOrderById(id);
        }


        [HttpGet]
        [Route("customer/{id}")]
        public async Task<List<Order>> GetByCustomerId(string id)
        {
            return await _orderRepository.GetOrderByCustomerId(id);
        }

        [HttpGet]
        [Route("reward/{code}")]
        public async Task<List<Order>> GetByRewardCode(string code)
        {
            return await _orderRepository.GetOrderByRewardCode(code);
        }


        [HttpPost]
        public async Task<List<Order>> Post([FromBody] Order order)
        {
            await _orderRepository.AddOrder(order);
            return await _orderRepository.GetAllOrders(); 
        }

        [HttpPut]
        public async Task<Order> UpdateOrder([FromBody] Order order)
        {
            return await _orderRepository.EditOrder(order);
             
        }

        [HttpDelete("{id}")]
        public async Task DeleteOrder(string id)
        {
            await _orderRepository.DeleteOrder(id);
        }
    }
}
