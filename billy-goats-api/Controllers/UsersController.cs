using System;
using BillyGoats.Api.Controllers.Base;
using BillyGoats.Api.Data.Services;
using BillyGoats.Api.Models;

namespace BillyGoats.Api.Controllers
{
    public class UsersController : ApiControllerBase<User>
    {
        public UsersController(IDataService<User> dataService) : base(dataService)
        {
        }
    }
}
