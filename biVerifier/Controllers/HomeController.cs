using biVerifier.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.Odbc;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;

namespace biVerifier.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly string _connectionString;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _connectionString = configuration.GetConnectionString("CrmDb");
        }

        public IActionResult Index()
        {
            getCountsInDb();
            AggregatedSalesConsultant();
            reasonForCancellation();
            getCurrentYearRevenue();
            getSalesByRegion();
            clientCountGraphByLeadSource();
            ConsultantTotalMonitoringFees();
            clientCountByMarketType();
            return View();
        }

        public IActionResult clientCountByMarketType()
        {
            Dictionary<string, int> clientCountByMarket = new Dictionary<string, int>();

            using (OdbcConnection connection = new OdbcConnection(_connectionString))
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

            ViewData["ClientCountByMarket"] = clientCountByMarket;
            return View("Index");
        }

        public IActionResult ConsultantTotalMonitoringFees()
        {
            Dictionary<string, decimal> totalMonitoringFeesByConsultant = new Dictionary<string, decimal>();

            using (OdbcConnection connection = new OdbcConnection(_connectionString))
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
                                continue;
                            }

                            if (!reader.IsDBNull(1))
                            {
                                decimal totalMonitoringFees = Convert.ToDecimal(reader.GetDouble(1));
                                totalMonitoringFeesByConsultant.Add(consultant, totalMonitoringFees);
                            }
                            else
                            {
                                totalMonitoringFeesByConsultant.Add(consultant, 0m);
                            }
                        }
                    }
                }
            }

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
            string query = @"
            SELECT Consultant, SUM(TotalMonitoringFees) AS TotalSales
            FROM CRM
            WHERE Status = 'CL'
            GROUP BY Consultant";

            var aggregatedSalesData = new List<AggregatedSalesData>();

            using (OdbcConnection connection = new OdbcConnection(_connectionString))
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
                catch (OdbcException ex)
                {
                    Console.WriteLine("OdbcException occurred: " + ex.Message);
                }
            }

            if (aggregatedSalesData == null || aggregatedSalesData.Count == 0)
            {
                return RedirectToAction("Error", "Home");
            }

            return View("Index", aggregatedSalesData);
        }

        public IActionResult getCountsInDb()
        {
            int siteCount = 0;
            int cancellationCount = 0;

            string siteCountQuery = "SELECT COUNT(*) FROM Sites";
            string cancellationCountQuery = "SELECT COUNT(*) FROM Cancellations";

            using (OdbcConnection connection = new OdbcConnection(_connectionString))
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
                    catch (OdbcException ex)
                    {
                        Console.WriteLine("OdbcException occurred: " + ex.Message);
                    }
                }
            }

            ViewData["SiteCount"] = siteCount;
            ViewData["CancellationCount"] = cancellationCount;
            return RedirectToAction("Index");
        }

        public IActionResult reasonForCancellation()
        {
            List<string> cancellationReasons = new List<string>();
            List<decimal> revenueLost = new List<decimal>();

            string query = "SELECT Reason, [Reduced Value ex vat] FROM Cancellations";

            using (OdbcConnection connection = new OdbcConnection(_connectionString))
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

            ViewData["CancellationReasons"] = cancellationReasons;
            ViewData["RevenueLost"] = revenueLost;
            return View("Index");
        }

        public IActionResult getCurrentYearRevenue()
        {
            int currentYear = DateTime.Now.Year;
            decimal totalRevenue = 0;
            List<decimal> totalRevenueList = new List<decimal>();

            string query = "SELECT SUM(TotalMonitoringFees) AS TotalRevenue FROM CRM WHERE liveyear = ?";

            using (OdbcConnection connection = new OdbcConnection(_connectionString))
            using (OdbcCommand command = new OdbcCommand(query, connection))
            {
                try
                {
                    command.Parameters.AddWithValue("liveyear", currentYear);
                    connection.Open();
                    var result = command.ExecuteScalar();

                    if (result != DBNull.Value)
                    {
                        totalRevenue = Convert.ToDecimal(result);
                    }

                    totalRevenueList.Add(totalRevenue);
                }
                catch (OdbcException ex)
                {
                    Console.WriteLine("OdbcException occurred: " + ex.Message);
                }
            }

            ViewData["TotalRevenue"] = totalRevenueList;
            return View("Index");
        }

        public IActionResult getSalesByRegion()
        {
            List<string> provinces = new List<string>();
            List<int> clientCounts = new List<int>();

            string query = "SELECT Province, COUNT(Client) AS ClientCount FROM CRM GROUP BY Province";

            using (OdbcConnection connection = new OdbcConnection(_connectionString))
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
            var leadSourceCounts = new Dictionary<string, int>();

            string query = @"
                SELECT LeadSource, COUNT(Client) AS ClientCount
                FROM CRM
                GROUP BY LeadSource";

            using (OdbcConnection connection = new OdbcConnection(_connectionString))
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
                }
                catch (OdbcException ex)
                {
                    // Handle exception
                }
            }

            ViewData["LeadSourceCounts"] = leadSourceCounts;
            return View("Index");
        }
    }
}