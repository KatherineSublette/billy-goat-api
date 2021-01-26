using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace BillyGoats.Api.Utils
{
    [AttributeUsage(AttributeTargets.Parameter, Inherited = true, AllowMultiple = false)]
    public class SeparatedByAttribute : FromQueryAttribute
    {
        public string Separator { get; set; }

        public SeparatedByAttribute(string separator = ",") : base()
        {
            Separator = separator;
        }
    }
}
