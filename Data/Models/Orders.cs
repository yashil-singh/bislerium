
namespace bislerium.Data
{
    public class Orders
    {
        public Guid OrderID { get; set; } = Guid.NewGuid();
        public List<OrderItems> Items { get; set; }
        public float OrderTotal { get; set; }
        public float Discount { get; set; }
        public string? MemberNumber { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public Guid HandledBy { get; set; }
        public int DiscountMonth { get; set; }
    }
    
}
