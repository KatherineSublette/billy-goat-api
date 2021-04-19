using System;
using BillyGoats.Api.Controllers.Base;
using BillyGoats.Api.Data.Services;
using BillyGoats.Api.Models;

namespace BillyGoats.Api.Controllers
{
    public class MessageController : ApiControllerBase<Message>
    {
        public MessageController(IDataService<Message> dataService) : base(dataService)
        {
        }
    }
}
