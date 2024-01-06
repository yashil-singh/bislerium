
using System.Text.Json;

namespace bislerium.Data
{
    public static class MenuItemServices
    {
        // Save all menu items
        private static void SaveAll(Guid itemId, List<MenuItems> menuItems)
        {
            string appDataDirectoryPath = Utils.GetAppDirectoryPath();
            string menuItemFilePath = Utils.GetItemsFilePath();

            if (!Directory.Exists(appDataDirectoryPath))
            {
                Directory.CreateDirectory(appDataDirectoryPath);
            }

            var json = JsonSerializer.Serialize(menuItems);
            File.WriteAllText(menuItemFilePath, json);
        }

        // Get all menu items
        private static List<MenuItems> GetAll()
        {
            string menuItemFilePath = Utils.GetItemsFilePath();
            if (!File.Exists(menuItemFilePath))
            {
                return new List<MenuItems>();
            }

            var json = File.ReadAllText(menuItemFilePath);

            return JsonSerializer.Deserialize<List<MenuItems>>(json);
        }

        // Add a item to the menu
        public static List<MenuItems> Create(Guid userId, string itemName, float itemPrice, string description, string imageURL, Items itemType)
        {
            if (itemPrice < 0)
            {
                throw new Exception("Price must not be less than Rs. 0.");
            }

            List<MenuItems> menuItems = GetAll();
            menuItems.Add(new MenuItems
            {
                Name = itemName,
                Price = itemPrice,
                CreatedBy = userId
            });
            SaveAll(userId, todos);
            return todos;
        }
    }
}
