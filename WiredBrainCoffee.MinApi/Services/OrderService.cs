using WiredBrainCoffee.MinApi.Services.Interfaces;
using WiredBrainCoffee.Models;

namespace WiredBrainCoffee.MinApi.Services
{
    public class OrderService : IOrderService
    {
        public List<Order> Orders { get; set; } = new List<Order>();

        public List<Order> GetOrders()
        {
            return GenerateOrders();
        }

        public Order GetOrderById(int id)
        {
            return Orders.FirstOrDefault(x => x.Id == id);
        }

        public Order AddOrder(Order order)
        {
            Orders.Add(order);

            return order;
        }

        public void UpdateOrder(int id, Order newOrder)
        {
            var order = Orders.FirstOrDefault(x => x.Id == id);

            order.Notes = newOrder.Notes;
            order.PromoCode = newOrder.PromoCode;
            order.OrderNumber = newOrder.OrderNumber;
        }

        public void DeleteOrder(int id)
        {
            var order = Orders.FirstOrDefault(x => x.Id == id);
            Orders.Remove(order);
        }

        private List<Order> GenerateOrders()
        {
            string[] names = ["Bob", "Alex", "Joe", "Jane", "Sarah", "Josh", "Ann", "Laura"];
            string[] lastNames = ["Test", "Sample", "Doe", "Example", "Testing"];
            string[] promoCodes = ["WiredFall123", "WiredCoffee", "dotnet9rocks", "Coffee123", "CoffeePromo"];
            string[] notes = ["Sample order notes", "Testing notes", "More notes", "Wired brain notes", "My notes"];
            var orders = new List<Order>();

            for (int i = 0; i < 100; i++)
            {
                var order = new Order()
                {
                    Id = i,
                    OrderNumber = new Random().Next(1, 10000),
                    Created = DateTime.Now.AddDays(new Random().Next(0, 100) * -1).AddHours(new Random().Next(0, 10) * -1),
                    FirstName = names[new Random().Next(0, names.Length)],
                    LastName = lastNames[new Random().Next(0, lastNames.Length)],
                    Notes = notes[new Random().Next(0, notes.Length)],
                    PromoCode = promoCodes[new Random().Next(0, promoCodes.Length)]
                };

                for (int y = 0; y < new Random().Next(1, 10); y++)
                {
                    order.Items.Add(new MenuItem());
                }

                orders.Add(order);
            }

            return orders;
        }
    }
}
