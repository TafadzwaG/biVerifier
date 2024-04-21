using biVerifier.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.OleDb;
using System.Diagnostics;
using System.Data.Odbc;

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
            getCountsInDb();
            AggregatedSalesConsultant();
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
            string connectionString = @"Driver={Microsoft Access Driver (*.mdb, *.accdb)};DBQ=E:\CODING_HASHIRA\PROJECTS\.NET\databaseAccess\VERIFIER1.accdb;Persist Security Info=False;";

            string query = @"
            SELECT Consultant, SUM(GandTotal) AS TotalSales
            FROM CRM
            WHERE Status = 'CL'
            GROUP BY Consultant";

            var aggregatedSalesData = new List<AggregatedSalesData>();

            using (OdbcConnection connection = new OdbcConnection(connectionString))
            using (OdbcCommand command = new OdbcCommand(query, connection))

            {
                try
                {
                    connection.Open();
                    using (OdbcDataReader reader = command.ExecuteReader())

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

            string connectionString = @"Driver={Microsoft Access Driver (*.mdb, *.accdb)};DBQ=E:\CODING_HASHIRA\PROJECTS\.NET\databaseAccess\VERIFIER1.accdb;Persist Security Info=False;";

            string siteCountQuery = "SELECT COUNT(*) FROM Sites";
            string cancellationCountQuery = "SELECT COUNT(*) FROM Cancellations";

            using (OdbcConnection connection = new OdbcConnection(connectionString))
            {
                using (OdbcCommand siteCommand = new OdbcCommand(siteCountQuery, connection))
                using (OdbcCommand cancellationCommand = new OdbcCommand(cancellationCountQuery, connection))
                {
                    try
                    {
                        connection.Open();
                        siteCount = (int)siteCommand.ExecuteScalar();
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

            Console.WriteLine("Count of sites: " + ViewData["SiteCount"] + "CancellationCount" + ViewData["CancellationCount"]);

            // Return the counts instead of a view
            return RedirectToAction("Index");
        }
    }
}
