using System;
using System.Threading.Tasks;
using System.Collections;
using BillyGoats.Api.Controllers.Base;
using BillyGoats.Api.Data.Services;
using BillyGoats.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace BillyGoats.Api.Controllers
{
    [Authorize]
    [Route("api/guest")]
    [Route("api/guest/userId")]
    public class GuestController : ApiControllerBase<Guest>
    {
        
        public GuestController(IDataService<Guest> dataService) : base(dataService)
        {
        }
    }
}
