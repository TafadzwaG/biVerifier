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

            string connectionString = @"Driver={Microsoft Access Driver (*.mdb, *.accdb)};DBQ=E:\CODING_HASHIRA\PROJECTS\.NET\databaseAccess\VERIFIER2.accdb;Persist Security Info=False;";

            // SQL query to execute


            string query = "SELECT * FROM CRM";


            var crmDataList = new List<Crm>();

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
                            var crmData = new Crm();
                            crmData.ID = (int)reader["ID"];
                            crmData.Client = reader["Client"].ToString();
                            crmData.Contact_Person = reader["Contact_Person"].ToString();
                            crmData.Email = reader["Email"].ToString();
                            crmData.Contact_Number = reader["Contact_Number"].ToString();
                            crmData.No = reader["No"].ToString();
                            crmData.Street = reader["Street"].ToString();
                            crmData.Suburb = reader["Suburb"].ToString();
                            crmData.City = reader["City"].ToString();
                            crmData.Province = reader["Province"].ToString();
                            crmData.LeadSource = reader["LeadSource"].ToString();
                            crmData.Service = reader["Service"].ToString();
                            crmData.Market = reader["Market"].ToString();
                            crmData.Category = reader["Category"].ToString();
                            crmData.Consultant = reader["Consultant"].ToString();
                            crmData.Branch = reader["Branch"].ToString();
                            crmData.Status = reader["Status"].ToString();
                            crmData.Probability = reader["Probability"].ToString();
                            crmData.leadyear = reader["leadyear"].ToString();
                            crmData.leadmonth = reader["leadmonth"].ToString();
                            crmData.liveyear = reader["liveyear"].ToString();
                            crmData.livemonth = reader["livemonth"].ToString();
                            crmData.TotalMonitoringFees = reader["TotalMonitoringFees"].ToString();
                            crmData.Onceoffsetupcosts = reader["Onceoffsetupcosts"].ToString();
                            crmData.Install_Comm = reader["Install_Comm"].ToString();
                            crmData.ManagedServicesFees = reader["ManagedServicesFees"].ToString();

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
