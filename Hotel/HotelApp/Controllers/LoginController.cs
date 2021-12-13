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
    [Route("authentication/")]
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

        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> OnPostAsync(InputModel inputModel)
        {

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, set lockoutOnFailure: true
            //var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);

            string json = JsonConvert.SerializeObject(new { Login = inputModel.Login, Password = inputModel.Password });
            StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _client.PostAsync(_identityServiceUrl + "authenticate/", httpContent);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return Ok(response.Content.ReadAsAsync<Token>().Result);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return Unauthorized(ModelState);
            }
        }

        [Route("logout")]
        [HttpPost]
        public async Task<IActionResult> OnPostLogoutAsync(LogoutUser token)
        {
            string json = JsonConvert.SerializeObject(token);
            StringContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _client.PostAsync(_identityServiceUrl + "logout/", httpContent);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return Ok();
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid logout attempt.");
                return Unauthorized(ModelState);
            }
        }

        [Route("token")]
        [HttpPost]
        public async Task<IActionResult> OnPostTokenAsync()
        {
            Request.Headers.TryGetValue("Authorization", out var token);
            if (string.IsNullOrEmpty(token))
                return Unauthorized();
            _client.DefaultRequestHeaders.Add("Authorization", token.FirstOrDefault());

            HttpResponseMessage response = await _client.PostAsync(_identityServiceUrl + "token/", null);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var resp = response.Content.ReadAsAsync<Token>().Result;
                return Ok(resp);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid get a token attempt.");
                return Unauthorized(ModelState);
            }
        }
    }
}
