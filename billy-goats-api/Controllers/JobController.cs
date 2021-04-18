using System;
using BillyGoats.Api.Controllers.Base;
using BillyGoats.Api.Data.Services;
using BillyGoats.Api.Models;

namespace BillyGoats.Api.Controllers
{
    public class JobController : ApiControllerBase<Job>
    {
        public JobController(IDataService<Job> dataService) : base(dataService)
        {
        }
    }
}
