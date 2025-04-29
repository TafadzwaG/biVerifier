using biVerifier.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data.Odbc;

namespace biVerifier.Controllers
{
    [Route("[controller]")]
    public class TechCancelController : Controller
    {
        private readonly string _connectionString;

        public TechCancelController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("CrmDb");
        }

        [HttpGet]
        public IActionResult Index(string searchTerm)
        {
            string query = "SELECT * FROM TechCancel";
            var parameters = new List<OdbcParameter>();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query += " WHERE Client LIKE ? OR SiteID LIKE ? OR Date LIKE ? OR TechResponsible LIKE ?";
                string searchPattern = $"%{searchTerm}%";
                parameters.Add(new OdbcParameter("Client", searchPattern));
                parameters.Add(new OdbcParameter("SiteID", searchPattern));
                parameters.Add(new OdbcParameter("Date", searchPattern));
                parameters.Add(new OdbcParameter("TechResponsible", searchPattern));
            }

            var techCancelDataList = new List<TechCancel>();

            using (OdbcConnection connection = new OdbcConnection(_connectionString))
            using (OdbcCommand command = new OdbcCommand(query, connection))
            {
                foreach (var param in parameters)
                {
                    command.Parameters.Add(param);
                }

                try
                {
                    connection.Open();
                    using (OdbcDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var cData = new TechCancel
                            {
                                Client = reader["Client"].ToString(),
                                SiteID = reader["SiteID"].ToString(),
                                Date = reader["Date"].ToString(),
                                TechResponsible = reader["TechResponsible"].ToString(),
                                Total_Channels = reader["Total_channels"] != DBNull.Value ? Convert.ToInt32(reader["Total_channels"]) : (int?)null,
                                Platform = reader["Platform"].ToString(),
                                Cancel_GSM = reader["Cancel_GSM"].ToString(),
                                Cancel_DNS = reader["Cancel_DNS"].ToString(),
                                Cancel_LPR_Licenses = reader["Cancel_LPR_Licenses"].ToString(),
                                Cancel_Video_Analytics_Licenses = reader["Cancel_Video_Analytics_Licenses"].ToString(),
                                Cancel_Internet_Connectivity = reader["Cancel_Internet_Connectivity"].ToString(),
                                Cancel_Billing = reader["Cancel_Billing"].ToString()
                            };
                            techCancelDataList.Add(cData);
                        }
                    }
                }
                catch (OdbcException ex)
                {
                    Console.WriteLine("There was an error: " + ex.Message);
                }
            }

            ViewBag.SearchTerm = searchTerm;
            return View(techCancelDataList);
        }
    }
}
