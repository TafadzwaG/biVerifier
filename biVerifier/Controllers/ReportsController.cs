using Microsoft.AspNetCore.Mvc;
using System.Data.OleDb;

using biVerifier.Models;

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



            string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=E:\CODING_HASHIRA\PROJECTS\.NET\databaseAccess\VERIFIER1.accdb;Persist Security Info=False;";

            string query = @"
                SELECT Consultant, Sites
                FROM CRM
                WHERE (LIVE_Year > @StartYear OR (LIVE_Year = @StartYear AND LIVE_Month >= @StartMonth))
                AND (LIVE_Year < @EndYear OR (LIVE_Year = @EndYear AND LIVE_Month <= @EndMonth))";

            var filteredData = new List<DateRangeData>();

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            using (OleDbCommand command = new OleDbCommand(query, connection))
            {
              
                command.Parameters.AddWithValue("@StartYear", startDate.Year);
                command.Parameters.AddWithValue("@StartMonth", startDate.ToString("MMM"));
                command.Parameters.AddWithValue("@EndYear", endDate.Year);
                command.Parameters.AddWithValue("@EndMonth", endDate.ToString("MMM"));

                try
                {
                    connection.Open();
                    using (OleDbDataReader reader = command.ExecuteReader())
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

            string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=E:\CODING_HASHIRA\PROJECTS\.NET\databaseAccess\VERIFIER1.accdb;Persist Security Info=False;";
            string siteCountQuery = "SELECT COUNT(*) FROM Sites";
            string cancellationCountQuery = "SELECT COUNT(*) FROM Cancellations";

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                using (OleDbCommand siteCommand = new OleDbCommand(siteCountQuery, connection))
                using (OleDbCommand cancellationCommand = new OleDbCommand(cancellationCountQuery, connection))
                {
                    try
                    {
                        connection.Open();
                        // Execute query to get site count
                        siteCount = (int)siteCommand.ExecuteScalar();
                        // Execute query to get cancellation count
                        cancellationCount = (int)cancellationCommand.ExecuteScalar();
                    }
                    catch (OleDbException ex)
                    {
                        // Handle exception if needed
                        Console.WriteLine("OleDbException occurred: " + ex.Message);
                    }
                }
            }

            // Pass the counts to the Index view
            ViewData["SiteCount"] = siteCount;
            ViewData["CancellationCount"] = cancellationCount;

            return RedirectToAction("Index"); // Redirect to the Index action method to display the counts
        }



        private List<CRMData> RetrieveUniqueRegionsFromDatabase()
        {
            string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=E:\CODING_HASHIRA\PROJECTS\.NET\databaseAccess\VERIFIER1.accdb;Persist Security Info=False;";
            string query = "SELECT DISTINCT Region FROM CRM";

            List<CRMData> uniqueRegions = new List<CRMData>();

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
            string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=E:\CODING_HASHIRA\PROJECTS\.NET\databaseAccess\VERIFIER1.accdb;Persist Security Info=False;";
            string query = "SELECT * FROM CRM WHERE Region = @Region";

            List<CRMData> dataList = new List<CRMData>();

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            using (OleDbCommand command = new OleDbCommand(query, connection))
            {
                // Add parameter for the region
                command.Parameters.AddWithValue("@Region", region);

                try
                {
                    connection.Open();
                    using (OleDbDataReader reader = command.ExecuteReader())
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
