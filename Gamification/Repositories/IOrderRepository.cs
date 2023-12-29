using Gamification.Model;

namespace Gamification.Repositories
{
    public interface IOrderRepository
    {
        public Task<Order> AddOrder(Order order);
        public Task DeleteOrder(string id);
        public Task<Order> EditOrder(Order order);
        public Task<List<Order>> GetAllOrders();
        public Task<Order> GetOrderById(string id);
        public Task<List<Order>> GetOrderByCustomerId(string id);
        public Task<List<Order>> GetOrderByRewardCode(string id);
    }
}
