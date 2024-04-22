using biVerifier.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;


namespace biVerifier.Controllers
{
    public class AccountController : Controller
    {
        private readonly string _connectionString;

        public AccountController()
        {
            // Initialize connection string
            _connectionString = @"Driver={Microsoft Access Driver (*.mdb, *.accdb)};DBQ=E:\CODING_HASHIRA\PROJECTS\.NET\databaseAccess\VERIFIER1.accdb;Persist Security Info=False;";
        }


        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string UserName, string UserPW)
        {
            Console.WriteLine("Username: {0}", UserName);
            Console.WriteLine("Password: {0}", UserPW);

            try
            {
                var userRepository = new UserRepository(_connectionString);
                var user = userRepository.GetUserByUsernameAndPassword(UserName, UserPW);
                if (user != null)
                {
                    // Set authentication cookie
                    var claims = new[]
                    {
                       new Claim(ClaimTypes.Name, UserName),
                        // Add more claims if needed
                    };
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties
                    {
                        // Set any additional properties
                    };
                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

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
                ViewBag.ErrorMessage = "An error occurred while trying to authenticate. Please try again later.";
                Console.WriteLine("ERROR: {0}", ex.ToString());
                return View();
            }
        }


    }
}
