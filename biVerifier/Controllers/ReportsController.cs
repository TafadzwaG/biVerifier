using Microsoft.AspNetCore.Mvc;
using System.Data.OleDb;

using biVerifier.Models;
using System.Data.Odbc;

namespace biVerifier.Controllers
{
    public class ReportsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult FilterByDateRange(DateTime startDate, DateTime endDate)
        {
            Console.WriteLine("Executing");
            Console.WriteLine(endDate.ToString());
            Console.WriteLine(startDate.ToString());
            Console.WriteLine(startDate.Year);
            Console.WriteLine(startDate.ToString("MMM"));



            string connectionString = @"Driver={Microsoft Access Driver (*.mdb, *.accdb)};DBQ=E:\CODING_HASHIRA\PROJECTS\.NET\databaseAccess\VERIFIER1.accdb;Persist Security Info=False;";

            string query = @"
                SELECT Consultant, Sites
                FROM CRM
                WHERE (LIVE_Year > @StartYear OR (LIVE_Year = @StartYear AND LIVE_Month >= @StartMonth))
                AND (LIVE_Year < @EndYear OR (LIVE_Year = @EndYear AND LIVE_Month <= @EndMonth))";

            var filteredData = new List<DateRangeData>();

            using (OdbcConnection connection = new OdbcConnection(connectionString))
            using (OdbcCommand command = new OdbcCommand(query, connection))

            {

                command.Parameters.AddWithValue("@StartYear", startDate.Year);
                command.Parameters.AddWithValue("@StartMonth", startDate.ToString("MMM"));
                command.Parameters.AddWithValue("@EndYear", endDate.Year);
                command.Parameters.AddWithValue("@EndMonth", endDate.ToString("MMM"));

                try
                {
                    connection.Open();
                    using (OdbcDataReader reader = command.ExecuteReader())

                    {
                        while (reader.Read())
                        {
                            var data = new DateRangeData
                            {
                                Consultant = reader["Consultant"].ToString(),
                                Sites = reader["Sites"].ToString(),
                            };
                            filteredData.Add(data);
                        }
                    }
                }
                catch (OleDbException ex)
                {
                    Console.WriteLine("OleDbException occurred: " + ex.Message);
                    // Handle the exception
                }
            }


            Console.WriteLine("Filtered Data: " + filteredData);

            foreach (var item in filteredData)
            {
                Console.WriteLine("Consultant: " + item.Consultant + ", Sites: " + item.Sites);
            }

            return View(filteredData);
        }

        [HttpPost]
        public IActionResult FilterByBusinessType(string businessType)
        {
            Console.WriteLine("Selected region: " + businessType);
            return View("FilterByBusinessType");
        }

        [HttpPost]
        public IActionResult FilterByDateAndConsultant(string businessType)
        {
            Console.WriteLine("Selected region: " + businessType);
            return View();
        }


        [HttpGet]
        public IActionResult ProvinceSelection()
        {
            List<CRMData> provinceList = RetrieveUniqueRegionsFromDatabase();
            return View("FilterByProvince", provinceList);
        }

        [HttpGet]
        public IActionResult FilterByProvince()
        {
           return ProvinceSelection();
        }

        [HttpPost]
        public IActionResult FilterByProvince(string region)
        {
            Console.WriteLine("Selected region: " + region);

            // Retrieve data based on the provided region
            List<CRMData> filteredData = RetrieveDataByRegion(region);

            return View("FilterByProvince", filteredData);
        }

        [HttpGet]
        public IActionResult getCountsInDb()
        {
            int siteCount = 0;
            int cancellationCount = 0;

            // Your logic to query the database and get the counts
            // Assuming you're using ADO.NET with OleDbConnection and OleDbCommand

            string connectionString = @"Driver={Microsoft Access Driver (*.mdb, *.accdb)};DBQ=E:\CODING_HASHIRA\PROJECTS\.NET\databaseAccess\VERIFIER1.accdb;Persist Security Info=False;";

            string siteCountQuery = "SELECT COUNT(*) FROM Sites";
            string cancellationCountQuery = "SELECT COUNT(*) FROM Cancellations";

            using (OdbcConnection connection = new OdbcConnection(connectionString))
            {
                using (OdbcCommand siteCommand = new OdbcCommand(siteCountQuery, connection))
                using (OdbcCommand cancellationCommand = new OdbcCommand(cancellationCountQuery, connection))
                {
                    try
                    {
                        connection.Open();
                        siteCount = (int)siteCommand.ExecuteScalar();
                        cancellationCount = (int)cancellationCommand.ExecuteScalar();
                    }
                    catch (OleDbException ex)
                    {
                        Console.WriteLine("OleDbException occurred: " + ex.Message);
                    }
                }
            }

            ViewData["SiteCount"] = siteCount;
            ViewData["CancellationCount"] = cancellationCount;

            return RedirectToAction("Index"); 
        }



        private List<CRMData> RetrieveUniqueRegionsFromDatabase()
        {
            string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=E:\CODING_HASHIRA\PROJECTS\.NET\databaseAccess\VERIFIER1.accdb;Persist Security Info=False;";
            string query = "SELECT DISTINCT Region FROM CRM";

            List<CRMData> uniqueRegions = new List<CRMData>();

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
                            CRMData regionData = new CRMData();
                            regionData.Region = reader["Region"].ToString();
                            uniqueRegions.Add(regionData);
                        }
                    }
                }
                catch (OleDbException ex)
                {
                    // Handle exception
                    Console.WriteLine("OleDbException occurred: " + ex.Message);
                }
            }

            return uniqueRegions;
        }


        private List<CRMData> RetrieveDataByRegion(string region)
        {
            string connectionString = @"Driver={Microsoft Access Driver (*.mdb, *.accdb)};DBQ=E:\CODING_HASHIRA\PROJECTS\.NET\databaseAccess\VERIFIER1.accdb;Persist Security Info=False;";

            string query = "SELECT * FROM CRM WHERE Region = @Region";

            List<CRMData> dataList = new List<CRMData>();

            using (OdbcConnection connection = new OdbcConnection(connectionString))
            using (OdbcCommand command = new OdbcCommand(query, connection))

            {
                // Add parameter for the region
                command.Parameters.AddWithValue("@Region", region);

                try
                {
                    connection.Open();
                    using (OdbcDataReader reader = command.ExecuteReader())

                    {
                        while (reader.Read())
                        {
                            CRMData crmData = new CRMData
                            {
                                Sites = reader["Sites"].ToString(),
                                Suburb = reader["Suburb"].ToString(),
                                Region = reader["Region"].ToString()
                                // Assign other properties as needed
                            };
                            dataList.Add(crmData);
                        }
                    }
                }
                catch (OleDbException ex)
                {
                    // Handle exception
                    Console.WriteLine("OleDbException occurred: " + ex.Message);
                }
            }

            return dataList;
        }




    }
}
