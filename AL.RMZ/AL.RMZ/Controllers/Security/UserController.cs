using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace AL.RMZ.Controllers.Security
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserController : Controller
    {
        [HttpGet]
        public ActionResult CurrentUserName()
        {
            return Ok(User.Identity.Name);
        }
    }
}
