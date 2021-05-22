using Identity.Service.Models;
using Identity.Service.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController : Controller
    {
        private readonly IdentityService service;

        public IdentityController(IdentityService _service)
        {
            service = _service;
        }

        [Route("authenticate")]
        [HttpPost]
        public ActionResult Login(LoginUser loginUser)
        {
            var user = service.Authenticate(loginUser);

            if (user == null)
                return Unauthorized();

            return Ok(user);
        }
    }
}
