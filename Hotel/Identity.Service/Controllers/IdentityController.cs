using Identity.Service.Models;
using Identity.Service.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

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

        [Route("token")]
        [HttpPost]
        public ActionResult GetToken()
        {
            Request.Headers.TryGetValue("Authorization", out var token);
            if (string.IsNullOrEmpty(token))
                return Unauthorized();

            var newToken = service.GetNewToken(token);

            if (newToken == null)
                return Unauthorized();

            return Ok(newToken);
        }
    }
}
