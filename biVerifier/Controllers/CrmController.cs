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
           
            return View();
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
