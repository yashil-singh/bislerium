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
        public static List<Orders> AddItemToOrderList(List<Orders> orderItems, MenuItems currentItem, int quantity, Guid staffID)
        {
            orderItems = orderItems ?? new List<Orders>();
            bool itemExists = orderItems.Any(x => x.ItemID == currentItem.ID);
            if (itemExists)
            {
                throw new Exception("Item has already been added.");
            }

            orderItems.Add(new Orders()
            {
                ItemID = currentItem.ID,
                ItemName = currentItem.Name,
                ItemUnitPrice = (float)currentItem.Price,
                Quantity = quantity,
                ItemTotal = quantity * (float)currentItem.Price,
                HandledBy = staffID
            });

            return orderItems;
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
