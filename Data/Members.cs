using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bislerium.Data
{
    public class Members
    {
        public string MemberNumber { get; set; }
        public string MemberName { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public Guid CreatedBy { get; set;}
    }
}
