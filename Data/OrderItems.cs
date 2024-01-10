
namespace bislerium.Data
{
    public class OrderItems
    {
        public Guid ItemID { get; set; }    
        public int Quantity { get; set; }
        public float UnitPrice { get; set; }
        public float ItemTotal { get; set; }
    }
}
