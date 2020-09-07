using System;
using System.Collections.Generic;

namespace BillingAPI.Models
{
    public partial class User
    {
        public User()
        {
            Customer = new HashSet<Customer>();
            Orders = new HashSet<Orders>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public bool? IsAdmin { get; set; }

        public ICollection<Customer> Customer { get; set; }
        public ICollection<Orders> Orders { get; set; }
    }
}
