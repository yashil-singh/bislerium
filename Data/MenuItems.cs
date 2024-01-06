
using System.ComponentModel.DataAnnotations;

namespace bislerium.Data
{
    public class MenuItems
    {
        public Guid ID { get; set; } = Guid.NewGuid();
        [Required(ErrorMessage = "Please provide a name for the item.")]
        public string? Name { get; set; }
        [Required(ErrorMessage = "Please provide a price for the item.")]
        public float? Price { get; set; }
        public string? Description { get; set; }
        public Items ItemType { get; set; }
        public string? ImageURL { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public Guid CreatedBy { get; set; }
    }
}
