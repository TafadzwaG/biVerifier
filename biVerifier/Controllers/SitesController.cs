using biVerifier.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.Common;
using System.Data.Odbc;
using System.Data.OleDb;

namespace biVerifier.Controllers
{
    public class SitesController : Controller
    {
        public IActionResult Index()
        {
            string connectionString = @"Driver={Microsoft Access Driver (*.mdb, *.accdb)};DBQ=C:\Users\Tafadzwag\Documents\DATABASE\VERIFIER2.accdb;Persist Security Info=False;";
            //string connectionString = @"Driver={Microsoft Access Driver (*.mdb, *.accdb)};DBQ=E:\CODING_HASHIRA\PROJECTS\.NET\databaseAccess\VERIFIER1.accdb;Persist Security Info=False;";

            string query = "SELECT * FROM Sites";
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
                            var cData = new SitesData();
                            cData.SiteID = reader["SiteID"].ToString();
                            cData.Client = reader["Client"].ToString();
                            cData.Contact_Person = reader["Contact_Person"].ToString();
                            cData.EmailAdd = reader["EmailAdd"].ToString();
                            cData.ContactNum = reader["ContactNum"].ToString();
                            cData.Num = reader["Num"].ToString();
                            cData.Street = reader["Street"].ToString();
                            cData.Suburb = reader["Suburb"].ToString();
                            cData.City = reader["City"].ToString();
                            cData.Region = reader["Region"].ToString();
                            cData.DNS_IP = reader["DNS _ IP"].ToString();
                            cData.IPv4 = reader["IPv4"].ToString();
                            cData.Server_Port = reader["Server Port"].ToString();
                            cData.HTTP_Port = reader["HTTP Port"].ToString();
                            cData.RTSP_Port = reader["RTSP Port"].ToString();
                            cData.Username = reader["Username"].ToString();
                            cData.Password = reader["Password"].ToString();
                            cData.No_Channels = reader["No Channels"].ToString();
                            cData.No_Channels_On_Analytics = reader["No Channels on analytics"].ToString();
                            cData.Monitoring_Platform = reader["Monitoring Platform"].ToString();
                            cData.GSM_Radio = reader["GSM Radio"].ToString();
                            cData.Alarm_Inputs = reader["Alarm Inputs"].ToString();
                            cData.Audio = reader["Audio"].ToString();
                            cData.SMTP_Server_Port = reader["SMTP Server Port"].ToString();
                            cData.Public_IP = reader["Public IP"].ToString();
                            cData.Notes = reader["Notes"].ToString();
                            sitesDataList.Add(cData);
                        }
                    }
                }
                catch (OdbcException ex)
                {
                    Console.WriteLine("There was an error " + ex.Message);
                }
            }

            return View(sitesDataList);
        }

    }
}