using HotelApp.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Owin;
using System.Threading;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HotelApp.Controllers
{
    [Route("authentication/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly HttpClient _client;
        private readonly string _identityServiceUrl;

        public LoginController(IConfiguration config)
        {
            _identityServiceUrl = config.GetSection("Identity.Service").GetSection("IdentityConnection").Value;
            _client = new HttpClient();
        }

        public class InputModel
        {
            public string Login { get; set; }

            public string Password { get; set; }
        }

        [HttpPost]
        public async Task<IActionResult> OnPostAsync(InputModel inputModel)
        {

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, set lockoutOnFailure: true
            //var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);

            string json = JsonConvert.SerializeObject(new { Login = inputModel.Login, Password = inputModel.Password });
            StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _client.PostAsync(_identityServiceUrl, httpContent);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return Ok(response.Content.ReadAsStringAsync().Result);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return Unauthorized(ModelState);
            }
        }

        // PUT api/<LoginController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<LoginController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
