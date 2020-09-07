using System;
using System.Collections.Generic;

namespace BillingAPI.Models
{
    public partial class OrderDetails
    {
        public long DetailsId { get; set; }
        public int? ProductId { get; set; }
        public decimal? Qty { get; set; }
        public decimal? Rate { get; set; }
        public long? OrderId { get; set; }

        public Orders Order { get; set; }
        public Product Product { get; set; }
    }
}
