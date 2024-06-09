using biVerifier.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.Odbc;
using System.Data.OleDb;

namespace biVerifier.Controllers
{
    public class LicencesController : Controller
    {
        public IActionResult Index()
        {
            string connectionString = @"Driver={Microsoft Access Driver (*.mdb, *.accdb)};DBQ=E:\CODING_HASHIRA\PROJECTS\.NET\databaseAccess\VERIFIER2.accdb;Persist Security Info=False;";
            string query = "SELECT * FROM Licenses";

            var licensesDataList = new List<Licenses>();

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
                catch (OleDbException ex)
                {
                    Console.WriteLine("There was an error " + ex.Message);
                }
            }

            return View(licensesDataList);
        }

        public IActionResult Search(string searchTerm)
        {
            string connectionString = @"Driver={Microsoft Access Driver (*.mdb, *.accdb)};DBQ=E:\CODING_HASHIRA\PROJECTS\.NET\databaseAccess\VERIFIER2.accdb;Persist Security Info=False;";
            string query = @"
                SELECT * FROM Licenses 
                WHERE Client LIKE ? OR Requestor LIKE ? OR ChangeDate LIKE ? 
                OR ChangeCode LIKE ? OR CurrentAI LIKE ? OR NewAI LIKE ? 
                OR LicensesNo LIKE ? OR Cost LIKE ? OR [Change Notes] LIKE ?";

            var licensesDataList = new List<Licenses>();

            using (OdbcConnection connection = new OdbcConnection(connectionString))
            using (OdbcCommand command = new OdbcCommand(query, connection))
            {
                for (int i = 0; i < 9; i++)
                {
                    command.Parameters.AddWithValue($"@SearchTerm{i}", "%" + searchTerm + "%");
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
                catch (OleDbException ex)
                {
                    Console.WriteLine("There was an error " + ex.Message);
                }
            }

            ViewBag.SearchTerm = searchTerm;
            return View("Index", licensesDataList);
        }
    }
}
