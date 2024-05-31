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

            string connectionString = @"Driver={Microsoft Access Driver (*.mdb, *.accdb)};DBQ=C:\Users\Tafadzwag\Documents\DATABASE\VERIFIER2.accdb;Persist Security Info=False;";
            //string connectionString = @"Driver={Microsoft Access Driver (*.mdb, *.accdb)};DBQ=E:\CODING_HASHIRA\PROJECTS\.NET\databaseAccess\VERIFIER2.accdb;Persist Security Info=False;";

            string query = "SELECT * FROM TechCancel";

            var techCancelDataList = new List<TechCancel>();

            using (OdbcConnection connection = new OdbcConnection(connectionString))
            using (OdbcCommand command = new OdbcCommand(query, connection))

                try
                {
                    connection.Open();
                    using (OdbcDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var cData = new TechCancel();
                            cData.Client = reader["Client"].ToString();
                            cData.SiteID = reader["SiteID"].ToString();
                            cData.Date = reader["Date"].ToString();
                            cData.TechResponsible = reader["TechResponsible"].ToString();
                            cData.Total_Channels = reader["Total_channels"] != DBNull.Value ? Convert.ToInt32(reader["Total_channels"]) : null;
                            cData.Platform = reader["Platform"].ToString();
                            cData.Cancel_GSM = reader["Cancel_GSM"].ToString();
                            cData.Cancel_DNS = reader["Cancel_DNS"].ToString();
                            cData.Cancel_LPR_Licenses = reader["Cancel_LPR_Licenses"].ToString();
                            cData.Cancel_Video_Analytics_Licenses = reader["Cancel_Internet_Connectivity"].ToString();
                            cData.Cancel_Billing = reader["Cancel_Billing"].ToString();
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
