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
            //string connectionString = @"Driver={Microsoft Access Driver (*.mdb, *.accdb)};DBQ=C:\Users\CRM Server\Documents\veriDB\VERIFIER2.accdb;Persist Security Info=False;";

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
                            crmData.Comments = reader["Comments"].ToString();
                            crmData.Sage = reader["Sage"].ToString();

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

        public IActionResult SearchIndex(string searchString)
        {
            string connectionString = @"Driver={Microsoft Access Driver (*.mdb, *.accdb)};DBQ=E:\CODING_HASHIRA\PROJECTS\.NET\databaseAccess\VERIFIER2.accdb;Persist Security Info=False;";
            //string connectionString = @"Driver={Microsoft Access Driver (*.mdb, *.accdb)};DBQ=C:\Users\CRM Server\Documents\veriDB\VERIFIER2.accdb;Persist Security Info=False;";
            string query = "SELECT * FROM CRM";


            if (!string.IsNullOrEmpty(searchString))
            {
                query += " WHERE Client LIKE ? OR Contact_Person LIKE ? OR Email LIKE ?";
            }

            var crmDataList = new List<Crm>();

            using (OdbcConnection connection = new OdbcConnection(connectionString))
            using (OdbcCommand command = new OdbcCommand(query, connection))
            {
                if (!string.IsNullOrEmpty(searchString))
                {
                    command.Parameters.AddWithValue("Client", "%" + searchString + "%");
                    command.Parameters.AddWithValue("Contact_Person", "%" + searchString + "%");
                    command.Parameters.AddWithValue("Email", "%" + searchString + "%");
                }

                try
                {
                    connection.Open();
                    using (OdbcDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var crmData = new Crm
                            {
                                ID = (int)reader["ID"],
                                Client = reader["Client"].ToString(),
                                Contact_Person = reader["Contact_Person"].ToString(),
                                Email = reader["Email"].ToString(),
                                Contact_Number = reader["Contact_Number"].ToString(),
                                No = reader["No"].ToString(),
                                Street = reader["Street"].ToString(),
                                Suburb = reader["Suburb"].ToString(),
                                City = reader["City"].ToString(),
                                Province = reader["Province"].ToString(),
                                LeadSource = reader["LeadSource"].ToString(),
                                Service = reader["Service"].ToString(),
                                Market = reader["Market"].ToString(),
                               
                                Consultant = reader["Consultant"].ToString(),
                                Branch = reader["Branch"].ToString(),
                                Status = reader["Status"].ToString(),
                                Probability = reader["Probability"].ToString(),
                                leadyear = reader["leadyear"].ToString(),
                                leadmonth = reader["leadmonth"].ToString(),
                                liveyear = reader["liveyear"].ToString(),
                                livemonth = reader["livemonth"].ToString(),
                                TotalMonitoringFees = reader["TotalMonitoringFees"].ToString(),
                                Onceoffsetupcosts = reader["Onceoffsetupcosts"].ToString(),
                                Install_Comm = reader["Install_Comm"].ToString(),
                                ManagedServicesFees = reader["ManagedServicesFees"].ToString(),
                                Comments = reader["Comments"].ToString(),
                                Sage = reader["Sage"].ToString(),

                            };
                            crmDataList.Add(crmData);
                        }
                    }
                }
                catch (OdbcException ex)
                {
                    Console.WriteLine("There was an error " + ex.Message);
                }
            }

            return View("Index", crmDataList);
        }

        public IActionResult RetrieveCrmData()
        {

            string connectionString = @"Driver={Microsoft Access Driver (*.mdb, *.accdb)};DBQ=C:\Users\Tafadzwag\Documents\DATABASE\VERIFIER2.accdb;Persist Security Info=False;";
            //string connectionString = @"Driver={Microsoft Access Driver (*.mdb, *.accdb)};DBQ=C:\Users\CRM Server\Documents\veriDB\VERIFIER2.accdb;Persist Security Info=False;";

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
            string connectionString = @"Driver={Microsoft Access Driver (*.mdb, *.accdb)};DBQ=C:\Users\Tafadzwag\Documents\DATABASE\VERIFIER2.accdb;Persist Security Info=False;";
            //string connectionString = @"Driver={Microsoft Access Driver (*.mdb, *.accdb)};DBQ=C:\Users\CRM Server\Documents\veriDB\VERIFIER2.accdb;Persist Security Info=False;";

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
