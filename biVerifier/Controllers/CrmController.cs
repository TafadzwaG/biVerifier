using Microsoft.AspNetCore.Mvc;
using System.Data.OleDb;
using biVerifier.Models;


namespace biVerifier.Controllers
{
    public class CrmController : Controller
    {
        public IActionResult RetrieveCrmData()
        {

            string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=E:\CODING_HASHIRA\PROJECTS\.NET\databaseAccess\VERIFIER1.accdb;Persist Security Info=False;";
            // SQL query to execute


            string query = "SELECT * FROM CRM";


            var crmDataList = new List<CRMData>();

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            using (OleDbCommand command = new OleDbCommand(query, connection))
            {
                try
                {
                    connection.Open();
                    using (OleDbDataReader reader = command.ExecuteReader())
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
                    // Handle exception
                }
            }

            return View(crmDataList);
        }


        public IActionResult FilterByProvince()
        {
            string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=E:\CODING_HASHIRA\PROJECTS\.NET\databaseAccess\VERIFIER1.accdb;Persist Security Info=False;";

            string query = "SELECT * FROM CRM WHERE Region = 'GP'";

            var crmDataList = new List<CRMData>();

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            using (OleDbCommand command = new OleDbCommand(query, connection))

                try
                {
                    connection.Open();
                    using (OleDbDataReader reader = command.ExecuteReader())
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
                    // Handle exception
                }

            return View(crmDataList);
        }


       

       
       


    }
}
