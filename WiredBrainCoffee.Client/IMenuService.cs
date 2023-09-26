using WiredBrainCoffee.Models;

namespace WiredBrainCoffee.Models.Services
{
    public interface IMenuService
    {
        Task<List<MenuItem>> GetMenuItems();

        List<MenuItem> GetPopularItems();
    }
}
