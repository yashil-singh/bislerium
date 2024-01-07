
using System.ComponentModel.DataAnnotations;

namespace bislerium.Data
{
    public class Orders
    {
        public Guid OrderID { get; set; } = Guid.NewGuid();
        public Guid ItemID { get; set; }
        public string ItemName { get; set; }
        public int Quantity { get; set; } = 1;
        public float ItemUnitPrice { get; set; }
        public float ItemTotal { get; set; }
        public string? MemberNumber { get; set; }
        public DayOfWeek OrderDay { get; set; } = DateTime.Now.DayOfWeek;
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public Guid HandledBy { get; set; }
    }
    
}
