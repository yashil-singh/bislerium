
using System.Text.Json;

namespace bislerium.Data
{
    public static class MenuItemService
    {
        // Save all menu items
        public static void SaveAll(List<MenuItems> menuItems)
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
        public static List<MenuItems> GetAll()
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
                Description = description,
                ImageURL = imageURL,
                ItemType = itemType,
                CreatedBy = userId
            });
            SaveAll(menuItems);
            return menuItems;
        }

        // To update a menu item
        public static List<MenuItems> Update(Guid itemId, string itemName, float itemPrice, string itemDescription, string imageURL)
        {
            List<MenuItems> menuItems = GetAll();
            MenuItems currentMenuItem = menuItems.FirstOrDefault(x => x.ID == itemId);

            if (currentMenuItem == null)
            {
                throw new Exception("Item not found.");
            }

            currentMenuItem.Name = itemName;
            currentMenuItem.Description = itemDescription;
            currentMenuItem.Price = itemPrice;
            currentMenuItem.ImageURL = imageURL;
            SaveAll(menuItems);
            return menuItems;
        }

        public static List<MenuItems> Delete(Guid itemId)
        {
            List<MenuItems> menuItems = GetAll();
            MenuItems menuItem = menuItems.FirstOrDefault(x => x.ID == itemId);

            if (menuItem == null)
            {
                throw new Exception("Item not found.");
            }

            menuItems.Remove(menuItem);
            SaveAll(menuItems);
            return menuItems;
        }

        public static MenuItems GetItemByID(Guid itemID)
        {
            List<MenuItems> menuItems = GetAll();
            return menuItems.FirstOrDefault(x => x.ID == itemID);
        }
    }
}
