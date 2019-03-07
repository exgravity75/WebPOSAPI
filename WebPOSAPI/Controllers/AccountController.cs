using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using WebPOSAPI.Models;

namespace WebPOSAPI.Controllers
{
    public class AccountController : ApiController
    {

        [Route("api/User/Register")]
        [HttpPost]
        public IdentityResult Register(ViewModels.AccountViewModel model)
        {
            // var appDbContext = new ApplicationDbContext();
            var userStore = new ApplicationUserStore(new ApplicationDbContext());
            var userManager = new ApplicationUserManager(userStore);
            var user = new ApplicationUser()
            {
                UserName = model.UserID,
                Email = model.Email
            };
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;

            userManager.PasswordValidator = new PasswordValidator()
            {
                RequiredLength = 5
            };

            IdentityResult result = userManager.Create(user, model.Password);

            return result;

        }


        [HttpGet]
        [Authorize]
        [Route("api/User/GetUserClaims")]
        public ViewModels.AccountViewModel GetUserClaims()
        {
            var identityClaims = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identityClaims.Claims;
            ViewModels.AccountViewModel model = new ViewModels.AccountViewModel()
            {
                UserID = identityClaims.FindFirst("UserName").Value,
                Email = identityClaims.FindFirst("Email").Value,
                FirstName = identityClaims.FindFirst("FirstName").Value,
                LastName = identityClaims.FindFirst("LastName").Value,
                LoggedOn = identityClaims.FindFirst("LoggedOn").Value

            };

            return model;

        }

    }
}
