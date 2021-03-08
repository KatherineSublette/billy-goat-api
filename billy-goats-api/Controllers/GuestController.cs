using System;
using BillyGoats.Api.Controllers.Base;
using BillyGoats.Api.Data.Services;
using BillyGoats.Api.Models;
using Microsoft.AspNetCore.Authorization;

namespace BillyGoats.Api.Controllers
{
    [Authorize] 
    public class GuestController : ApiControllerBase<Guest>
    {
        public GuestController(IDataService<Guest> dataService) : base(dataService)
        {
        }
    }
}
