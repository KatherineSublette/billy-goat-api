using BillyGoats.Api.Utils.Extensions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace BillyGoats.Api.Filter
{
    public class FilterBinder : IModelBinder
    {
        //Implement base member
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var queryString = bindingContext.ActionContext.HttpContext.Request.QueryString;
            bindingContext.ModelName = bindingContext.FieldName;
            if (!queryString.HasValue)
            {
                // default filter page size to 100
                var model = new FilterRequest();
                bindingContext.Model = model;
                bindingContext.Result = ModelBindingResult.Success(model);
                return Task.CompletedTask;
            }
            try
            {
                var model = new FilterRequest();
                var dict = HttpUtility.ParseQueryString(queryString.Value);
                var allKeys = dict.AllKeys.ToDictionary(k => k, k => dict[k]);
                var filterCriteria = allKeys.Where(kv => kv.Key.ToLower() != "page" && kv.Key.ToLower() != "pagesize" && kv.Key.ToLower() != "expand" && kv.Key.ToLower() != "columns");

                if (allKeys.Any(kv => kv.Key.ToLower() == "columns"))
                {
                    model.SelectedColumns = allKeys.FirstOrDefault(kv => kv.Key.ToLower() == "columns").Value.Split(",");
                }

                model.FilterCriteria = filterCriteria.ToDictionary(f=> f.Key, f=>f.Value);
                bindingContext.Model = model;
                bindingContext.Result = ModelBindingResult.Success(model);
            }
            catch (Exception ex)
            {
                bindingContext.ModelState.AddModelError(
                    bindingContext.ModelName, ex.Message);
            }

            return Task.CompletedTask;
        }
    }
}
