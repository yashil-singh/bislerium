
using System.Text.Json;

namespace bislerium.Data
{
    public class OrderService
    {
        // Save all orders to json file
        public static void SaveAllOrders(List<Orders> orders)
        {
            string ordersFilePath = Utils.GetOrdersFilePath();

            var json = JsonSerializer.Serialize(orders);
            File.WriteAllText(ordersFilePath, json);
        }

        // Get all orders from json file
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

        // Add items to order items list
        public static List<OrderItems> AddItemToOrderList(List<OrderItems> orderItems, MenuItems currentItem, int quantity, Guid staffID)
        {
            orderItems = orderItems ?? new List<OrderItems>();
            bool itemExists = orderItems.Any(x => x.ItemID == currentItem.ID);
            if (itemExists)
            {
                throw new Exception("Item has already been added.");
            }

            orderItems.Add(new OrderItems()
            {
                ItemID = currentItem.ID,
                Quantity = quantity,
                UnitPrice = (float)currentItem.Price,
                ItemTotal = quantity * (float)currentItem.Price,
            });

            return orderItems;
        }

        public static float CalculateGrandTotal(List<OrderItems> orderItemsList, float discount)
        {
            float orderTotal = orderItemsList.Sum(item => item.ItemTotal);

            if (discount > 0)
            {
                orderTotal = orderTotal - (orderTotal * discount / 100);
            }

            return orderTotal;
        }

        public static void CreateOrder(List<OrderItems> orderItemsList, float discount, string memberNumber, Guid userID)
        {
            List<Orders> orders = GetAllOrders();

            var orderTotal = CalculateGrandTotal(orderItemsList, discount);

            orders.Add(new Orders()
            {
                Items = orderItemsList,
                OrderTotal = orderTotal,
                Discount = discount,
                MemberNumber = memberNumber,
                HandledBy = userID

            });

            SaveAllOrders(orders);
        }

        public static int GetUniqueOrderCountByMemberNumber(string memberPhone)
        {
            List<Orders> orders = GetAllOrders();

            DateTime currentDate = DateTime.Now;
            DateTime monthAgoDate = currentDate.AddMonths(-1);

            int monthlyOrderCount = 0;

            // Find member's orders for each day excluding weekends
            var uniqueMemberRecords = orders
            .Where(x => x.MemberNumber == memberPhone && x.OrderDate.DayOfWeek != DayOfWeek.Saturday && x.OrderDate.DayOfWeek != DayOfWeek.Sunday)
            .GroupBy(x => x.OrderDate)
            .Select(group => group.First())
            .ToList();

            foreach (var order in uniqueMemberRecords)
            {
                if(order.OrderDate >= monthAgoDate)
                {
                    monthlyOrderCount++;
                }
            }
            return monthlyOrderCount;
        }

        public static int GetOrderCountByMemberNumber(string memberPhone)
        {
            List<Orders> orders = GetAllOrders();

            // Find member's orders for each day excluding weekends
            var memberRecords = orders
            .Where(x => x.MemberNumber == memberPhone && x.OrderDate.DayOfWeek != DayOfWeek.Saturday && x.OrderDate.DayOfWeek != DayOfWeek.Sunday)
            .ToList();

            return memberRecords.Count;
        }

        public static bool CheckRedeemAvailability(string memberNumber)
        {

            int orderCount = GetOrderCountByMemberNumber(memberNumber);

            if (orderCount != 0 && orderCount % 10 == 0)
            {
                return true;
            }

            return false;
        }

        public static bool CheckDiscountAvailability(string memberNumber)
        {
            if (MemberService.CheckMemberType(memberNumber) == MemberType.Regular)
            {
                return true;
            }

            DateTime currentDate = DateTime.Now;
            DateTime monthAgoDate = currentDate.AddMonths(-1);
            int bussinessDays = Utils.GetBusinessDays(monthAgoDate);
            int orderCount = GetUniqueOrderCountByMemberNumber(memberNumber);

            if (orderCount >= bussinessDays )
            {
                MemberService.ConvertMemberType(memberNumber);
                return true;
            }

            return false;
        }

        public static List<Orders> GetOrdersByDate(DateTime selectedDate)
        {
            List<Orders> orders = GetAllOrders();
            return orders.Where(o => o.OrderDate.Date == selectedDate.Date).ToList();
        }

        public static List<Orders> GetOrdersByMonth(DateTime selectedDate)
        {
            List<Orders> orders = GetAllOrders();
            return orders.Where(o => o.OrderDate.Month == selectedDate.Month && o.OrderDate.Year == selectedDate.Year).ToList();
        }
    }
}
