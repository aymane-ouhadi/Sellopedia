using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sellopedia.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime OrderDate { get; set; }

        public ICollection<Order> Orders { get; set; }
    }
}