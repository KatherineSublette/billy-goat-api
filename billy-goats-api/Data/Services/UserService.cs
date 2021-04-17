using System;
using System.Collections.Generic;
using System.Linq;
using BillyGoats.Api.Models;
using BillyGoats.Api.Data.Repositories;
using System.Threading.Tasks;

namespace BillyGoats.Api.Data.Services
{
    public class UserService : DataService<User>
    {
 
        public UserService(IRepository<User> userRepo) : base(userRepo)
        {
        }

        public override Task<User> Add(User item)
        {
            // encrypt password
            item.Password = BillyGoats.Api.Models.User.HashString(item.Password);

            return base.Add(item);
        }
    }
}