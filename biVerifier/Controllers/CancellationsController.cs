using biVerifier.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.Odbc;
using Microsoft.AspNetCore.Authorization;

namespace biVerifier.Controllers
{
    [Authorize(Roles = "Management, Sales")]
    [Route("[controller]")]
    public class CancellationsController : Controller
    {
        [HttpGet]
        [Route("")]
        [Route("Index")]
        public IActionResult Index(string searchTerm)
        {
            string connectionString = @"Driver={Microsoft Access Driver (*.mdb, *.accdb)};DBQ=C:\Users\Tafadzwag\Documents\DATABASE\VERIFIER2.accdb;Persist Security Info=False;";
            var cancellationsDataList = new List<CancellationsData>();

            string query = "SELECT * FROM Cancellations";
            if (!string.IsNullOrEmpty(searchTerm))
            {
                query += " WHERE Client LIKE ? OR [Contact Person] LIKE ?";
            }

            using (OdbcConnection connection = new OdbcConnection(connectionString))
            using (OdbcCommand command = new OdbcCommand(query, connection))
            {
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    command.Parameters.AddWithValue("@Client", "%" + searchTerm + "%");
                    command.Parameters.AddWithValue("@ContactPerson", "%" + searchTerm + "%");
                }

                try
                {
                    connection.Open();
                    using (OdbcDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var cData = new CancellationsData
                            {
                                ID = (int)reader["ID"],
                                Client = reader["Client"].ToString(),
                                Status = reader.IsDBNull(reader.GetOrdinal("Status")) ? string.Empty : reader["Status"].ToString(),
                                Cancel_Billing = reader.IsDBNull(reader.GetOrdinal("Cancel_Billing")) ? string.Empty : reader["Cancel_Billing"].ToString(),
                                Site = reader["Site"].ToString(),
                                Contact_Person = reader["Contact Person"].ToString(),
                                Account_Manager = reader["Account Manager"].ToString(),
                                Cancellation_Month = reader["Cancellation Month"].ToString(),
                                Cancellation_Received_Date = reader["Cancellation received Date"].ToString(),
                                Cancellation_End_Date = reader["Cancellation End date"].ToString(),
                                Reason = reader["Reason"].ToString(),
                                Notes = reader["Notes"].ToString(),
                                Reduced_Value_Ex_Vat = reader["Reduced Value ex vat"].ToString(),
                                TechResponsible = reader["TechResponsible"].ToString(),
                                Total_Channels = reader["Total_channels"].ToString(),
                                Platform = reader["Platform"].ToString(),
                                Cancel_GSM = reader["Cancel_GSM"].ToString(),
                                Cancel_DNS = reader["Cancel_DNS"].ToString(),
                                Cancel_LPR_Licenses = reader["Cancel_LPR_Licenses"].ToString(),
                                Cancel_Video_Analytics_Licenses = reader["Cancel_Video_Analytics_Licenses"].ToString(),
                                Cancel_Internet_Connectivity = reader["Cancel_Internet_Connectivity"].ToString()
                            };

                            cancellationsDataList.Add(cData);
                        }
                    }
                }
                catch (OdbcException ex)
                {
                    Console.WriteLine("There was an error " + ex.Message);
                }
            }

            ViewBag.SearchTerm = searchTerm;
            return View(cancellationsDataList);
        }
    }
}
