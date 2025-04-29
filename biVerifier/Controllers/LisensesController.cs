using biVerifier.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.Odbc;
using Microsoft.Extensions.Configuration;

namespace biVerifier.Controllers
{
    public class LisensesController : Controller
    {
        private readonly string _connectionString;

        public LisensesController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("CrmDb");
        }

        public IActionResult Index()
        {
            string query = "SELECT * FROM Licenses";
            var licensesDataList = new List<Licenses>();

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
                            var licencesData = new Licenses
                            {
                                Client = reader["Client"].ToString(),
                                Requestor = reader["Requestor"].ToString(),
                                ChangeDate = reader["ChangeDate"].ToString(),
                                ChangeCode = reader["ChangeCode"].ToString(),
                                CurrentAI = reader["CurrentAI"].ToString(),
                                NewAI = reader["NewAI"].ToString(),
                                LicensesNo = reader["LicensesNo"].ToString(),
                                Cost = reader["Cost"].ToString(),
                                ChangeNotes = reader["Change Notes"].ToString()
                            };

                            licensesDataList.Add(licencesData);
                        }
                    }
                }
                catch (OdbcException ex)
                {
                    Console.WriteLine("There was an error " + ex.Message);
                }
            }

            return View(licensesDataList);
        }

        public IActionResult Search(string searchTerm)
        {
            string query = "SELECT * FROM Licenses WHERE Client LIKE ? OR Requestor LIKE ? OR ChangeDate LIKE ? " +
                         "OR ChangeCode LIKE ? OR CurrentAI LIKE ? OR NewAI LIKE ? OR LicensesNo LIKE ? " +
                         "OR Cost LIKE ? OR [Change Notes] LIKE ?";

            var licensesDataList = new List<Licenses>();

            using (OdbcConnection connection = new OdbcConnection(_connectionString))
            using (OdbcCommand command = new OdbcCommand(query, connection))
            {
                // Add parameters for each search term
                for (int i = 0; i < 9; i++)
                {
                    command.Parameters.AddWithValue("@SearchTerm", "%" + searchTerm + "%");
                }

                try
                {
                    connection.Open();
                    using (OdbcDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var licencesData = new Licenses
                            {
                                Client = reader["Client"].ToString(),
                                Requestor = reader["Requestor"].ToString(),
                                ChangeDate = reader["ChangeDate"].ToString(),
                                ChangeCode = reader["ChangeCode"].ToString(),
                                CurrentAI = reader["CurrentAI"].ToString(),
                                NewAI = reader["NewAI"].ToString(),
                                LicensesNo = reader["LicensesNo"].ToString(),
                                Cost = reader["Cost"].ToString(),
                                ChangeNotes = reader["Change Notes"].ToString()
                            };

                            licensesDataList.Add(licencesData);
                        }
                    }
                }
                catch (OdbcException ex)
                {
                    Console.WriteLine("There was an error " + ex.Message);
                }
            }

            ViewBag.SearchTerm = searchTerm;
            return View("Index", licensesDataList);
        }
    }
}