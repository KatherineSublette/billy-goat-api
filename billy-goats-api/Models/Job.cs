using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BillyGoats.Api.Models
{
    public class Job : ModelBase
    {
        [Required]
        public long ResortId { get; set; }

        public long? GuestId { get; set; }

        public long? GuideId { get; set; }


        [Required]
        [MaxLength(75)]
        public string Name { get; set; }

        [Required]
        [MaxLength(75)]
        public DateTime Date { get; set; }

        [Required]
        [MaxLength(75)]
        public bool Completed { get; set; }

        [ForeignKey("GuestId")]
        public Guest Guest { get; set; }

        [ForeignKey("GuideId")]
        public Guide Guide { get; set; }

        [ForeignKey("ResortId")]
        public Resort Resort { get; set; }

    }
}
