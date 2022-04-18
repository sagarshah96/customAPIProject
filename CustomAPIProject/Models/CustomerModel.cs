using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomAPIProject.Models
{
    public class CustomerModel
    {
        public int CustomerId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string MiddleName { get; set; }
        public string MobileNo { get; set; }

        public string Email { get; set; }
        public string Password { get; set; }

    }
}
