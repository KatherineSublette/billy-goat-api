using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BillyGoats.Api.Models
{
    public class Resort : ModelBase
    {
        [Required]
        [MaxLength(75)]
        public string Name { get; set; }

    }
}
