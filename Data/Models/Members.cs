

namespace bislerium.Data
{
    public class Members
    {
        public string MemberNumber { get; set; }
        public string MemberName { get; set; }
        public MemberType MemberType { get; set; } = MemberType.General;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public Guid CreatedBy { get; set; }
    }
}
