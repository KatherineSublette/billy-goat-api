using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace BillyGoats.Api.Models
{
    public class LoginInfoResult
    {
        [JsonProperty("jwt")]
        public string Jwt { get; set; }

    }
}