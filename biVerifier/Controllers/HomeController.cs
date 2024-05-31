using biVerifier.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.OleDb;
using System.Diagnostics;
using System.Data.Odbc;
using Microsoft.AspNetCore.Authorization;


namespace biVerifier.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private string connectionString = @"Driver={Microsoft Access Driver (*.mdb, *.accdb)};DBQ=C:\Users\Tafadzwag\Documents\DATABASE\VERIFIER2.accdb;Persist Security Info=False;";
        //private string connectionString = @"Driver={Microsoft Access Driver (*.mdb, *.accdb)};DBQ=E:\CODING_HASHIRA\PROJECTS\.NET\databaseAccess\VERIFIER2.accdb;Persist Security Info=False;";

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }


        public IActionResult Index()
        {
            getCountsInDb();
            AggregatedSalesConsultant();
            reasonForCancellation();
            //salesLeadSource();
            getCurrentYearRevenue();
            //getSitesByRegion();
            getSalesByRegion();
            clientCountGraphByLeadSource();
            //consultantTotalMonitoringFees();
            ConsultantTotalMonitoringFees();
            clientCountByMarketType();
            return View();
        }


        public IActionResult clientCountByMarketType()
        {
            Dictionary<string, int> clientCountByMarket = new Dictionary<string, int>();

            using (OdbcConnection connection = new OdbcConnection(connectionString))
            {
                connection.Open();

                using (OdbcCommand command = new OdbcCommand("SELECT Market, COUNT(Client) AS ClientCount FROM CRM GROUP BY Market", connection))
                {
                    using (OdbcDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string market = reader["Market"].ToString();
                            int clientCount = int.Parse(reader["ClientCount"].ToString());
                            clientCountByMarket.Add(string.IsNullOrEmpty(market) ? "Other" : market, clientCount);
                        }
                    }
                }
            }

            // Output the result to the console
            foreach (var item in clientCountByMarket)
            {
                Console.WriteLine($"Market: {item.Key}, ClientCount: {item.Value}");
            }

            ViewData["ClientCountByMarket"] = clientCountByMarket;
            return View("Index");
        }

        //public IActionResult consultantTotalMonitoringFees()
        //{
        //    Dictionary<string, decimal> totalMonitoringFeesByConsultant = new Dictionary<string, decimal>();

        //    using (OdbcConnection connection = new OdbcConnection(connectionString))
        //    {
        //        connection.Open();

        //        using (OdbcCommand command = new OdbcCommand("SELECT Consultant, SUM(TotalMonitoringFees) FROM CRM GROUP BY Consultant", connection))
        //        {
        //            using (OdbcDataReader reader = command.ExecuteReader())
        //            {
        //                while (reader.Read())
        //                {
        //                    string consultant = reader["Consultant"].ToString();
        //                    if (!reader.IsDBNull(1))
        //                    {
        //                        decimal totalMonitoringFees = reader.GetDecimal(1);
        //                        totalMonitoringFeesByConsultant.Add(consultant, totalMonitoringFees);
        //                    }
        //                    else
        //                    {
        //                        // Handle null or empty value
        //                        totalMonitoringFeesByConsultant.Add(consultant, 0m); // Or any other default value
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    // Output the result to the console
        //    foreach (var item in totalMonitoringFeesByConsultant)
        //    {
        //        Console.WriteLine($"Consultant: {item.Key}, TotalMonitoringFees: {item.Value}");
        //    }

        //    ViewData["TotalMonitoringFeesByConsultant"] = totalMonitoringFeesByConsultant;
        //    return View("Index");
        //}

        public IActionResult ConsultantTotalMonitoringFees()
        {
            Dictionary<string, decimal> totalMonitoringFeesByConsultant = new Dictionary<string, decimal>();

            using (OdbcConnection connection = new OdbcConnection(connectionString))
            {
                connection.Open();

                using (OdbcCommand command = new OdbcCommand("SELECT Consultant, SUM(TotalMonitoringFees) FROM CRM GROUP BY Consultant", connection))
                {
                    using (OdbcDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string consultant = reader["Consultant"].ToString();

                            if (totalMonitoringFeesByConsultant.ContainsKey(consultant))
                            {
                                // Handle duplicate consultant entry if necessary
                                continue; // Skip this entry
                            }

                            if (!reader.IsDBNull(1))
                            {
                                decimal totalMonitoringFees = reader.GetDecimal(1);
                                totalMonitoringFeesByConsultant.Add(consultant, totalMonitoringFees);
                            }
                            else
                            {
                                // Handle null or empty value
                                totalMonitoringFeesByConsultant.Add(consultant, 0m); // Or any other default value
                            }
                        }
                    }
                }
            }

            // Store the dictionary in ViewData
            ViewData["TotalMonitoringFeesByConsultant"] = totalMonitoringFeesByConsultant;

            return View("Index");
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
            //string connectionString = @"Driver={Microsoft Access Driver (*.mdb, *.accdb)};DBQ=C:\Users\CRM Server\Documents\veriDB\VERIFIER2.accdb;Persist Security Info=False;";
            string connectionString = @"Driver={Microsoft Access Driver (*.mdb, *.accdb)};DBQ=C:\Users\Tafadzwag\Documents\DATABASE\VERIFIER2.accdb;Persist Security Info=False;";

            string query = @"
            SELECT Consultant, SUM(TotalMonitoringFees) AS TotalSales
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

            string connectionString = @"Driver={Microsoft Access Driver (*.mdb, *.accdb)};DBQ=C:\Users\Tafadzwag\Documents\DATABASE\VERIFIER2.accdb;Persist Security Info=False;";
            //string connectionString = @"Driver={Microsoft Access Driver (*.mdb, *.accdb)};DBQ=E:\CODING_HASHIRA\PROJECTS\.NET\databaseAccess\VERIFIER2.accdb;Persist Security Info=False;";

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



        public IActionResult reasonForCancellation()
        {
            string connectionString = @"Driver={Microsoft Access Driver (*.mdb, *.accdb)};DBQ=C:\Users\Tafadzwag\Documents\DATABASE\VERIFIER2.accdb;Persist Security Info=False;";
            //string connectionString = @"Driver={Microsoft Access Driver (*.mdb, *.accdb)};DBQ=E:\CODING_HASHIRA\PROJECTS\.NET\databaseAccess\VERIFIER2.accdb;Persist Security Info=False;";

            List<string> cancellationReasons = new List<string>();
            List<decimal> revenueLost = new List<decimal>();

            string query = "SELECT Reason, [Reduced Value ex vat] FROM Cancellations";

            using (OdbcConnection connection = new OdbcConnection(connectionString))
            {
                using (OdbcCommand command = new OdbcCommand(query, connection))
                {
                    try
                    {
                        connection.Open();
                        using (OdbcDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string? reason = reader["Reason"].ToString();
                                decimal reducedValue = Convert.ToDecimal(reader["Reduced Value ex vat"]);

                                if (string.IsNullOrEmpty(reason))
                                {
                                    reason = "Other";
                                }

                                if (cancellationReasons.Contains(reason))
                                {
                                    int index = cancellationReasons.IndexOf(reason);
                                    revenueLost[index] += reducedValue;
                                }
                                else
                                {
                                    cancellationReasons.Add(reason);
                                    revenueLost.Add(reducedValue);
                                }
                            }
                        }
                    }
                    catch (OdbcException ex)
                    {
                        // Handle exception
                    }
                }
            }

            // Pass the cancellation reasons and revenue lost to the Index view
            ViewData["CancellationReasons"] = cancellationReasons;
            ViewData["RevenueLost"] = revenueLost;

            Console.WriteLine("CancellationReasons" + cancellationReasons);
            Console.WriteLine("RevenueLost" + revenueLost);

            return View("Index");
        }

        public IActionResult salesLeadSource()
        {
            //string connectionString = @"Driver={Microsoft Access Driver (*.mdb, *.accdb)};DBQ=C:\Users\CRM Server\Documents\veriDB\VERIFIER2.accdb;Persist Security Info=False;";
            string connectionString = @"Driver={Microsoft Access Driver (*.mdb, *.accdb)};DBQ=C:\Users\Tafadzwag\Documents\DATABASE\VERIFIER2.accdb;Persist Security Info=False;";

            Dictionary<string, int> leadSourceCounts = new Dictionary<string, int>();

            using (OdbcConnection connection = new OdbcConnection(connectionString))
            {
                string query = "SELECT Lead_Source, COUNT(*) AS Count FROM CRM GROUP BY Lead_Source";

                using (OdbcCommand command = new OdbcCommand(query, connection))
                {
                    try
                    {
                        connection.Open();

                        using (OdbcDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string? leadSource = reader["Lead_Source"].ToString();
                                int count = int.Parse(reader["Count"].ToString());

                                leadSourceCounts.Add(leadSource, count);
                            }
                        }
                    }
                    catch (OdbcException ex)
                    {
                        // Handle exception
                        Console.WriteLine("OdbcException occurred: " + ex.Message);
                    }
                }
            }

            ViewData["LeadSourceCounts"] = leadSourceCounts;

            return RedirectToAction("Index");
        }


        public IActionResult getCurrentYearRevenue()
        {
            string connectionString = @"Driver={Microsoft Access Driver (*.mdb, *.accdb)};DBQ=C:\Users\Tafadzwag\Documents\DATABASE\VERIFIER2.accdb;Persist Security Info=False;";
            //string connectionString = @"Driver={Microsoft Access Driver (*.mdb, *.accdb)};DBQ=E:\CODING_HASHIRA\PROJECTS\.NET\databaseAccess\VERIFIER2.accdb;Persist Security Info=False;";

            int currentYear = DateTime.Now.Year;

            decimal totalRevenue = 0;

            List<decimal> totalRevenueList = new List<decimal>();

            string query = "SELECT SUM(TotalMonitoringFees) AS TotalRevenue FROM CRM WHERE liveyear = ?";

            using (OdbcConnection connection = new OdbcConnection(connectionString))
            using (OdbcCommand command = new OdbcCommand(query, connection))
            {
                try
                {
                    // Add parameter to the command
                    command.Parameters.AddWithValue("liveyear", currentYear);

                    // Open the database connection
                    connection.Open();

                    // Execute the query and retrieve the result
                    totalRevenue = (decimal)command.ExecuteScalar();

                    // Add the TotalRevenue to the list
                    totalRevenueList.Add(totalRevenue);
                }
                catch (OleDbException ex)
                {
                    // Handle exception if needed
                    Console.WriteLine("OleDbException occurred: " + ex.Message);
                }
            }

            // Pass the TotalRevenue to the Index view
            ViewData["TotalRevenue"] = totalRevenueList;

            // Return the view to render the TotalRevenue
            return View("Index");
        }

        [HttpGet]
        public IActionResult getSitesByRegion()
        {
            //string connectionString = @"Driver={Microsoft Access Driver (*.mdb, *.accdb)};DBQ=E:\CODING_HASHIRA\PROJECTS\.NET\databaseAccess\VERIFIER1.accdb;Persist Security Info=False;";
            string connectionString = @"Driver={Microsoft Access Driver (*.mdb, *.accdb)};DBQ=C:\Users\Tafadzwag\Documents\DATABASE\VERIFIER2.accdb;Persist Security Info=False;";

            // Initialize a dictionary to store the count of sites per region
            Dictionary<string, int> sitesByRegion = new Dictionary<string, int>();

            // Define the query to calculate the count of sites per region
            string query = "SELECT Region, COUNT(Sites) AS SiteCount FROM CRM GROUP BY Region";

            using (OdbcConnection connection = new OdbcConnection(connectionString))
            {
                using (OdbcCommand command = new OdbcCommand(query, connection))
                {
                    try
                    {
                        connection.Open();
                        using (OdbcDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string region = reader["Region"].ToString();
                                int siteCount = Convert.ToInt32(reader["SiteCount"]);

                                // Add the count of sites for each region to the dictionary
                                sitesByRegion.Add(region, siteCount);
                            }
                        }
                    }
                    catch (OdbcException ex)
                    {
                        // Handle exception if needed
                        Console.WriteLine("OdbcException occurred: " + ex.Message);
                    }
                }
            }

            // Pass the dictionary containing sites per region data to the Index view
            ViewData["SitesByRegion"] = sitesByRegion;

            // Return the view to render the sites per region data
            return View("Index");
        }

        public IActionResult getSalesByRegion()
        {
            string connectionString = @"Driver={Microsoft Access Driver (*.mdb, *.accdb)};DBQ=C:\Users\Tafadzwag\Documents\DATABASE\VERIFIER2.accdb;Persist Security Info=False;";
            //string connectionString = @"Driver={Microsoft Access Driver (*.mdb, *.accdb)};DBQ=E:\CODING_HASHIRA\PROJECTS\.NET\databaseAccess\VERIFIER2.accdb;Persist Security Info=False;";

            List<string> provinces = new List<string>();
            List<int> clientCounts = new List<int>();

            string query = "SELECT Province, COUNT(Client) AS ClientCount FROM CRM GROUP BY Province";

            using (OdbcConnection connection = new OdbcConnection(connectionString))
            {
                using (OdbcCommand command = new OdbcCommand(query, connection))
                {
                    try
                    {
                        connection.Open();
                        using (OdbcDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string province = reader["Province"].ToString();
                                int clientCount = Convert.ToInt32(reader["ClientCount"]);

                                provinces.Add(province);
                                clientCounts.Add(clientCount);
                            }
                        }
                    }
                    catch (OdbcException ex)
                    {
                        // Handle exception
                    }
                }
            }

            ViewData["Provinces"] = provinces;
            ViewData["ClientCounts"] = clientCounts;

            return View("Index");
        }

        public IActionResult clientCountGraphByLeadSource()
        {
            string connectionString = @"Driver={Microsoft Access Driver (*.mdb, *.accdb)};DBQ=C:\Users\Tafadzwag\Documents\DATABASE\VERIFIER2.accdb;Persist Security Info=False;";
            //string connectionString = @"Driver={Microsoft Access Driver (*.mdb, *.accdb)};DBQ=E:\CODING_HASHIRA\PROJECTS\.NET\databaseAccess\VERIFIER2.accdb;Persist Security Info=False;";

            var leadSourceCounts = new Dictionary<string, int>();

            // Query the database to get the count of clients by lead source
            string query = @"
                SELECT LeadSource, COUNT(Client) AS ClientCount
                FROM CRM
                GROUP BY LeadSource";

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
                            string leadSource = reader["LeadSource"].ToString();
                            int clientCount = Convert.ToInt32(reader["ClientCount"]);
                            leadSourceCounts.Add(leadSource, clientCount);
                        }
                    }

                    // Log the lead source counts
                    foreach (var kvp in leadSourceCounts)
                    {
                        Console.WriteLine($"Lead Source: {kvp.Key}, Client Count: {kvp.Value}");
                    }
                }
                catch (OdbcException ex)
                {
                    // Handle exception
                }
            }

            // Pass the lead source counts to the view
            ViewData["LeadSourceCounts"] = leadSourceCounts;

            return View("Index");
        }





    }





}

