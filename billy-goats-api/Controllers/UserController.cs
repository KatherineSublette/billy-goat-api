using System;
using BillyGoats.Api.Controllers.Base;
using BillyGoats.Api.Data.Services;
using BillyGoats.Api.Models;

namespace BillyGoats.Api.Controllers
{
    public class UserController : ApiControllerBase<User>
    {
        public UserController(IDataService<User> dataService) : base(dataService)
        {
        }
    }
}
