using WiredBrainCoffee.Models;

namespace WiredBrainCoffee.Models.Services
{
    public interface IOrderService
    {
        Task<List<Order>> GetOrders();
    }
}