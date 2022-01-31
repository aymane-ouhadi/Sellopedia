using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sellopedia.Models
{
    public class MessageViewModel
    {
        [Display(Name = "Subject", ResourceType = typeof(Resources.ResMessage))]
        public string Subject { get; set; }

        [Required(ErrorMessageResourceName = "ContentRequired", ErrorMessageResourceType = typeof(Resources.ResMessage))]
        [Display(Name = "Content", ResourceType = typeof(Resources.ResMessage))]
        public string Content { get; set; }
    }
}