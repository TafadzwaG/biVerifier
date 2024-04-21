using biVerifier.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.Odbc;
using System.Data.OleDb;

namespace biVerifier.Controllers
{
    public class TechCancelController : Controller
    {
        public IActionResult Index()
        {

            string connectionString = @"Driver={Microsoft Access Driver (*.mdb, *.accdb)};DBQ=E:\CODING_HASHIRA\PROJECTS\.NET\databaseAccess\VERIFIER1.accdb;Persist Security Info=False;";

            string query = "SELECT * FROM TechCancel";

            var techCancelDataList = new List<TechCancelData>();

            using (OdbcConnection connection = new OdbcConnection(connectionString))
            using (OdbcCommand command = new OdbcCommand(query, connection))

                try
                {
                    connection.Open();
                    using (OdbcDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var cData = new TechCancelData();
                            cData.ID = reader["ID"] != DBNull.Value ? Convert.ToInt32(reader["ID"]) : null;
                            cData.ClientID = reader["ClientID"].ToString();

                            techCancelDataList.Add(cData);
                        }
                    }
                }
                catch (OdbcException ex)
                {
                    Console.WriteLine("There was an error " + ex.Message);
                }

            return View(techCancelDataList);
        }

       
    }
}
