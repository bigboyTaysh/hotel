using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using HotelApp.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using System.Net;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace HotelApp.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly HttpClient _client;
        private readonly string _identityServiceUrl;


        public LoginModel(SignInManager<ApplicationUser> signInManager, 
            ILogger<LoginModel> logger,
            UserManager<ApplicationUser> userManager,
            IConfiguration config)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _identityServiceUrl = config.GetSection("Identity.Service").GetSection("IdentityConnection").Value;
            _client = new HttpClient();
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                //var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);

                string json = JsonConvert.SerializeObject(new { Email = Input.Email, Password = Input.Password });
                StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _client.PostAsync(_identityServiceUrl, httpContent);

                if (response.StatusCode == HttpStatusCode.OK)
                {


                    LoggedUser loggedUser = JsonConvert.DeserializeObject<LoggedUser>(response.Content.ReadAsStringAsync().Result);


                    /*
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Email, loggedUser.Email),
                        new Claim(ClaimTypes.Role, loggedUser.Role),
                    };
                    */

                    /*

                    var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                    identity.AddClaim(new Claim(ClaimTypes.Name, loggedUser.Email));
                    identity.AddClaim(new Claim(ClaimTypes.Email, loggedUser.Email));
                    identity.AddClaim(new Claim(ClaimTypes.Role, loggedUser.Role));
                    identity.AddClaim(new Claim(ClaimTypes.Authentication, loggedUser.Token));


                    var principal = new ClaimsPrincipal(identity);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                    */

                    var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, loggedUser.Email),
                            new Claim(ClaimTypes.Email, loggedUser.Email),
                            new Claim(ClaimTypes.Role, loggedUser.Role),
                            new Claim(ClaimTypes.Authentication, loggedUser.Token),
                        };

                    var claimsIdentity = new ClaimsIdentity(
                        claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        new AuthenticationProperties
                        {
                            IsPersistent = true,
                        });

                    /*
                    var claimsIdentity = new ClaimsIdentity(
                        claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    */
                    /*
                    var user = new ApplicationUser();
                    user.Email = loggedUser.Email;

                    IdentityResult result = await _userManager.CreateAsync(user, Input.Password);
                    result = await _userManager.AddToRoleAsync(user, loggedUser.Role);
                    result = await _userManager.SetAuthenticationTokenAsync(user, null, "Bearer", loggedUser.Token);

                    */

                    _logger.LogInformation("User logged in.");
                    return LocalRedirect(returnUrl);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
