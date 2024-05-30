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
            string connectionString = @"Driver={Microsoft Access Driver (*.mdb, *.accdb)};DBQ=C:\Users\Tafadzwag\Documents\DATABASE\VERIFIER2.accdb;Persist Security Info=False;";
            //string connectionString = @"Driver={Microsoft Access Driver (*.mdb, *.accdb)};DBQ=E:\CODING_HASHIRA\PROJECTS\.NET\databaseAccess\VERIFIER1.accdb;Persist Security Info=False;";

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
    }
}
