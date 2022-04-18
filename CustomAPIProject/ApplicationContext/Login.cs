using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CustomAPIProject.ApplicationContext
{
    public class Login
    {
        [Key]
        public int LoginId { get; set; }

        [Required]
        [MaxLength(50)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [Required]
        public int RoleId { get; set; }

        [ForeignKey(nameof(RoleId))]
        public virtual Roles roles { get; set; }

        [ForeignKey(nameof(CustomerId))]

        public virtual Customer customer { get; set; }
    }
}
