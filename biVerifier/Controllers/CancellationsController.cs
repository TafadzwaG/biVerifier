using biVerifier.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.OleDb;
using System.Data.Odbc;

namespace biVerifier.Controllers
{
    public class CancellationsController : Controller
    {
        public IActionResult Index()
        {

            string connectionString = @"Driver={Microsoft Access Driver (*.mdb, *.accdb)};DBQ=E:\CODING_HASHIRA\PROJECTS\.NET\databaseAccess\VERIFIER1.accdb;Persist Security Info=False;";

            string query = "SELECT * FROM Cancellations";

            var cancellationsDataList = new List<CancellationsData>();

            using (OdbcConnection connection = new OdbcConnection(connectionString))
            using (OdbcCommand command = new OdbcCommand(query, connection))

                try
                {
                    connection.Open();
                    using (OdbcDataReader reader = command.ExecuteReader())

                    {
                        while (reader.Read())
                        {
                            var cData = new CancellationsData();
                            cData.ID = (int)reader["ID"];
                            cData.Client = reader["Client"].ToString();
                            cData.Status = reader["Status"].ToString();
                            cData.Cancel_Billing = reader["Cancel_Billing"].ToString();
                            cData.Site = reader["Site"].ToString();
                            cData.Contact_Person = reader["Contact Person"].ToString();
                            cData.Account_Manager = reader["Account Manager"].ToString();
                            cancellationsDataList.Add(cData);
                        }
                    }
                }
                catch (OleDbException ex)
                {
                  Console.WriteLine("There was an error " + ex.Message);
                }

            return View(cancellationsDataList);
        }
    }
}
