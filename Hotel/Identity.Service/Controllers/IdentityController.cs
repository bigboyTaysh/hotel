using Identity.Service.Models;
using Identity.Service.Services;
using Microsoft.AspNetCore.Mvc;

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
            var token = service.Authenticate(loginUser);

            if (token == null)
                return Unauthorized();

            return Ok(token);
        }

        [Route("logout")]
        [HttpPost]
        public ActionResult Logout(LogoutUser token)
        {
            service.Logout(token.Token);

            return Ok();
        }
    }
}
