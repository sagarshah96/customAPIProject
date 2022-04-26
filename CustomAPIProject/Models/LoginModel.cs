using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CustomAPIProject.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Please enter email.")]
        [RegularExpression(@"[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}", ErrorMessage = "Please enter correct email.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Please enter password.")]
        public string Password { get; set; }
    }
}
