using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BillyGoats.Api.Models
{
    public class Message : ModelBase
    {

        public long? SenderId { get; set; }

        public long? RecipientId { get; set; }


        [Required]
        [MaxLength(75)]
        public string Subject { get; set; }

        [Required]
        public string Body { get; set; }

        [Required]
        public DateTimeOffset Date { get; set; }

        [Required]
        public bool Read { get; set; }

        [ForeignKey("SenderId")]
        public User sender { get; set; }

        [ForeignKey("RecipientId")]
        public User recipient { get; set; }


    }
}
