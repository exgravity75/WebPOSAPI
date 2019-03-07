using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using WebPOSAPI.Models;

namespace WebPOSAPI.Authorization
{
    public class AuthorizationServerProvider :OAuthAuthorizationServerProvider
    {
       
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            //context.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
            //context.Response.Headers.Add("Access-Control-Allow-Methods", new[] { "GET, PUT, DELETE, POST, OPTIONS" });
            //context.Response.Headers.Add("Access-Control-Allow-Headers", new[] { "Content-Type, Accept, Authorization" });
            //context.Response.Headers.Add("Access-Control-Max-Age", new[] { "1728000" });

            var userStore = new UserStore<ApplicationUser>(new ApplicationDbContext());
            var userManager = new UserManager<ApplicationUser>(userStore);
            var user = await userManager.FindAsync(context.UserName, context.Password);

            if (user != null)
            {
                var claims = new List<Claim>()
                {
                    new Claim("UserName", user.UserName),
                    new Claim("Email", user.Email),
                    new Claim("FirstName", user.FirstName),
                    new Claim("LastName", user.LastName),
                    new Claim("LoggedOn", DateTime.Now.ToString())
                };

                ClaimsIdentity oAuthIdentity = new ClaimsIdentity(claims, context.Options.AuthenticationType);
                context.Validated(new AuthenticationTicket(oAuthIdentity, new AuthenticationProperties() { }));
            }
            else
            {
                context.SetError("Invalid Grant", "UserName or password is incorrect");
                return;
            }

            //return Task.Factory.StartNew(() =>
            //{
            //    var username = context.UserName;
            //    var password = context.Password;

            //    if(username != "")
            //    {
            //        var claims = new List<Claim>()
            //        {
            //            new Claim(ClaimTypes.Name, username),
            //            new Claim("UserID", username),
            //            new Claim(ClaimTypes.Role, username)
            //        };

            //        ClaimsIdentity oAuthIdentity = new ClaimsIdentity(claims, context.Options.AuthenticationType);
            //        context.Validated(new AuthenticationTicket(oAuthIdentity, new AuthenticationProperties() { }));
            //        //ClaimsIdentity oAuthIdentity = new ClaimsIdentity(claims, Startup.o);
            //    }
            //    else
            //    {
            //        context.SetError("Invalid Grant", "UserName or password is incorrect");
            //    }
            //});

        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            if(context.ClientId == null)
            {
                context.Validated();
            }
            return Task.FromResult<object>(null);
        }

        private void SetContextHeaders(IOwinContext context)
        {
            context.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
            context.Response.Headers.Add("Access-Control-Allow-Methods", new[] { "GET, PUT, DELETE, POST, OPTIONS" });
            context.Response.Headers.Add("Access-Control-Allow-Headers", new[] { "Content-Type, Accept, Authorization" });
            context.Response.Headers.Add("Access-Control-Max-Age", new[] { "1728000" });
        }
    }
}