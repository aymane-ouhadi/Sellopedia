using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Sellopedia.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sellopedia.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [StringLength(300)]
        public string Description { get; set; }

        public decimal OriginalPrice { get; set; }
        public decimal DiscountPrice { get; set; }

        [Range(0, 1000)]
        public int Quantity { get; set; }

        //-------------- Navigation Properties
        //--- Product Owner
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        //--- Product Category
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public virtual Product Category { get; set; }

        //--- Product Buyers
        public virtual ICollection<Order> Orders { get; set; }

        //--- Product Reviews
        public virtual ICollection<Review> Reviews { get; set; }

        //--- Product Images
        public virtual ICollection<ProductImage> ProductImages { get; set; }

        public Product()
        {
            DiscountPrice = 0;
            Quantity = 1;
        }
    }
}