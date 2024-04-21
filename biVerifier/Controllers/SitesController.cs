using biVerifier.Models;
using Microsoft.AspNetCore.Mvc;
using Syncfusion.XlsIO.Implementation;
using System.Data.Common;
using System.Data.Odbc;
using System.Data.OleDb;

namespace biVerifier.Controllers
{
    public class SitesController : Controller
    {
        public IActionResult Index()
        {
            string connectionString = @"Driver={Microsoft Access Driver (*.mdb, *.accdb)};DBQ=E:\CODING_HASHIRA\PROJECTS\.NET\databaseAccess\VERIFIER1.accdb;Persist Security Info=False;";

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
                            cData.Site = reader["Site"].ToString();
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