using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace bislerium.Data
{
    public class OrderServices
    {
        public static void SaveAllOrders(List<Orders> orders)
        {
            string ordersFilePath = Utils.GetOrdersFilePath();

            var json = JsonSerializer.Serialize(orders);
            File.WriteAllText(ordersFilePath, json);
        }

        public static List<Orders> GetAllOrders()
        {
            string orderItemsFilePath = Utils.GetOrdersFilePath();
            if (!File.Exists(orderItemsFilePath))
            {
                return new List<Orders>();
            }

            var json = File.ReadAllText(orderItemsFilePath);
            return JsonSerializer.Deserialize<List<Orders>>(json);
        }
        public static List<Orders> AddItemToOrderList(List<MenuItems> menuItems, MenuItems orderData, int quantity)
        {
            List<Orders> orders = GetAllOrders();
            bool itemExists = orders.Any(x => x.OrderID == orderData.ID);
            if (itemExists)
            {
                throw new Exception("Item already added.");
            }

            orders.Add(
                new Orders()
                {
                    OrderID = new Guid(),
                    ItemName = orderData.Name,
                    Quantity = quantity,
                }
            );

            SaveAllOrders(orders);
            return orders;
        }

        public static List<MonthlyOrders> GetMonthlyOrdersByMemberNumber(string memberPhone)
        {
            string ordersFilePath = Utils.GetOrdersFilePath();

            if (!File.Exists(ordersFilePath))
            {
                return new List<MonthlyOrders>();
            }

            var json = File.ReadAllText(ordersFilePath);
            var orders = JsonSerializer.Deserialize<List<Orders>>(json);

            var memberOrderData = orders.Where(x => x.MemberNumber == memberPhone && x.OrderDay != DayOfWeek.Sunday && x.OrderDay != DayOfWeek.Saturday && x.OrderDate.Month == DateTime.Now.Month).ToList();
            var orderCountCurrentMonth = memberOrderData.Count();
            var monthlyOrderData = new List<MonthlyOrders>();

            foreach (var order in memberOrderData)
            {
                monthlyOrderData.Add( new MonthlyOrders()
                    {
                        MemberNumber = order.MemberNumber,
                        OrderMonth = order.OrderDate.Month,
                        OderCount = orderCountCurrentMonth,
                    }
                );
            }

            return monthlyOrderData;
        }
    }
}
