using System;
using System.Collections.Generic;

namespace BillingAPI.Models
{
    public partial class Product
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public decimal? Rate { get; set; }
        public string ImagePath { get; set; }
        public string ShortDescription { get; set; }
        public string DetailedDescription { get; set; }
    }
}
