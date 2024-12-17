using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestaurantMVC.Models.Auth;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace RestaurantMVC.Controllers
{
    public class AdminController : Controller
    {
        private readonly string baseUrl = "http://localhost:5107/api/Auth";
        private readonly HttpClient _client;
        public AdminController()
        {
            _client = new HttpClient();
        }

        public IActionResult AdminLogin()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AdminLogin(AdminLogin adminLogin)
        {
            var response = await _client.PostAsJsonAsync($"{baseUrl}/Login", adminLogin);
            
            if (!response.IsSuccessStatusCode)
            {
                return View(adminLogin);
            }

            var json = await response.Content.ReadAsStringAsync();
            var token = JsonConvert.DeserializeObject<TokenResponse>(json);

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token.Token);

            var claims = jwtToken.Claims.ToList();

            var claimIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimIdentity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = jwtToken.ValidTo
            });

            HttpContext.Response.Cookies.Append("jwtToken", token.Token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = jwtToken.ValidTo
            });

            return RedirectToAction("index");
        }

        [HttpPost] 
        public async Task<IActionResult> AdminLogout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Response.Cookies.Delete("jwtToken");
            return RedirectToAction("AdminLogin", "Admin");
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }


    }
}
