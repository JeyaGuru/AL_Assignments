using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

namespace AL.RMZ.Controllers.Security
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public IConfiguration Configuration { get; }
        public AuthController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        [HttpGet]
        [Route("Login")]
        public async Task Login()
        {
            await HttpContext.ChallengeAsync(new AuthenticationProperties() { RedirectUri = Configuration.GetValue<string>("Url:Home") });
        }

        [HttpGet]
        [Route("LogOut")]
        [Authorize]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            //Revoke access token
           //await HttpContext.SignOutAsync();

            return Redirect(Configuration.GetValue<string>("Url:Home"));
        }
    }
}
