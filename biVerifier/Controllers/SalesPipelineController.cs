using biVerifier.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data.Odbc;
using System.Text.RegularExpressions;

namespace biVerifier.Controllers
{
    public class SalesPipelineController : Controller
    {
        private readonly string _connectionString;

        public SalesPipelineController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("CrmDb");
        }

        public IActionResult Index()
        {
            string query = "SELECT * FROM Sales_Pipeline";
            var salesPipelineList = ExecuteQuery(query, null);
            return View(salesPipelineList);
        }

        public IActionResult Search(string searchTerm)
        {
            string query = "SELECT * FROM Sales_Pipeline WHERE Client LIKE ?";
            var parameters = new[] { "%" + searchTerm + "%" };
            var salesPipelineList = ExecuteQuery(query, parameters);
            return View("Index", salesPipelineList);
        }

        public IActionResult FilterByDateRange(DateTime startDate, DateTime endDate)
        {
            string query = "SELECT * FROM Sales_Pipeline WHERE leadmonth BETWEEN ? AND ?";
            var parameters = new object[] { startDate, endDate };
            var salesPipelineList = ExecuteQuery(query, parameters);
            return View("Index", salesPipelineList);
        }

        public IActionResult FilterBySalesPerson(string salesPerson)
        {
            string query = "SELECT * FROM Sales_Pipeline WHERE Consultant = ?";
            var parameters = new[] { salesPerson };
            var salesPipelineList = ExecuteQuery(query, parameters);
            return View("Index", salesPipelineList);
        }

        public IActionResult FilterByProvince(string province)
        {
            string query = "SELECT * FROM Sales_Pipeline WHERE City = ?";
            var parameters = new[] { province };
            var salesPipelineList = ExecuteQuery(query, parameters);
            return View("Index", salesPipelineList);
        }

        private List<SalesPipeline> ExecuteQuery(string query, object[] parameters)
        {
            var salesPipelineList = new List<SalesPipeline>();

            using (var connection = new OdbcConnection(_connectionString))
            using (var command = new OdbcCommand(query, connection))
            {
                if (parameters != null)
                {
                    foreach (var param in parameters)
                    {
                        command.Parameters.AddWithValue("?", param);
                    }
                }

                try
                {
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            salesPipelineList.Add(new SalesPipeline
                            {
                                QuoteNum = Convert.ToInt32(reader["QuoteNum"]),
                                Client = reader["Client"].ToString(),
                                Lead_Source = reader["LeadSource"].ToString(),
                                Contact_Person = reader["Contact_Person"].ToString(),
                                Email = reader["Email"].ToString(),
                                City = reader["City"].ToString(),
                                Service = reader["Service"].ToString(),
                                Lead_Month = reader["leadmonth"].ToString(),
                                Consultant = reader["Consultant"].ToString(),
                                OnceOffCost = reader["OnceOffCost"].ToString(),
                                RecurringCost = reader["RecurringCost"].ToString(),
                                Probability = reader["Probability"].ToString(),
                                Market = reader["Market"].ToString(),
                                SageQuote = reader["SageQuote"].ToString()
                            });
                        }
                    }
                }
                catch (OdbcException ex)
                {
                    Console.WriteLine("There was an error: " + ex.Message);
                }
            }

            return salesPipelineList;
        }


        public IActionResult FilterByEnquiries(string leadMonth)
        {
            string query;
            object[] parameters;

            // Check if the leadMonth is a month name (Jan, Feb, etc.)
            if (Regex.IsMatch(leadMonth, @"^(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)", RegexOptions.IgnoreCase))
            {
                query = "SELECT * FROM Sales_Pipeline WHERE Probability IS NOT NULL AND (leadmonth LIKE ? OR leadmonth LIKE ? OR leadmonth LIKE ?)";
                parameters = new object[]
                {
            leadMonth + "%",
            "%/" + leadMonth + "%",
            "% " + leadMonth + "%"
                };
            }
            else
            {
                // Try to parse as date
                if (DateTime.TryParse(leadMonth, out var date))
                {
                    query = "SELECT * FROM Sales_Pipeline WHERE Probability IS NOT NULL AND leadmonth = ?";
                    parameters = new object[] { date.ToString("yyyy/MM/dd") };
                }
                else
                {
                    // Fallback to simple contains if format is unknown
                    query = "SELECT * FROM Sales_Pipeline WHERE Probability IS NOT NULL AND leadmonth LIKE ?";
                    parameters = new object[] { "%" + leadMonth + "%" };
                }
            }

            var salesPipelineList = ExecuteQuery(query, parameters);

            // Pass the count to the view
            ViewBag.EnquiryCount = salesPipelineList.Count;
            ViewBag.FilteredMonth = leadMonth;

            return View("Index", salesPipelineList);
        }
    }
}
