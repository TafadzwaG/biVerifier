using biVerifier.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.Odbc;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using System.Text.RegularExpressions;

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
            GetCityProbabilityData();
            QuotedRevenue();
            GetEnquiriesByCity();
            GetQuotedByMarket();
            return View();
        }

        // In your controller action
        public IActionResult GetQuotedByMarket()
        {
            var query = @"
        SELECT 
            Market, 
            COUNT(*) AS Count
        FROM Sales_Pipeline
        WHERE Probability = 'Quoted'
        GROUP BY Market
        ORDER BY Market";

            var marketData = new Dictionary<string, int>();

            using (OdbcConnection connection = new OdbcConnection(_connectionString))
            using (OdbcCommand command = new OdbcCommand(query, connection))
            {
                connection.Open();
                using (OdbcDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string market = reader["Market"].ToString();
                        if (string.IsNullOrEmpty(market))
                        {
                            market = "Unknown";
                        }
                        int count = Convert.ToInt32(reader["Count"]);
                        marketData[market] = count;
                    }
                }
            }

            ViewData["QuotedMarkets"] = marketData.Keys.ToList();
            ViewData["QuotedCounts"] = marketData.Values.ToList();

            return View("Index");
        }

        public IActionResult GetEnquiriesByCity()
        {
            var probabilityValues = new List<string>();

            using (var connection = new OdbcConnection(_connectionString))
            {
                connection.Open();

                string probabilityQuery = @"
            SELECT DISTINCT IIF(Probability IS NULL OR Probability = '', 'N/A', Probability) AS CleanProbability 
            FROM Sales_Pipeline 
            WHERE Probability IS NOT NULL AND Probability <> ''";

                using (var command = new OdbcCommand(probabilityQuery, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string value = reader["CleanProbability"].ToString();
                        if (!string.IsNullOrEmpty(value))
                        {
                            probabilityValues.Add(value);
                        }
                    }
                }

                // Add "N/A" manually if no entries were found
                if (!probabilityValues.Contains("N/A"))
                {
                    probabilityValues.Add("N/A");
                }
            }

            var query = @"
        SELECT 
            IIF(City IS NULL OR City = '', 'N/A', City) AS CleanCity,
            IIF(Probability IS NULL OR Probability = '', 'N/A', Probability) AS CleanProbability,
            COUNT(*) AS Count
        FROM Sales_Pipeline
        GROUP BY 
            IIF(City IS NULL OR City = '', 'N/A', City),
            IIF(Probability IS NULL OR Probability = '', 'N/A', Probability)
        ORDER BY 
            IIF(City IS NULL OR City = '', 'N/A', City),
            IIF(Probability IS NULL OR Probability = '', 'N/A', Probability)";

            var cityData = new Dictionary<string, Dictionary<string, int>>();

            using (var connection = new OdbcConnection(_connectionString))
            {
                connection.Open();

                using (var command = new OdbcCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string city = reader["CleanCity"].ToString();
                        string probability = reader["CleanProbability"].ToString();
                        int count = Convert.ToInt32(reader["Count"]);

                        if (!cityData.ContainsKey(city))
                        {
                            cityData[city] = new Dictionary<string, int>();
                        }

                        cityData[city][probability] = count;
                    }
                }
            }

            ViewData["EnquiryCities"] = cityData.Keys.OrderBy(c => c).ToList();
            ViewData["EnquiryTypes"] = probabilityValues.OrderBy(p => p).ToList();
            ViewData["EnquiryCounts"] = cityData;

            return View("Index");
        }




        public IActionResult QuotedRevenue()
        {
            decimal quotedRevenue = 0;

            string query = @"
        SELECT OnceOffCost 
        FROM Sales_Pipeline 
        WHERE Probability = 'Quoted'"; // Adjust this condition based on your actual probability values

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
                            string onceOffCost = reader["OnceOffCost"].ToString();

                            if (string.IsNullOrEmpty(onceOffCost) || onceOffCost == "R")
                                continue;

                            // Remove 'R' and any other non-numeric characters except digits and decimal point
                            string numericValue = Regex.Replace(onceOffCost, @"[^\d.]", "");
                            if (decimal.TryParse(numericValue, out decimal result))
                            {
                                quotedRevenue += result;
                            }
                        }
                    }
                }
                catch (OdbcException ex)
                {
                    Console.WriteLine("OdbcException occurred: " + ex.Message);
                }
            }

            ViewData["QuotedRevenue"] = quotedRevenue;
            return View("Index");
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


        public IActionResult GetCityProbabilityData()
        {
            var result = new Dictionary<string, Dictionary<string, int>>();

            string query = @"
        SELECT City, Probability, COUNT(*) as Count 
        FROM Sales_Pipeline 
        GROUP BY City, Probability
        ORDER BY City, Probability";

            using (OdbcConnection connection = new OdbcConnection(_connectionString))
            using (OdbcCommand command = new OdbcCommand(query, connection))
            {
                connection.Open();
                using (OdbcDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string city = reader["City"].ToString();
                        string probability = string.IsNullOrEmpty(reader["Probability"].ToString())
                            ? "Unnamed"
                            : reader["Probability"].ToString();
                        int count = Convert.ToInt32(reader["Count"]);

                        if (!result.ContainsKey(city))
                        {
                            result[city] = new Dictionary<string, int>();
                        }

                        result[city][probability] = count;
                    }
                }
            }

            // Convert to a format that's easier to work with in JavaScript
            var cities = result.Keys.ToList();
            var probabilities = result.Values.SelectMany(x => x.Keys).Distinct().ToList();

            var probabilityCounts = new Dictionary<string, List<int>>();
            foreach (var prob in probabilities)
            {
                probabilityCounts[prob] = new List<int>();
                foreach (var city in cities)
                {
                    probabilityCounts[prob].Add(result.ContainsKey(city) && result[city].ContainsKey(prob)
                        ? result[city][prob]
                        : 0);
                }
            }

            ViewData["Cities"] = cities;
            ViewData["ProbabilityCounts"] = probabilityCounts;

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