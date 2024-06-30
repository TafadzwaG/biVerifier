using biVerifier.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.Odbc;
using System.Data.OleDb;

namespace biVerifier.Controllers
{
    public class LisensesController : Controller
    {
        public IActionResult Index()
        {
            //string connectionString = @"Driver={Microsoft Access Driver (*.mdb, *.accdb)};DBQ=E:\CODING_HASHIRA\PROJECTS\.NET\databaseAccess\VERIFIER2.accdb;Persist Security Info=False;";
            string connectionString = @"Driver={Microsoft Access Driver (*.mdb, *.accdb)};DBQ=C:\Users\CRM Server\Documents\veriDB\VERIFIER2.accdb;Persist Security Info=False;";

            // SQL query to execute


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
                            var licencesData = new Licenses();

                            // Check if ID is DBNull
                            //if (reader["ID"] != DBNull.Value)
                            //{
                            //    licencesData.ID = (int)reader["ID"];
                            //}
                            //else
                            //{
                            //    // Assign a default value or handle the case appropriately
                            //    licencesData.ID = -1; // Or any other default value
                            //}

                            licencesData.Client = reader["Client"].ToString();
                            licencesData.Requestor = reader["Requestor"].ToString();
                            licencesData.ChangeDate = reader["ChangeDate"].ToString();
                            licencesData.ChangeCode = reader["ChangeCode"].ToString();
                            licencesData.CurrentAI = reader["CurrentAI"].ToString();
                            licencesData.NewAI = reader["NewAI"].ToString();
                            licencesData.LicensesNo = reader["LicensesNo"].ToString();
                            licencesData.Cost = reader["Cost"].ToString();
                            licencesData.ChangeNotes = reader["Change Notes"].ToString();

                            // Assign other properties as needed
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
            //string connectionString = @"Driver={Microsoft Access Driver (*.mdb, *.accdb)};DBQ=E:\CODING_HASHIRA\PROJECTS\.NET\databaseAccess\VERIFIER2.accdb;Persist Security Info=False;";
            string connectionString = @"Driver={Microsoft Access Driver (*.mdb, *.accdb)};DBQ=C:\Users\CRM Server\Documents\veriDB\VERIFIER2.accdb;Persist Security Info=False;";
            string query = "SELECT * FROM Licenses WHERE Client LIKE ? OR Requestor LIKE ? OR ChangeDate LIKE ? OR ChangeCode LIKE ? OR CurrentAI LIKE ? OR NewAI LIKE ? OR LicensesNo LIKE ? OR Cost LIKE ? OR [Change Notes] LIKE ?";

            var licensesDataList = new List<Licenses>();

            using (OdbcConnection connection = new OdbcConnection(connectionString))
            using (OdbcCommand command = new OdbcCommand(query, connection))
            {
                command.Parameters.AddWithValue("@SearchTerm", "%" + searchTerm + "%");

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

            return View("Index", licensesDataList);
        }
    }
}
