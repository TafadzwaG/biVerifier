using System;
using System.Data.Odbc;
using biVerifier.Models;

namespace biVerifier.Repositories
{
    public class UserRepository
    {
        private readonly string _connectionString;

        public UserRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public User? GetUserByUsernameAndPassword(string username, string password)
        {
            Console.WriteLine("Username", username);
            Console.WriteLine("Password", password);

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                // Handle empty or null username/password
                return null;
            }

            using (var connection = new OdbcConnection(_connectionString))
            {
                connection.Open();
                var query = "SELECT * FROM Users WHERE UserName = ? AND UserPW = ?";
                using (var command = new OdbcCommand(query, connection))
                {
                    command.Parameters.AddWithValue("?", username);
                    command.Parameters.AddWithValue("?", password);
                    try
                    {
                        var reader = command.ExecuteReader();
                        if (reader.Read())
                        {
                            // Mapping user properties from database columns
                            var user = new User
                            {
                                UserID = Convert.ToInt32(reader["UserID"]),
                                UName = reader["UName"].ToString(),
                                UserName = reader["UserName"].ToString(),
                                Surname = reader["Surname"].ToString(),
                                UserPW = reader["UserPW"].ToString(),
                                email = reader["email"].ToString(),
                                Security = reader["Security"].ToString(),
                                Department = reader["Department"].ToString(),
                                Status = reader["Status"].ToString(),
                                Answer = reader["Answer"].ToString(),
                                Role = reader["Department"].ToString()
                            };
                            return user;
                        }
                    }
                    catch (OdbcException ex)
                    {
                        Console.WriteLine("OleDbException occurred: " + ex.Message);
                       
                    }
                }
            }
            return null;
        }

    }
}
