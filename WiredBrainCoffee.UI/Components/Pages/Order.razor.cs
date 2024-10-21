using Microsoft.AspNetCore.Components;
using WiredBrainCoffee.Models;
using WiredBrainCoffee.UI.Services;

namespace WiredBrainCoffee.UI.Components.Pages
{
    public partial class Order
    {
        [Inject]
        public IMenuService MenuService { get; set; }

        [Inject]
        public NavigationManager NavManager { get; set; }

        public List<MenuItem> CurrentOrder { get; set; } = new List<MenuItem>();
        public List<MenuItem> MenuItems { get; set; } = new List<MenuItem>();
        public decimal OrderTotal { get; set; } = 0;
        public decimal SalesTax { get; set; } = 0.06m;
        public string PromoCode { get; set; } = string.Empty;
        public decimal Discount { get; set; } = 0;

        public string SearchTerm { get; set; } = string.Empty;

        public List<MenuItem> FilteredMenu = new List<MenuItem>();

        protected override async Task OnInitializedAsync()
        {
            MenuItems = await MenuService.GetMenuItems();
        }

        public void FilterMenu()
        {
            if (!string.IsNullOrWhiteSpace(SearchTerm))
            {
                FilteredMenu = MenuItems
                .Where(x => x.Name.ToLower().Contains(SearchTerm.ToLower())).ToList();
            } 
            else
            {
                FilteredMenu = new();
            }
        }

        private void AddToOrder(MenuItem item)
        {
            CurrentOrder.Add(new MenuItem()
            {
                Name = item.Name,
                Id = item.Id,
                Price = item.Price,
            });

            OrderTotal += item.Price;
        }

        private void RemoveFromOrder(MenuItem item)
        {
            CurrentOrder.Remove(item);
            OrderTotal -= item.Price;
        }

        private void PlaceOrder()
        {
            NavManager.NavigateTo("order-confirmation");
        }
    }
}
