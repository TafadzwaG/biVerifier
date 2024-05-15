using biVerifier.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.OleDb;
using System.Data.Odbc;


using Microsoft.AspNetCore.Authorization;

namespace biVerifier.Controllers
{
    [Authorize(Roles = "Management, Sales")]
    public class CancellationsController : Controller
    {
        public IActionResult Index()
        {

            string connectionString = @"Driver={Microsoft Access Driver (*.mdb, *.accdb)};DBQ=E:\CODING_HASHIRA\PROJECTS\.NET\databaseAccess\VERIFIER2.accdb;Persist Security Info=False;";

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
                            cData.Cancellation_Month = reader["Cancellation Month"].ToString();
                            cData.Cancellation_Received_Date = reader["Cancellation received Date"].ToString();
                            cData.Cancellation_End_Date = reader["Cancellation End date"].ToString();
                            cData.Reason = reader["Reason"].ToString();
                            cData.Notes = reader["Notes"].ToString();
                            cData.Reduced_Value_Ex_Vat = reader["Reduced Value ex vat"].ToString();
                            cData.TechResponsible = reader["TechResponsible"].ToString();
                            cData.Total_Channels = reader["Total_channels"].ToString();
                            cData.Platform = reader["Platform"].ToString();
                            cData.Cancel_GSM = reader["Cancel_GSM"].ToString();
                            cData.Cancel_DNS = reader["Cancel_DNS"].ToString();
                            cData.Cancel_LPR_Licenses = reader["Cancel_LPR_Licenses"].ToString();
                            cData.Cancel_Video_Analytics_Licenses = reader["Cancel_Video_Analytics_Licenses"].ToString() ;
                            cData.Cancel_Internet_Connectivity = reader["Cancel_Internet_Connectivity"].ToString();
                            cData.Cancel_Billing = reader["Cancel_Billing"].ToString();
                            cData.Status = reader["Status"].ToString();

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
