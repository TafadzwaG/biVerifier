﻿using biVerifier.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;

namespace biVerifier.Controllers
{
    public class AccountController : Controller
    {
        private readonly string _connectionString;

        public AccountController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("CrmDb");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string UserName, string UserPW)
        {
            try
            {
                var userRepository = new UserRepository(_connectionString);
                var user = userRepository.GetUserByUsernameAndPassword(UserName, UserPW);

                if (user != null)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, UserName),
                        new Claim(ClaimTypes.Role, user.Role)
                        // Add more claims as needed
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties
                    {
                        // Optional: configure auth properties like expiration
                    };

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.ErrorMessage = "Invalid username or password";
                    return View();
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred during login. Please try again later.";
                Console.WriteLine("Login Error: " + ex.Message);
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
