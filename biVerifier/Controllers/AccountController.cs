using biVerifier.Repositories;
using Microsoft.AspNetCore.Mvc;

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
            // Print out the values of username and password for debugging purposes
            Console.WriteLine("Username: {0}", UserName);
            Console.WriteLine("Password: {0}", UserPW);

            try
            {
                var userRepository = new UserRepository(_connectionString);
                var user = userRepository.GetUserByUsernameAndPassword(UserName, UserPW);
                if (user != null)
                {
                    // Authentication successful, redirect to dashboard or another page
                    return RedirectToAction("Index", "Dashboard");
                }
                else
                {
                    // Authentication failed, return to login page with error message
                    ViewBag.ErrorMessage = "Invalid username or password";
                    return View();
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred while trying to authenticate. Please try again later.";
                // Log the exception for troubleshooting
                //logger.LogError(ex, "An error occurred while trying to authenticate user.");
                Console.WriteLine("ERROR: {0}", ex.ToString());
                return View();
            }
        }

    }
}
