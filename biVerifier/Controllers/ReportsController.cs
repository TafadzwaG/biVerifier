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
            string connectionString = @"Driver={Microsoft Access Driver (*.mdb, *.accdb)};DBQ=E:\CODING_HASHIRA\PROJECTS\.NET\databaseAccess\VERIFIER1.accdb;Persist Security Info=False;";

            string query = @"
              SELECT *
              FROM CRM
              WHERE
              (LIVE_Year > ? OR (LIVE_Year = ? AND LIVE_Month >= ?))
              AND
              (LIVE_Year < ? OR (LIVE_Year = ? AND LIVE_Month <= ?))";

            var filteredData = new List<CRMData>();

            using (OdbcConnection connection = new OdbcConnection(connectionString))
            using (OdbcCommand command = new OdbcCommand(query, connection))
            {
                // Parameters for ? placeholders in the query
                command.Parameters.AddWithValue("?", startDate.Year);
                command.Parameters.AddWithValue("?", startDate.Year);
                command.Parameters.AddWithValue("?", startDate.ToString("MMM"));
                command.Parameters.AddWithValue("?", endDate.Year);
                command.Parameters.AddWithValue("?", endDate.Year);
                command.Parameters.AddWithValue("?", endDate.ToString("MMM"));

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
                                Region = reader["Region"].ToString(),
                                ClientID = (int)reader["ClientID"],
                                Contact_Person = reader["Contact_Person"].ToString(), 
                                EmailAdd = reader["EmailAdd"].ToString(),
                                Contact_Num = reader["ContactNum"].ToString(),
                                Num = reader["Num"].ToString(),
                                Street = reader["Street"].ToString(),
                                City = reader["City"].ToString(),
                                Lead_Source = reader["Lead_Source"].ToString(),
                                Service_Type = reader["Service_Type"].ToString(),
                                Market_Type = reader["Market_Type"].ToString(),
                                Category = reader["Category"].ToString(),
                                Consultant = reader["Consultant"].ToString(),
                                Branch = reader["Branch"].ToString(),
                                Status = reader["Status"].ToString(),
                                Probability = reader["Probability"].ToString(),
                                LEAD_Year = reader["LEAD_Year"].ToString(),
                                LEAD_Month = reader["LEAD_Month"].ToString(),
                                LIVE_Year = reader["LIVE_Year"].ToString(),
                                LIVE_Month = reader["LIVE_Month"].ToString(),
                                GandTotal = reader["GandTotal"].ToString(),
                                Onceoffsetupcosts = reader["Onceoffsetupcosts"].ToString(),
                                Install_Comm = reader["Install_Comm"].ToString()
                            };
                            filteredData.Add(crmData);
                        }
                    }
                }
                catch (OdbcException ex)
                {
                    Console.WriteLine("OleDbException occurred: " + ex.Message);
                 
                }
            }

            Console.WriteLine("Filtered Data: " + filteredData.Count());
            return View(filteredData);
        }







      

        public IActionResult FilterByBusinessType(string businessType)
        {
            string connectionString = @"Driver={Microsoft Access Driver (*.mdb, *.accdb)};DBQ=E:\CODING_HASHIRA\PROJECTS\.NET\databaseAccess\VERIFIER1.accdb;Persist Security Info=False;";

            // SQL query to execute
            string query = "SELECT * FROM CRM";

            // Append WHERE clause conditionally based on businessType
            if (!string.IsNullOrEmpty(businessType))
            {
                query += " WHERE Market_Type = ?";
            }
            else
            {
                query += " WHERE Market_Type IS NULL OR Market_Type = ''"; 
            }

            var filteredData = new List<CRMData>();

            using (OdbcConnection connection = new OdbcConnection(connectionString))
            using (OdbcCommand command = new OdbcCommand(query, connection))
            {
                // Add parameter for the businessType if it's not empty
                if (!string.IsNullOrEmpty(businessType))
                {
                    command.Parameters.AddWithValue("Market_Type", businessType);
                }

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
                                Region = reader["Region"].ToString(),
                                ClientID = (int)reader["ClientID"],
                                Contact_Person = reader["Contact_Person"].ToString(),
                                EmailAdd = reader["EmailAdd"].ToString(),
                                Contact_Num = reader["ContactNum"].ToString(),
                                Num = reader["Num"].ToString(),
                                Street = reader["Street"].ToString(),
                                City = reader["City"].ToString(),
                                Lead_Source = reader["Lead_Source"].ToString(),
                                Service_Type = reader["Service_Type"].ToString(),
                                Market_Type = reader["Market_Type"].ToString(),
                                Category = reader["Category"].ToString(),
                                Consultant = reader["Consultant"].ToString(),
                                Branch = reader["Branch"].ToString(),
                                Status = reader["Status"].ToString(),
                                Probability = reader["Probability"].ToString(),
                                LEAD_Year = reader["LEAD_Year"].ToString(),
                                LEAD_Month = reader["LEAD_Month"].ToString(),
                                LIVE_Year = reader["LIVE_Year"].ToString(),
                                LIVE_Month = reader["LIVE_Month"].ToString(),
                                GandTotal = reader["GandTotal"].ToString(),
                                Onceoffsetupcosts = reader["Onceoffsetupcosts"].ToString(),
                                Install_Comm = reader["Install_Comm"].ToString()
                            };
                            filteredData.Add(crmData);
                        }
                    }
                }
                catch (OleDbException ex)
                {
                    Console.WriteLine("There was an error " + ex.Message);
                }
            }

            // Pass the filtered data to the view
            return View(filteredData);
        }


     
        

        public IActionResult FilterByDateAndConsultant(DateTime startDate, DateTime endDate, string consultant)
        {

            Console.WriteLine("Consultant", consultant);
            Console.WriteLine("StartDate", startDate);
            Console.WriteLine("EndDate", endDate);

            string connectionString = @"Driver={Microsoft Access Driver (*.mdb, *.accdb)};DBQ=E:\CODING_HASHIRA\PROJECTS\.NET\databaseAccess\VERIFIER1.accdb;Persist Security Info=False;";

            string query = @"
            SELECT *
            FROM CRM
            WHERE
            (LIVE_Year > ? OR (LIVE_Year = ? AND LIVE_Month >= ?))
            AND
            (LIVE_Year < ? OR (LIVE_Year = ? AND LIVE_Month <= ?))";

            // Append consultant condition if it's not empty
            if (!string.IsNullOrEmpty(consultant))
            {
                query += " AND Consultant = ?";
            }

            var filteredData = new List<CRMData>();

            using (OdbcConnection connection = new OdbcConnection(connectionString))
            using (OdbcCommand command = new OdbcCommand(query, connection))
            {
                // Parameters for ? placeholders in the query
                command.Parameters.AddWithValue("?", startDate.Year);
                command.Parameters.AddWithValue("?", startDate.Year);
                command.Parameters.AddWithValue("?", startDate.ToString("MMM"));
                command.Parameters.AddWithValue("?", endDate.Year);
                command.Parameters.AddWithValue("?", endDate.Year);
                command.Parameters.AddWithValue("?", endDate.ToString("MMM"));

                // Add parameter for consultant if it's not empty
                if (!string.IsNullOrEmpty(consultant))
                {
                    command.Parameters.AddWithValue("?", consultant);
                }

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
                                Region = reader["Region"].ToString(),
                                ClientID = (int)reader["ClientID"],
                                Contact_Person = reader["Contact_Person"].ToString(),
                                EmailAdd = reader["EmailAdd"].ToString(),
                                Contact_Num = reader["ContactNum"].ToString(),
                                Num = reader["Num"].ToString(),
                                Street = reader["Street"].ToString(),
                                City = reader["City"].ToString(),
                                Lead_Source = reader["Lead_Source"].ToString(),
                                Service_Type = reader["Service_Type"].ToString(),
                                Market_Type = reader["Market_Type"].ToString(),
                                Category = reader["Category"].ToString(),
                                Consultant = reader["Consultant"].ToString(),
                                Branch = reader["Branch"].ToString(),
                                Status = reader["Status"].ToString(),
                                Probability = reader["Probability"].ToString(),
                                LEAD_Year = reader["LEAD_Year"].ToString(),
                                LEAD_Month = reader["LEAD_Month"].ToString(),
                                LIVE_Year = reader["LIVE_Year"].ToString(),
                                LIVE_Month = reader["LIVE_Month"].ToString(),
                                GandTotal = reader["GandTotal"].ToString(),
                                Onceoffsetupcosts = reader["Onceoffsetupcosts"].ToString(),
                                Install_Comm = reader["Install_Comm"].ToString()
                            };
                            filteredData.Add(crmData);
                        }
                    }
                }
                catch (OdbcException ex)
                {
                    Console.WriteLine("OleDbException occurred: " + ex.Message);
                }
            }

            Console.WriteLine("Filtered Data: " + filteredData.Count());
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
            string connectionString = @"Driver={Microsoft Access Driver (*.mdb, *.accdb)};DBQ=E:\CODING_HASHIRA\PROJECTS\.NET\databaseAccess\VERIFIER1.accdb;Persist Security Info=False;";
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
                catch (OdbcException ex)
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

            string query = "SELECT * FROM CRM WHERE Region = ?"; // Using "?" for parameter placeholder

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
                                Suburb = reader["Suburb"].ToString(), // This line is redundant, it's already assigned above
                                Region = reader["Region"].ToString(),
                                ClientID = (int)reader["ClientID"],
                                Contact_Person = reader["Contact_Person"].ToString(), // Corrected property name
                                EmailAdd = reader["EmailAdd"].ToString(),
                                Contact_Num = reader["ContactNum"].ToString(),
                                Num = reader["Num"].ToString(),
                                Street = reader["Street"].ToString(),
                                City = reader["City"].ToString(), // Assuming City is a property in CRMData
                                Lead_Source = reader["Lead_Source"].ToString(),
                                Service_Type = reader["Service_Type"].ToString(),
                                Market_Type = reader["Market_Type"].ToString(),
                                Category = reader["Category"].ToString(),
                                Consultant = reader["Consultant"].ToString(),
                                Branch = reader["Branch"].ToString(),
                                Status = reader["Status"].ToString(),
                                Probability = reader["Probability"].ToString(),
                                LEAD_Year = reader["LEAD_Year"].ToString(),
                                LEAD_Month = reader["LEAD_Month"].ToString(),
                                LIVE_Year = reader["LIVE_Year"].ToString(),
                                LIVE_Month = reader["LIVE_Month"].ToString(),
                                GandTotal = reader["GandTotal"].ToString(),
                                Onceoffsetupcosts = reader["Onceoffsetupcosts"].ToString(),
                                Install_Comm = reader["Install_Comm"].ToString()
                            };
                            dataList.Add(crmData);
                        }
                    }
                }
                catch (OdbcException ex)
                {
                    // Handle exception
                    Console.WriteLine("Error whilst fetching_Data: " + ex.Message);
                }
            }

            return dataList;
        }





    }
}
