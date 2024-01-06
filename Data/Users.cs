using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bislerium.Data
{
    public class Users
    {
        public Guid ID { get; set; } = Guid.NewGuid();
        [Required(ErrorMessage = "Please provide a username.")]

        public string? Username { get; set; }
        [Required(ErrorMessage = "Please provide a password.")]
        public string? PasswordHash { get; set; }
        [Required(ErrorMessage = "Please provide a role.")]
        public Roles Role { get; set; }
        public bool HasInitialPassword { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public Guid CreatedBy { get; set; }
    }
}
