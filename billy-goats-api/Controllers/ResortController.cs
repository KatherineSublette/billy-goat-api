using System;
using BillyGoats.Api.Controllers.Base;
using BillyGoats.Api.Data.Services;
using BillyGoats.Api.Models;

namespace BillyGoats.Api.Controllers
{
    public class ResortController : ApiControllerBase<Resort>
    {
        public ResortController(IDataService<Resort> dataService) : base(dataService)
        {
        }
    }
}
