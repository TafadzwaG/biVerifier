using biVerifier.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.OleDb;
using System.Diagnostics;

namespace biVerifier.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            AggregatedSalesConsultant();
            getCountsInDb();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Reports()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        [HttpGet]
        public IActionResult AggregatedSalesConsultant()
        {
            string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=E:\CODING_HASHIRA\PROJECTS\.NET\databaseAccess\VERIFIER1.accdb;Persist Security Info=False;";

            string query = @"
            SELECT Consultant, SUM(GandTotal) AS TotalSales
            FROM CRM
            WHERE Status = 'CL'
            GROUP BY Consultant";

            var aggregatedSalesData = new List<AggregatedSalesData>();

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            using (OleDbCommand command = new OleDbCommand(query, connection))
            {
                try
                {
                    connection.Open();
                    using (OleDbDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var salesData = new AggregatedSalesData
                            {
                                ConsultantName = reader["Consultant"].ToString(),
                                TotalSales = Convert.ToDecimal(reader["TotalSales"])
                            };
                            aggregatedSalesData.Add(salesData);
                        }
                    }
                }
                catch (OleDbException ex)
                {
                    Console.WriteLine("OleDbException occurred: " + ex.Message);
                    // Handle the exception if needed
                }
            }

            Console.WriteLine("Number of aggregated sales data: " + aggregatedSalesData.Count);

            if (aggregatedSalesData == null || aggregatedSalesData.Count == 0)
            {
                // Handle the scenario where aggregatedSalesData is null or empty
                // For example, you can redirect the user to an error page
                return RedirectToAction("Error", "Home");
            }

            return View("Index", aggregatedSalesData);
        }


        public IActionResult getCountsInDb()
        {
            int siteCount = 0;
            int cancellationCount = 0;

            // Your logic to query the database and get the counts
            // Assuming you're using ADO.NET with OleDbConnection and OleDbCommand

            string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=E:\CODING_HASHIRA\PROJECTS\.NET\databaseAccess\VERIFIER1.accdb;Persist Security Info=False;";
            string siteCountQuery = "SELECT COUNT(*) FROM Sites";
            string cancellationCountQuery = "SELECT COUNT(*) FROM Cancellations";

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                using (OleDbCommand siteCommand = new OleDbCommand(siteCountQuery, connection))
                using (OleDbCommand cancellationCommand = new OleDbCommand(cancellationCountQuery, connection))
                {
                    try
                    {
                        connection.Open();
                        // Execute query to get site count
                        siteCount = (int)siteCommand.ExecuteScalar();
                        // Execute query to get cancellation count
                        cancellationCount = (int)cancellationCommand.ExecuteScalar();
                    }
                    catch (OleDbException ex)
                    {
                        // Handle exception if needed
                        Console.WriteLine("OleDbException occurred: " + ex.Message);
                    }
                }
            }

            // Pass the counts to the Index view
            ViewData["SiteCount"] = siteCount;
            ViewData["CancellationCount"] = cancellationCount;

            // Return the counts instead of a view
            return RedirectToAction("Index");
        }
    }
}
