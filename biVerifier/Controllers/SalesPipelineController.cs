﻿using biVerifier.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.Odbc;



namespace biVerifier.Controllers
{
    public class SalesPipelineController : Controller
    {
       

        public IActionResult Index()
        {

            //string connectionString = @"Driver={Microsoft Access Driver (*.mdb, *.accdb)};DBQ=E:\CODING_HASHIRA\PROJECTS\.NET\databaseAccess\VERIFIER2.accdb;Persist Security Info=False;";
            string connectionString = @"Driver={Microsoft Access Driver (*.mdb, *.accdb)};DBQ=C:\Users\CRM Server\Documents\veriDB\VERIFIER2.accdb;Persist Security Info=False;";

            // SQL query to execute


            string query = "SELECT * FROM Sales_Pipeline";

            var salesPipelineList = new List<SalesPipeline>();

            using (OdbcConnection connection = new OdbcConnection(connectionString))
            using (OdbcCommand command = new OdbcCommand(query, connection))


                try
                {
                    connection.Open();
                    using (OdbcDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            SalesPipeline crmData = new SalesPipeline
                            {
                                QuoteNum = (int)reader["QuoteNum"],
                                Client = reader["Client"].ToString(),
                                Lead_Source = reader["LeadSource"].ToString(),
                                Contact_Person = reader["Contact_Person"].ToString(),
                                Email = reader["Email"].ToString(),
                                City = reader["City"].ToString(),
                                Service = reader["Service"].ToString(),
                                Lead_Month = reader["leadmonth"].ToString(),
                                Consultant = reader["Consultant"].ToString(),
                                OnceOffCost = reader["OnceOffCost"].ToString(),
                                RecurringCost = reader["RecurringCost"].ToString(),
                                Probability = reader["Probability"].ToString(),
                                Market = reader["Market"].ToString(),
                                SageQuote = reader["SageQuote"].ToString()


                            };


                            salesPipelineList.Add(crmData);
                        }
                    }

                }
                catch (OdbcException ex)
                {
                    Console.WriteLine("There was an error " + ex.Message);
                }

            return View(salesPipelineList);

        }

        public IActionResult Search(string searchTerm)
        {
            //string connectionString = @"Driver={Microsoft Access Driver (*.mdb, *.accdb)};DBQ=E:\CODING_HASHIRA\PROJECTS\.NET\databaseAccess\VERIFIER2.accdb;Persist Security Info=False;";
            string connectionString = @"Driver={Microsoft Access Driver (*.mdb, *.accdb)};DBQ=C:\Users\CRM Server\Documents\veriDB\VERIFIER2.accdb;Persist Security Info=False;";
            string query = "SELECT * FROM Sales_Pipeline WHERE Client LIKE ?";
            var salesPipelineList = new List<SalesPipeline>();

            using (OdbcConnection connection = new OdbcConnection(connectionString))
            using (OdbcCommand command = new OdbcCommand(query, connection))
            {
                command.Parameters.AddWithValue("@SearchTerm", "%" + searchTerm + "%");

                try
                {
                    connection.Open();
                    using (OdbcDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            SalesPipeline crmData = new SalesPipeline
                            {
                                QuoteNum = (int)reader["QuoteNum"],
                                Client = reader["Client"].ToString(),
                                Lead_Source = reader["LeadSource"].ToString(),
                                Contact_Person = reader["Contact_Person"].ToString(),
                                Email = reader["Email"].ToString(),
                                City = reader["City"].ToString(),
                                Service = reader["Service"].ToString(),
                                Lead_Month = reader["leadmonth"].ToString(),
            
                                Consultant = reader["Consultant"].ToString(),
                                OnceOffCost = reader["OnceOffCost"].ToString(),
                                RecurringCost = reader["RecurringCost"].ToString(),
                                Probability = reader["Probability"].ToString(),
                                Market = reader["Market"].ToString(),
                                 SageQuote = reader["SageQuote"].ToString()
                            };
                            salesPipelineList.Add(crmData);
                        }
                    }
                }
                catch (OdbcException ex)
                {
                    Console.WriteLine("There was an error " + ex.Message);
                }
            }

            return View("Index", salesPipelineList);
        }


        public IActionResult FilterByDateRange(DateTime startDate, DateTime endDate)
        {
            //string connectionString = @"Driver={Microsoft Access Driver (*.mdb, *.accdb)};DBQ=E:\CODING_HASHIRA\PROJECTS\.NET\databaseAccess\VERIFIER2.accdb;Persist Security Info=False;";
            string connectionString = @"Driver={Microsoft Access Driver (*.mdb, *.accdb)};DBQ=C:\Users\CRM Server\Documents\veriDB\VERIFIER2.accdb;Persist Security Info=False;"; 

            string query = "SELECT * FROM Sales_Pipeline WHERE leadmonth BETWEEN ? AND ?";

            var salesPipelineList = new List<SalesPipeline>();

            using (OdbcConnection connection = new OdbcConnection(connectionString))
            using (OdbcCommand command = new OdbcCommand(query, connection))
            {
                command.Parameters.AddWithValue("@StartDate", startDate);
                command.Parameters.AddWithValue("@EndDate", endDate);

                try
                {
                    connection.Open();
                    using (OdbcDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            SalesPipeline crmData = new SalesPipeline
                            {
                                QuoteNum = (int)reader["QuoteNum"],
                                Client = reader["Client"].ToString(),
                                Lead_Source = reader["LeadSource"].ToString(),
                                Contact_Person = reader["Contact_Person"].ToString(),
                                Email = reader["Email"].ToString(),
                                City = reader["City"].ToString(),
                                Service = reader["Service"].ToString(),
                                Lead_Month = reader["leadmonth"].ToString(),
                               
                                Consultant = reader["Consultant"].ToString(),
                                OnceOffCost = reader["OnceOffCost"].ToString(),
                                RecurringCost = reader["RecurringCost"].ToString(),
                                Probability = reader["Probability"].ToString(),
                                Market = reader["Market"].ToString(),
                                SageQuote = reader["SageQuote"].ToString()
                            };
                            salesPipelineList.Add(crmData);
                        }
                    }

                }
                catch (OdbcException ex)
                {
                    Console.WriteLine("There was an error " + ex.Message);
                }
            }

            return View(salesPipelineList);
        }

        public IActionResult FilterBySalesPerson(string salesPerson)
        {
            //string connectionString = @"Driver={Microsoft Access Driver (*.mdb, *.accdb)};DBQ=E:\CODING_HASHIRA\PROJECTS\.NET\databaseAccess\VERIFIER2.accdb;Persist Security Info=False;";
            string connectionString = @"Driver={Microsoft Access Driver (*.mdb, *.accdb)};DBQ=C:\Users\CRM Server\Documents\veriDB\VERIFIER2.accdb;Persist Security Info=False;";

            string query = "SELECT * FROM Sales_Pipeline WHERE Consultant = ?";

            var salesPipelineList = new List<SalesPipeline>();

            using (OdbcConnection connection = new OdbcConnection(connectionString))
            using (OdbcCommand command = new OdbcCommand(query, connection))
            {
                command.Parameters.AddWithValue("@ContactPerson", salesPerson);

                try
                {
                    connection.Open();
                    using (OdbcDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            SalesPipeline crmData = new SalesPipeline
                            {
                                QuoteNum = (int)reader["QuoteNum"],
                                Client = reader["Client"].ToString(),
                                Lead_Source = reader["LeadSource"].ToString(),
                                Contact_Person = reader["Contact_Person"].ToString(),
                                Email = reader["Email"].ToString(),
                                City = reader["City"].ToString(),
                                Service = reader["Service"].ToString(),
                                Lead_Month = reader["leadmonth"].ToString(),
                                Consultant = reader["Consultant"].ToString(),
                                OnceOffCost = reader["OnceOffCost"].ToString(),
                                RecurringCost = reader["RecurringCost"].ToString(),
                                Probability = reader["Probability"].ToString(),
                                Market = reader["Market"].ToString(),
                                SageQuote = reader["SageQuote"].ToString()

                            };
                            salesPipelineList.Add(crmData);
                        }
                    }

                }
                catch (OdbcException ex)
                {
                    Console.WriteLine("There was an error " + ex.Message);
                }
            }

            return View(salesPipelineList);
        }


        public IActionResult FilterByProvince(string province)
        {
            //string connectionString = @"Driver={Microsoft Access Driver (*.mdb, *.accdb)};DBQ=E:\CODING_HASHIRA\PROJECTS\.NET\databaseAccess\VERIFIER2.accdb;Persist Security Info=False;";
            string connectionString = @"Driver={Microsoft Access Driver (*.mdb, *.accdb)};DBQ=C:\Users\CRM Server\Documents\veriDB\VERIFIER2.accdb;Persist Security Info=False;";

            string query = "SELECT * FROM Sales_Pipeline WHERE City = ?";

            var salesPipelineList = new List<SalesPipeline>();

            using (OdbcConnection connection = new OdbcConnection(connectionString))
            using (OdbcCommand command = new OdbcCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Province", province);

                try
                {
                    connection.Open();
                    using (OdbcDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            SalesPipeline crmData = new SalesPipeline
                            {
                                QuoteNum = (int)reader["QuoteNum"],
                                Client = reader["Client"].ToString(),
                                Lead_Source = reader["LeadSource"].ToString(),
                                Contact_Person = reader["Contact_Person"].ToString(),
                                Email = reader["Email"].ToString(),
                                City = reader["City"].ToString(),
                                Service = reader["Service"].ToString(),
                                Lead_Month = reader["leadmonth"].ToString(),
                                Consultant = reader["Consultant"].ToString(),
                                OnceOffCost = reader["OnceOffCost"].ToString(),
                                RecurringCost = reader["RecurringCost"].ToString(),
                                Probability = reader["Probability"].ToString(),
                                Market = reader["Market"].ToString(),
                                SageQuote = reader["SageQuote"].ToString()
                            };
                            salesPipelineList.Add(crmData);
                        }
                    }

                }
                catch (OdbcException ex)
                {
                    Console.WriteLine("There was an error " + ex.Message);
                }
            }

            return View(salesPipelineList);
        }
    }

    }
