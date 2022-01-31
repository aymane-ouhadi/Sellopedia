using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using static Sellopedia.Models.EnumerationsClass;

namespace Sellopedia.Models
{
    public class Message
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string Subject { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public MessageState MessageState { get; set; }

        [Required]
        public DateTime SendingTime { get; set; }

        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        //Constructor 
        public Message()
        {
            this.SendingTime = DateTime.Now;
            this.MessageState = MessageState.Pending;
        }

    }
}