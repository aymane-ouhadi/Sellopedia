using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sellopedia.Models
{
    public class Upload
    {
        public Product Product { get; set; }
        public List<ProductImage> Images { get; set; }
    }
}