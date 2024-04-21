using biVerifier.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.OleDb;

namespace biVerifier.Controllers
{
    public class CancellationsController : Controller
    {
        public IActionResult Index()
        {

            string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=E:\CODING_HASHIRA\PROJECTS\.NET\databaseAccess\VERIFIER1.accdb;Persist Security Info=False;";

            string query = "SELECT * FROM Cancellations";

            var cancellationsDataList = new List<CancellationsData>();

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            using (OleDbCommand command = new OleDbCommand(query, connection))

                try
                {
                    connection.Open();
                    using (OleDbDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var cData = new CancellationsData();
                            cData.ID = (int)reader["ID"];
                            cData.Client = reader["Client"].ToString();
                            cData.Status = reader["Status"].ToString();
                            cData.Cancel_Billing = reader["Cancel_Billing"].ToString();
                            cancellationsDataList.Add(cData);
                        }
                    }
                }
                catch (OleDbException ex)
                {
                    // Handle exception
                }

            return View(cancellationsDataList);
        }
    }
}
