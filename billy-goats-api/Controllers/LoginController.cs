using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BillyGoats.Api.Data.Services;
using BillyGoats.Api.Models;

namespace BillyGoats.Api.Controllers
{
    [ApiController]
    [Route("api/login")]
    public class LoginController : ControllerBase
    {
        private IDataService<User> userDataService;

        public LoginController(IDataService<User> userDataService)
        {
            this.userDataService = userDataService;
        }

        public virtual async Task<ActionResult> LoginPost(LoginInfo loginInfo)
        {
            // find the user by email, return 404 if you can't find it
            var user = this.userDataService.Get().Result.Where(u => u.Email == loginInfo.Email).FirstOrDefault();

            if (user == null)
            {
                return this.NotFound();
            }

            // hash the given pwd
            string password = BillyGoats.Api.Models.User.HashString(loginInfo.Password);

            // compare to what is stored, create JWT if correct, return 401 if not
            if(user.Password != password)
            {
                return this.Unauthorized();
            }

            LoginInfoResult loginInfoResponse = new LoginInfoResult();
            loginInfoResponse.Jwt = user.GenerateJwt();


            return this.Ok(loginInfoResponse);
        }
    }
}