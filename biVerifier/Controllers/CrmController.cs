using Microsoft.AspNetCore.Mvc;
using System.Data.OleDb;
using biVerifier.Models;
using System.Data.Odbc;


namespace biVerifier.Controllers
{
    public class CrmController : Controller
    {

        public IActionResult Index()
        {

            string connectionString = @"Driver={Microsoft Access Driver (*.mdb, *.accdb)};DBQ=E:\CODING_HASHIRA\PROJECTS\.NET\databaseAccess\VERIFIER1.accdb;Persist Security Info=False;";

            // SQL query to execute


            string query = "SELECT * FROM CRM";


            var crmDataList = new List<CRMData>();

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
                            var crmData = new CRMData();
                            crmData.ClientID = (int)reader["ClientID"];
                            crmData.Sites = reader["Sites"].ToString();
                            crmData.Contact_Person = reader["Contact_Person"].ToString();
                            crmData.EmailAdd = reader["EmailAdd"].ToString();
                            crmData.Contact_Num = reader["ContactNum"].ToString();
                            crmData.Num = reader["Num"].ToString();
                            crmData.Street = reader["Street"].ToString();
                            crmData.Suburb = reader["Suburb"].ToString();
                            crmData.City = reader["City"].ToString();
                            crmData.Region = reader["Region"].ToString();
                            crmData.Lead_Source = reader["Lead_Source"].ToString();
                            crmData.Service_Type = reader["Service_Type"].ToString();
                            crmData.Market_Type = reader["Market_Type"].ToString();
                            crmData.Category = reader["Category"].ToString();
                            crmData.Consultant = reader["Consultant"].ToString();
                            crmData.Branch = reader["Branch"].ToString();
                            crmData.Status = reader["Status"].ToString();
                            crmData.Probability = reader["Probability"].ToString();
                            crmData.LEAD_Year = reader["LEAD_Year"].ToString();
                            crmData.LEAD_Month = reader["LEAD_Month"].ToString();
                            crmData.LIVE_Year = reader["LIVE_Year"].ToString();
                            crmData.LIVE_Month = reader["LIVE_Month"].ToString();
                            crmData.GandTotal = reader["GandTotal"].ToString();
                            crmData.Onceoffsetupcosts = reader["Onceoffsetupcosts"].ToString();
                            crmData.Install_Comm = reader["Install_Comm"].ToString();

                            // Assign other properties as needed
                            crmDataList.Add(crmData);
                        }
                    }
                }
                catch (OleDbException ex)
                {
                    Console.WriteLine("There was an error " + ex.Message);
                }
            }

            return View(crmDataList);
        }
        public IActionResult RetrieveCrmData()
        {

            string connectionString = @"Driver={Microsoft Access Driver (*.mdb, *.accdb)};DBQ=E:\CODING_HASHIRA\PROJECTS\.NET\databaseAccess\VERIFIER1.accdb;Persist Security Info=False;";

            // SQL query to execute


            string query = "SELECT * FROM CRM";


            var crmDataList = new List<CRMData>();

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
                            var crmData = new CRMData();
                            crmData.Sites = reader["Sites"].ToString();
                            crmData.Suburb = reader["Suburb"].ToString();
                            // Assign other properties as needed
                            crmDataList.Add(crmData);
                        }
                    }
                }
                catch (OleDbException ex)
                {
                    Console.WriteLine("There was an error " + ex.Message);
                }
            }

            return View(crmDataList);
        }


        public IActionResult FilterByProvince()
        {
            string connectionString = @"Driver={Microsoft Access Driver (*.mdb, *.accdb)};DBQ=E:\CODING_HASHIRA\PROJECTS\.NET\databaseAccess\VERIFIER1.accdb;Persist Security Info=False;";

            string query = "SELECT * FROM CRM WHERE Region = 'GP'";

            var crmDataList = new List<CRMData>();

            using (OdbcConnection connection = new OdbcConnection(connectionString))
            using (OdbcCommand command = new OdbcCommand(query, connection))


                try
                {
                    connection.Open();
                    using (OdbcDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var crmData = new CRMData();
                            crmData.Sites = reader["Sites"].ToString();
                            crmData.Suburb = reader["Suburb"].ToString();
                            // Assign other properties as needed
                            crmDataList.Add(crmData);
                        }
                    }
                }
                catch (OleDbException ex)
                {
                    Console.WriteLine("There was an error " + ex.Message);
                }

            return View(crmDataList);
        }


       

       
       


    }
}
