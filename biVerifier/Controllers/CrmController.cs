﻿using Microsoft.AspNetCore.Mvc;
using biVerifier.Models;
using System.Data.Odbc;
using Microsoft.Extensions.Configuration;

namespace biVerifier.Controllers
{
    public class CrmController : Controller
    {
        private readonly string _connectionString;

        public CrmController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("CrmDb");
        }

        public IActionResult Index()
        {
            string query = "SELECT * FROM CRM";
            var crmDataList = new List<Crm>();

            using (OdbcConnection connection = new OdbcConnection(_connectionString))
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
                            crmData.VCC_Code = reader["VCC_Code"].ToString();
                            crmDataList.Add(crmData);
                        }
                    }
                }
                catch (OdbcException ex)
                {
                    Console.WriteLine("There was an error " + ex.Message);
                }
            }

            return View(crmDataList);
        }

        public IActionResult SearchIndex(string searchString)
        {
            string query = "SELECT * FROM CRM";
            if (!string.IsNullOrEmpty(searchString))
            {
                query += " WHERE Client LIKE ? OR Contact_Person LIKE ? OR Email LIKE ?";
            }

            var crmDataList = new List<Crm>();

            using (OdbcConnection connection = new OdbcConnection(_connectionString))
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
                                VCC_Code = reader["VCC_Code"].ToString()
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
            string query = "SELECT * FROM CRM";
            var crmDataList = new List<CRMData>();

            using (OdbcConnection connection = new OdbcConnection(_connectionString))
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
                            crmDataList.Add(crmData);
                        }
                    }
                }
                catch (OdbcException ex)
                {
                    Console.WriteLine("There was an error " + ex.Message);
                }
            }

            return View(crmDataList);
        }

        public IActionResult FilterByProvince()
        {
            string query = "SELECT * FROM CRM WHERE Region = 'GP'";
            var crmDataList = new List<CRMData>();

            using (OdbcConnection connection = new OdbcConnection(_connectionString))
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
                            crmDataList.Add(crmData);
                        }
                    }
                }
                catch (OdbcException ex)
                {
                    Console.WriteLine("There was an error " + ex.Message);
                }
            }

            return View(crmDataList);
        }
    }
}