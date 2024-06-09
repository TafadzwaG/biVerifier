﻿using Microsoft.AspNetCore.Mvc;
using System.Data.OleDb;

using biVerifier.Models;
using System.Data.Odbc;
using System.Data;


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

            Console.WriteLine("StartDate", startDate);
            Console.WriteLine("EndDate", endDate);


            string connectionString = @"Driver={Microsoft Access Driver (*.mdb, *.accdb)};DBQ=E:\CODING_HASHIRA\PROJECTS\.NET\databaseAccess\VERIFIER2.accdb;Persist Security Info=False;";
            //string connectionString = @"Driver={Microsoft Access Driver (*.mdb, *.accdb)};DBQ=C:\Users\CRM Server\Documents\veriDB\VERIFIER2.accdb;Persist Security Info=False;";

            string query = @"
              SELECT *
              FROM CRM
              WHERE
              (liveyear > ? OR (liveyear = ? AND livemonth >= ?))
              AND
              (liveyear < ? OR (liveyear = ? AND livemonth <= ?))";

            var filteredData = new List<Crm>();

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
                            Crm crmData = new Crm
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
                                Sage = reader["Comments"].ToString(),


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
            string connectionString = @"Driver={Microsoft Access Driver (*.mdb, *.accdb)};DBQ=E:\CODING_HASHIRA\PROJECTS\.NET\databaseAccess\VERIFIER2.accdb;Persist Security Info=False;";
            //string connectionString = @"Driver={Microsoft Access Driver (*.mdb, *.accdb)};DBQ=C:\Users\CRM Server\Documents\veriDB\VERIFIER2.accdb;Persist Security Info=False;";

            // SQL query to execute
            string query = "SELECT * FROM CRM";

            // Append WHERE clause conditionally based on businessType
            if (!string.IsNullOrEmpty(businessType))
            {
                query += " WHERE Market = ?";
            }
            else
            {
                query += " WHERE Market IS NULL OR Market = ''";
            }

            var filteredData = new List<Crm>();

            using (OdbcConnection connection = new OdbcConnection(connectionString))
            using (OdbcCommand command = new OdbcCommand(query, connection))
            {
                // Add parameter for the businessType if it's not empty
                if (!string.IsNullOrEmpty(businessType))
                {
                    command.Parameters.AddWithValue("Market", businessType);
                }

                try
                {
                    connection.Open();
                    using (OdbcDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Crm crmData = new Crm
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

            Console.WriteLine("Consultant" + consultant);
            Console.WriteLine("StartDate" + startDate);
            Console.WriteLine("EndDate" + endDate);

            string connectionString = @"Driver={Microsoft Access Driver (*.mdb, *.accdb)};DBQ=E:\CODING_HASHIRA\PROJECTS\.NET\databaseAccess\VERIFIER2.accdb;Persist Security Info=False;";
            //string connectionString = @"Driver={Microsoft Access Driver (*.mdb, *.accdb)};DBQ=C:\Users\CRM Server\Documents\veriDB\VERIFIER2.accdb;Persist Security Info=False;";

            string query = @"
            SELECT *
            FROM CRM
            WHERE
            (liveyear > ? OR (liveyear = ? AND livemonth >= ?))
            AND
            (liveyear < ? OR (liveyear = ? AND livemonth <= ?))";

            // Append consultant condition if it's not empty
            if (!string.IsNullOrEmpty(consultant))
            {
                query += " AND Consultant = ?";
            }

            var filteredData = new List<Crm>();

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
                            Crm crmData = new Crm
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

       

        [HttpPost]
        public IActionResult FilterByProvince(string region)
        {
            Console.WriteLine("Selected region: " + region);

            // Retrieve data based on the provided region
            List<Crm> filteredData = RetrieveDataByRegion(region);

            return View(filteredData);
        }

        public IActionResult GetReportByProvince(string region)
        {
            List<Crm> filteredData = RetrieveDataByRegion(region);
            return View(filteredData);
        }

        [HttpGet]
        public IActionResult getCountsInDb()
        {
            int siteCount = 0;
            int cancellationCount = 0;

            // Your logic to query the database and get the counts
            // Assuming you're using ADO.NET with OleDbConnection and OleDbCommand

            string connectionString = @"Driver={Microsoft Access Driver (*.mdb, *.accdb)};DBQ=E:\CODING_HASHIRA\PROJECTS\.NET\databaseAccess\VERIFIER2.accdb;Persist Security Info=False;";
            //string connectionString = @"Driver={Microsoft Access Driver (*.mdb, *.accdb)};DBQ=C:\Users\CRM Server\Documents\veriDB\VERIFIER2.accdb;Persist Security Info=False;";

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

            string connectionString = @"Driver={Microsoft Access Driver (*.mdb, *.accdb)};DBQ=E:\CODING_HASHIRA\PROJECTS\.NET\databaseAccess\VERIFIER2.accdb;Persist Security Info=False;";
            //string connectionString = @"Driver={Microsoft Access Driver (*.mdb, *.accdb)};DBQ=C:\Users\CRM Server\Documents\veriDB\VERIFIER2.accdb;Persist Security Info=False;";

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

        private List<Crm> RetrieveDataByRegion(string region)
        {
            string connectionString = @"Driver={Microsoft Access Driver (*.mdb, *.accdb)};DBQ=E:\CODING_HASHIRA\PROJECTS\.NET\databaseAccess\VERIFIER2.accdb;Persist Security Info=False;";
            //string connectionString = @"Driver={Microsoft Access Driver (*.mdb, *.accdb)};DBQ=C:\Users\CRM Server\Documents\veriDB\VERIFIER2.accdb;Persist Security Info=False;";

            string query = "SELECT * FROM CRM WHERE Province = ?"; // Using "?" for parameter placeholder

            List<Crm> dataList = new List<Crm>();


            using (OdbcConnection connection = new OdbcConnection(connectionString))
            using (OdbcCommand command = new OdbcCommand(query, connection))
            {
                // Add parameter for the region
                command.Parameters.AddWithValue("@Province", region);

                try
                {
                    connection.Open();
                    using (OdbcDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Crm crmData = new Crm
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


        //Cancellation Reports 
        public IActionResult CancellationByDateRange(DateTime startDate, DateTime endDate)
        {
            // Ensure that the date format matches "yyyy/MM/dd"
            string startDateString = startDate.ToString("yyyy/MM/dd");
            string endDateString = endDate.ToString("yyyy/MM/dd");

            // Connection string for Microsoft Access
            string connectionString = @"Driver={Microsoft Access Driver (*.mdb, *.accdb)};DBQ=E:\CODING_HASHIRA\PROJECTS\.NET\databaseAccess\VERIFIER2.accdb;Persist Security Info=False;";
            //string connectionString = @"Driver={Microsoft Access Driver (*.mdb, *.accdb)};DBQ=C:\Users\CRM Server\Documents\veriDB\VERIFIER2.accdb;Persist Security Info=False;";

            // SQL query to filter cancellations by date range
            string query = $"SELECT * FROM Cancellations WHERE [Cancellation End Date] BETWEEN #{startDateString}# AND #{endDateString}#";

            // Create a DataTable to store the results
            var cancellationsDataList = new List<CancellationsData>();

            // Connect to the database and execute the query
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
                            var cData = new CancellationsData();
                            cData.ID = (int)reader["ID"];
                            cData.Client = reader["Client"].ToString();
                            cData.Status = reader["Status"].ToString();
                            cData.Cancel_Billing = reader["Cancel_Billing"].ToString();
                            cData.Site = reader["Site"].ToString();
                            cData.Contact_Person = reader["Contact Person"].ToString();
                            cData.Account_Manager = reader["Account Manager"].ToString();
                            cData.Cancellation_Month = reader["Cancellation Month"].ToString();
                            cData.Cancellation_Received_Date = reader["Cancellation received Date"].ToString();
                            cData.Cancellation_End_Date = reader["Cancellation End date"].ToString();
                            cData.Reason = reader["Reason"].ToString();
                            cData.Notes = reader["Notes"].ToString();
                            cData.Reduced_Value_Ex_Vat = reader["Reduced Value ex vat"].ToString();
                            cData.TechResponsible = reader["TechResponsible"].ToString();
                            cData.Total_Channels = reader["Total_channels"].ToString();
                            cData.Platform = reader["Platform"].ToString();
                            cData.Cancel_GSM = reader["Cancel_GSM"].ToString();
                            cData.Cancel_DNS = reader["Cancel_DNS"].ToString();
                            cData.Cancel_LPR_Licenses = reader["Cancel_LPR_Licenses"].ToString();
                            cData.Cancel_Video_Analytics_Licenses = reader["Cancel_Video_Analytics_Licenses"].ToString();
                            cData.Cancel_Internet_Connectivity = reader["Cancel_Internet_Connectivity"].ToString();
                            cData.Cancel_Billing = reader["Cancel_Billing"].ToString();
                            cData.Status = reader["Status"].ToString();

                            cancellationsDataList.Add(cData);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Handle exceptions, such as connection errors or query errors
                    Console.WriteLine("An error occurred: " + ex.Message);
                    // Optionally, return an error view or redirect to an error page
                    return View("Error");
                }
            }

            // Pass the DataTable to the view to display the filtered cancellations
            Console.WriteLine(cancellationsDataList.Count());
            return View(cancellationsDataList);
        }




        public IActionResult CancellationFilterByReason(string reason)
        {
            Console.WriteLine("Cancellation" + " " + reason);
            string connectionString = @"Driver={Microsoft Access Driver (*.mdb, *.accdb)};DBQ=E:\CODING_HASHIRA\PROJECTS\.NET\databaseAccess\VERIFIER2.accdb;Persist Security Info=False;";
            //string connectionString = @"Driver={Microsoft Access Driver (*.mdb, *.accdb)};DBQ=C:\Users\CRM Server\Documents\veriDB\VERIFIER2.accdb;Persist Security Info=False;";

            // SQL query to execute
            string query = "SELECT * FROM Cancellations";

            // Append WHERE clause conditionally based on the provided reason
            if (!string.IsNullOrEmpty(reason))
            {
                if (reason.ToLower() != "other")
                {
                    query += " WHERE Reason = ?";
                }
                else
                {
                    query += " WHERE Reason IS NULL OR Reason = ''";
                }
            }

            var cancellationData = new List<CancellationsData>();

            using (OdbcConnection connection = new OdbcConnection(connectionString))
            using (OdbcCommand command = new OdbcCommand(query, connection))
            {
                // Add parameter for the reason if it's not "other"
                if (!string.IsNullOrEmpty(reason) && reason.ToLower() != "other")
                {
                    command.Parameters.AddWithValue("Reason", reason);
                }

                try
                {
                    connection.Open();
                    using (OdbcDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var cData = new CancellationsData();
                            {
                                cData.ID = (int)reader["ID"];
                                cData.Client = reader["Client"].ToString();
                                cData.Status = reader["Status"].ToString();
                                cData.Cancel_Billing = reader["Cancel_Billing"].ToString();
                                cData.Site = reader["Site"].ToString();
                                cData.Contact_Person = reader["Contact Person"].ToString();
                                cData.Account_Manager = reader["Account Manager"].ToString();
                                cData.Cancellation_Month = reader["Cancellation Month"].ToString();
                                cData.Cancellation_Received_Date = reader["Cancellation received Date"].ToString();
                                cData.Cancellation_End_Date = reader["Cancellation End date"].ToString();
                                cData.Reason = reader["Reason"].ToString();
                                cData.Notes = reader["Notes"].ToString();
                                cData.Reduced_Value_Ex_Vat = reader["Reduced Value ex vat"].ToString();
                                cData.TechResponsible = reader["TechResponsible"].ToString();
                                cData.Total_Channels = reader["Total_channels"].ToString();
                                cData.Platform = reader["Platform"].ToString();
                                cData.Cancel_GSM = reader["Cancel_GSM"].ToString();
                                cData.Cancel_DNS = reader["Cancel_DNS"].ToString();
                                cData.Cancel_LPR_Licenses = reader["Cancel_LPR_Licenses"].ToString();
                                cData.Cancel_Video_Analytics_Licenses = reader["Cancel_Video_Analytics_Licenses"].ToString();
                                cData.Cancel_Internet_Connectivity = reader["Cancel_Internet_Connectivity"].ToString();
                                cData.Cancel_Billing = reader["Cancel_Billing"].ToString();
                                cData.Status = reader["Status"].ToString();

                            };
                            cancellationData.Add(cData);
                        }
                    }
                }
                catch (OdbcException ex)
                {
                    Console.WriteLine("OleDbException occurred: " + ex.Message);
                }
            }

            // Pass the filtered data to the view
            Console.WriteLine("Cancellation Found Count" + " " + cancellationData.Count());
            return View(cancellationData);
        }



        //Technical Reports 
        public IActionResult TechnicalByClientType(string client)
        {

            if (client == null)
            {
                return View(new List<TechCancel>());
            }

            string connectionString = @"Driver={Microsoft Access Driver (*.mdb, *.accdb)};DBQ=E:\CODING_HASHIRA\PROJECTS\.NET\databaseAccess\VERIFIER2.accdb;Persist Security Info=False;";
            //string connectionString = @"Driver={Microsoft Access Driver (*.mdb, *.accdb)};DBQ=C:\Users\CRM Server\Documents\veriDB\VERIFIER2.accdb;Persist Security Info=False;";

            string query;
            var techCancelDataList = new List<TechCancel>();

            // Check if client is "other" to adjust the query and parameters
            if (client.ToLower() == "other")
            {
                query = "SELECT * FROM TechCancel WHERE Platform IS NULL OR Platform = '' OR Platform = 'N/A'";
            }
            else
            {
                query = "SELECT * FROM TechCancel WHERE Platform = ?";
            }

            using (OdbcConnection connection = new OdbcConnection(connectionString))
            using (OdbcCommand command = new OdbcCommand(query, connection))
            {
                // Add parameter for the client if it's not "other"
                if (client.ToLower() != "other")
                {
                    command.Parameters.AddWithValue("@Platform", client);
                }

                try
                {
                    connection.Open();
                    using (OdbcDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var cData = new TechCancel();
                            cData.Client = reader["Client"].ToString();
                            cData.SiteID = reader["SiteID"].ToString();
                            cData.Date = reader["Date"].ToString();
                            cData.TechResponsible = reader["TechResponsible"].ToString();
                            cData.Total_Channels = reader["Total_channels"] != DBNull.Value ? Convert.ToInt32(reader["Total_channels"]) : null;
                            cData.Platform = reader["Platform"].ToString();
                            cData.Cancel_GSM = reader["Cancel_GSM"].ToString();
                            cData.Cancel_DNS = reader["Cancel_DNS"].ToString();
                            cData.Cancel_LPR_Licenses = reader["Cancel_LPR_Licenses"].ToString();
                            cData.Cancel_Video_Analytics_Licenses = reader["Cancel_Internet_Connectivity"].ToString();
                            cData.Cancel_Billing = reader["Cancel_Billing"].ToString();
                            techCancelDataList.Add(cData);
                        }
                    }
                }
                catch (OdbcException ex)
                {
                    // Handle exception
                    Console.WriteLine("Error whilst fetching Data: " + ex.Message);
                }
            }
            Console.WriteLine("Paltform Search Count" + " " + techCancelDataList);
            return View(techCancelDataList);
        }


        public IActionResult TechnicalByTechnician(string technician)
        {
            if (technician == null)
            {
                return View(new List<TechCancel>());
            }

            string connectionString = @"Driver={Microsoft Access Driver (*.mdb, *.accdb)};DBQ=E:\CODING_HASHIRA\PROJECTS\.NET\databaseAccess\VERIFIER2.accdb;Persist Security Info=False;";
            //string connectionString = @"Driver={Microsoft Access Driver (*.mdb, *.accdb)};DBQ=C:\Users\CRM Server\Documents\veriDB\VERIFIER2.accdb;Persist Security Info=False;";

            string query = "SELECT * FROM TechCancel WHERE TechResponsible = ?";
            var techCancelDataList = new List<TechCancel>();

            using (OdbcConnection connection = new OdbcConnection(connectionString))
            using (OdbcCommand command = new OdbcCommand(query, connection))
            {
                command.Parameters.AddWithValue("@TechResponsible", technician);
                try
                {
                    connection.Open();
                    using (OdbcDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var cData = new TechCancel
                            {
                                Client = reader["Client"].ToString(),
                                SiteID = reader["SiteID"].ToString(),
                                Date = reader["Date"].ToString(),
                                TechResponsible = reader["TechResponsible"].ToString(),
                                Total_Channels = reader["Total_channels"] != DBNull.Value ? Convert.ToInt32(reader["Total_channels"]) : 0,
                                Platform = reader["Platform"].ToString(),
                                Cancel_GSM = reader["Cancel_GSM"].ToString(),
                                Cancel_DNS = reader["Cancel_DNS"].ToString(),
                                Cancel_LPR_Licenses = reader["Cancel_LPR_Licenses"].ToString(),
                                Cancel_Video_Analytics_Licenses = reader["Cancel_Internet_Connectivity"].ToString(),
                                Cancel_Billing = reader["Cancel_Billing"].ToString()
                            };
                            techCancelDataList.Add(cData);
                        }
                    }
                }
                catch (OdbcException ex)
                {
                    // Handle exception
                    Console.WriteLine("Error whilst fetching Data: " + ex.Message);
                   
                }
            }
            Console.Write("TechCancel" + " " + techCancelDataList.Count());
            return View(techCancelDataList);
        }



        public IActionResult TechnicalByDateRange(DateTime startDate, DateTime endDate)
        {
            // Ensure that the date format matches "dd.MM.yyyy"
            string startDateString = startDate.ToString("dd.MM.yyyy");
            string endDateString = endDate.ToString("dd.MM.yyyy");

            //string connectionString = @"Driver={Microsoft Access Driver (*.mdb, *.accdb)};DBQ=C:\Users\CRM Server\Documents\veriDB\VERIFIER2.accdb;Persist Security Info=False;";
            string connectionString = @"Driver={Microsoft Access Driver (*.mdb, *.accdb)};DBQ=E:\CODING_HASHIRA\PROJECTS\.NET\databaseAccess\VERIFIER2.accdb;Persist Security Info=False;";

            var techCancelDataList = new List<TechCancel>();

            // SQL query to filter TechCancelData by date range
            string query = "SELECT * FROM TechCancel WHERE [Date] BETWEEN ? AND ?";

            using (OdbcConnection connection = new OdbcConnection(connectionString))
            using (OdbcCommand command = new OdbcCommand(query, connection))
            {
                // Add parameters for the start and end dates
                command.Parameters.AddWithValue("@StartDate", startDateString);
                command.Parameters.AddWithValue("@EndDate", endDateString);

                try
                {
                    connection.Open();
                    using (OdbcDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var cData = new TechCancel
                            {
                                Client = reader["Client"].ToString(),
                                SiteID = reader["SiteID"].ToString(),
                                Date = reader["Date"].ToString(),
                                TechResponsible = reader["TechResponsible"].ToString(),
                                Total_Channels = reader["Total_channels"] != DBNull.Value ? Convert.ToInt32(reader["Total_channels"]) : 0,
                                Platform = reader["Platform"].ToString(),
                                Cancel_GSM = reader["Cancel_GSM"].ToString(),
                                Cancel_DNS = reader["Cancel_DNS"].ToString(),
                                Cancel_LPR_Licenses = reader["Cancel_LPR_Licenses"].ToString(),
                                Cancel_Video_Analytics_Licenses = reader["Cancel_Internet_Connectivity"].ToString(),
                                Cancel_Billing = reader["Cancel_Billing"].ToString()
                            };
                            techCancelDataList.Add(cData);
                        }
                    }
                }
                catch (OdbcException ex)
                {
                    Console.WriteLine("Error whilst fetching Data: " + ex.Message);
                   
                }
            }
            Console.WriteLine("TECH COUNTS" + " " + techCancelDataList.Count());

            return View(techCancelDataList);
        }





    }
}
