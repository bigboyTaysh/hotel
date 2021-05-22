using Identity.Service.Models;
using Identity.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Identity.Service.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly UserService service;

        public UsersController(UserService _service)
        {
            service = _service;
        }

        [HttpGet]
        public ActionResult<List<User>> GetUsers()
        {
            return service.GetUsers();
        }

        [HttpGet("{id:length(24)}")]
        public ActionResult<User> GetUser(string id)
        {
            var user = service.GetUser(id);
            return Json(user);
        }

        [HttpPost]
        public ActionResult<User> Create(User user)
        {
            service.Create(user);
            return Json(user);
        }

    }
}
