using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.QuickGrid;
using WiredBrainCoffee.Models;
using WiredBrainCoffee.Models.Services;

namespace WiredBrainCoffee.UI.Admin
{
    public partial class OrderHistory
    {
        [Inject]
        public IOrderService OrderService { get; set; }

        public IQueryable<Order> Orders { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Orders = (await OrderService.GetOrders()).AsQueryable();
        }
    }
}
