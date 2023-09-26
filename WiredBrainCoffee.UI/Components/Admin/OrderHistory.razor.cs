using Microsoft.AspNetCore.Components;
using WiredBrainCoffee.Models;
using WiredBrainCoffee.Models.Services;

namespace WiredBrainCoffee.UI.Admin
{
    public partial class OrderHistory
    {
        [Inject]
        public IOrderService OrderService { get; set; }

        public List<Order> Items { get; set; } = new List<Order>();

        protected override async Task OnInitializedAsync()
        {
            await Task.Delay(3000);
            Items = await OrderService.GetOrders();
        }
    }
}
