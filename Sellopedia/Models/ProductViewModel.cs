using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sellopedia.Models
{
    public class ProductViewModel
    {
        [Required(ErrorMessageResourceName = "NameRequired", ErrorMessageResourceType = typeof(Resources.ResProduct))]
        [Display(Name = "Name", ResourceType = typeof(Resources.ResProduct))]
        public string Name { get; set; }

        [Required(ErrorMessageResourceName = "DescriptionRequired", ErrorMessageResourceType = typeof(Resources.ResProduct))]
        [Display(Name = "Description", ResourceType = typeof(Resources.ResProduct))]
        [StringLength(300)]
        public string Description { get; set; }

        [Required(ErrorMessageResourceName = "OriginalPriceRequired", ErrorMessageResourceType = typeof(Resources.ResProduct))]
        [Display(Name = "OriginalPrice", ResourceType = typeof(Resources.ResProduct))]
        public decimal OriginalPrice { get; set; }


        [Display(Name = "DiscountPrice", ResourceType = typeof(Resources.ResProduct))]
        public decimal? DiscountPrice { get; set; }

        [Required(ErrorMessageResourceName = "QuantityRequired", ErrorMessageResourceType = typeof(Resources.ResProduct))]
        [Display(Name = "Quantity", ResourceType = typeof(Resources.ResProduct))]
        [Range(0, 1000)]
        public int Quantity { get; set; }

        //--- Product Category
        [Required(ErrorMessageResourceName = "CategoryRequired", ErrorMessageResourceType = typeof(Resources.ResProduct))]
        [Display(Name = "Category", ResourceType = typeof(Resources.ResProduct))]
        public int CategoryId { get; set; }

        //--- Product Images
        [Required(ErrorMessageResourceName = "ProductImagesRequired", ErrorMessageResourceType = typeof(Resources.ResProduct))]
        [Display(Name = "ProductImages", ResourceType = typeof(Resources.ResProduct))]
        public HttpPostedFileBase[] ProductImages { get; set; }
    }
}