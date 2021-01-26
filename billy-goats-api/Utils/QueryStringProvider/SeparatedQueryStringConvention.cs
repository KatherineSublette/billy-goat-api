using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BillyGoats.Api.Utils
{
    public class SeparatedQueryStringConvention : IActionModelConvention
    {
        public void Apply(ActionModel action)
        {
            foreach (var parameter in action.Parameters)
            {
                if (parameter.Attributes.OfType<SeparatedByAttribute>().Any() && !parameter.Action.Filters.OfType<SeparatedQueryStringAttribute>().Any())
                {
                    var attribute = parameter.Attributes.OfType<SeparatedByAttribute>().FirstOrDefault();
                    parameter.Action.Filters.Add(new SeparatedQueryStringAttribute(parameter.ParameterName, attribute.Separator));
                }
            }
        }
    }
}
