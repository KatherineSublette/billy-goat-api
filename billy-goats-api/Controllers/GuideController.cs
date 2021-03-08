using System;
using BillyGoats.Api.Controllers.Base;
using BillyGoats.Api.Data.Services;
using BillyGoats.Api.Models;

namespace BillyGoats.Api.Controllers
{
    public class GuideController : ApiControllerBase<Guide>
    {
        public GuideController(IDataService<Guide> dataService) : base(dataService)
        {
        }
    }
}
