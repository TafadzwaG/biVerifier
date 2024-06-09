using biVerifier.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.Odbc;
using System.Collections.Generic;

namespace biVerifier.Controllers
{
    public class SitesController : Controller
    {
        private readonly string connectionString = @"Driver={Microsoft Access Driver (*.mdb, *.accdb)};DBQ=E:\CODING_HASHIRA\PROJECTS\.NET\databaseAccess\VERIFIER2.accdb;Persist Security Info=False;";
        //private readonly string connectionString = @"Driver={Microsoft Access Driver (*.mdb, *.accdb)};DBQ=C:\Users\CRM Server\Documents\veriDB\VERIFIER2.accdb;Persist Security Info=False;";
        public IActionResult Index(string searchTerm = null)
        {
            var sitesDataList = string.IsNullOrEmpty(searchTerm) ? GetAllSitesData() : SearchSitesData(searchTerm);
            ViewBag.SearchTerm = searchTerm;
            return View(sitesDataList);
        }

        private List<SitesData> GetAllSitesData()
        {
            string query = "SELECT * FROM Sites";
            return ExecuteQuery(query);
        }

        private List<SitesData> SearchSitesData(string searchTerm)
        {
            string query = $"SELECT * FROM Sites WHERE Client LIKE '%{searchTerm}%' OR City LIKE '%{searchTerm}%'";
            return ExecuteQuery(query);
        }

        private List<SitesData> ExecuteQuery(string query)
        {
            var sitesDataList = new List<SitesData>();

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
                            var cData = new SitesData
                            {
                                SiteID = reader["SiteID"].ToString(),
                                Client = reader["Client"].ToString(),
                                Contact_Person = reader["Contact_Person"].ToString(),
                                EmailAdd = reader["EmailAdd"].ToString(),
                                ContactNum = reader["ContactNum"].ToString(),
                                Num = reader["Num"].ToString(),
                                Street = reader["Street"].ToString(),
                                Suburb = reader["Suburb"].ToString(),
                                City = reader["City"].ToString(),
                                Region = reader["Region"].ToString(),
                                DNS_IP = reader["DNS _ IP"].ToString(),
                                IPv4 = reader["IPv4"].ToString(),
                                Server_Port = reader["Server Port"].ToString(),
                                HTTP_Port = reader["HTTP Port"].ToString(),
                                RTSP_Port = reader["RTSP Port"].ToString(),
                                Username = reader["Username"].ToString(),
                                Password = reader["Password"].ToString(),
                                No_Channels = reader["No Channels"].ToString(),
                                No_Channels_On_Analytics = reader["No Channels on analytics"].ToString(),
                                Monitoring_Platform = reader["Monitoring Platform"].ToString(),
                                GSM_Radio = reader["GSM Radio"].ToString(),
                                Alarm_Inputs = reader["Alarm Inputs"].ToString(),
                                Audio = reader["Audio"].ToString(),
                                SMTP_Server_Port = reader["SMTP Server Port"].ToString(),
                                Public_IP = reader["Public IP"].ToString(),
                                Notes = reader["Notes"].ToString()
                            };
                            sitesDataList.Add(cData);
                        }
                    }
                }
                catch (OdbcException ex)
                {
                    Console.WriteLine("There was an error " + ex.Message);
                }
            }

            return sitesDataList;
        }
    }
}
