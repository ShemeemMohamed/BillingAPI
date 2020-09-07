using System;
using System.Collections.Generic;

namespace BillingAPI.Models
{
    public partial class Customer
    {
        public Customer()
        {
            Orders = new HashSet<Orders>();
        }

        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public int? UserId { get; set; }
        public DateTime? CreatedDate { get; set; }

        public User User { get; set; }
        public ICollection<Orders> Orders { get; set; }
    }
}
