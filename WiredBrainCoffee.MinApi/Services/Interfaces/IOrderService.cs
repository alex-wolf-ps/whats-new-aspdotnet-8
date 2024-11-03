using WiredBrainCoffee.Models;

namespace WiredBrainCoffee.MinApi.Services.Interfaces
{
    public interface IOrderService
    {
        Order GetOrderById(int id);
        Task<List<Order>> GetOrders();
    }
}
