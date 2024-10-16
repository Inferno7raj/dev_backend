// PersonalDetailsDAL.cs
using PersonalDetailsAPI.Models;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Collections.Generic;

namespace PersonalDetailsAPI.DAL
{
    public class PersonalDetailsDAL
    {
        private readonly string _connectionString;

        public PersonalDetailsDAL(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public void InsertPersonalDetails(PersonalDetails details)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(_connectionString))
                {
                    using (MySqlCommand cmd = new MySqlCommand("InsertPersonalDetails", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@firstName", details.FirstName);
                        cmd.Parameters.AddWithValue("@lastName", details.LastName);
                        cmd.Parameters.AddWithValue("@email", details.Email);
                        cmd.Parameters.AddWithValue("@phone", details.Phone);
                        cmd.Parameters.AddWithValue("@address", details.Address);
                        cmd.Parameters.AddWithValue("@city", details.City);
                        cmd.Parameters.AddWithValue("@state", details.State);
                        cmd.Parameters.AddWithValue("@postalCode", details.PostalCode);

                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (MySqlException ex) when (ex.Number == 1062) // MySQL duplicate entry error code
            {
                throw new DuplicateEntryException("Duplicate entry error: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error inserting personal details: " + ex.Message);
            }
        }

        public List<PersonalDetails> GetAllPersonalDetails()
        {
            var detailsList = new List<PersonalDetails>();

            try
            {
                using (MySqlConnection conn = new MySqlConnection(_connectionString))
                {
                    string query = "SELECT * FROM PersonalDetails";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        conn.Open();
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                detailsList.Add(new PersonalDetails
                                {
                                    Id = Convert.ToInt32(reader["Id"]),
                                    FirstName = reader["FirstName"].ToString(),
                                    LastName = reader["LastName"].ToString(),
                                    Email = reader["Email"].ToString(),
                                    Phone = reader["Phone"].ToString(),
                                    Address = reader["Address"].ToString(),
                                    City = reader["City"].ToString(),
                                    State = reader["State"].ToString(),
                                    PostalCode = reader["PostalCode"].ToString(),
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching personal details: " + ex.Message);
            }

            return detailsList;
        }

        public PersonalDetails GetPersonalDetailsById(int id)
        {
            PersonalDetails details = null;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(_connectionString))
                {
                    string query = "SELECT * FROM PersonalDetails WHERE Id = @Id";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Id", id);
                        conn.Open();
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                details = new PersonalDetails
                                {
                                    Id = Convert.ToInt32(reader["Id"]),
                                    FirstName = reader["FirstName"].ToString(),
                                    LastName = reader["LastName"].ToString(),
                                    Email = reader["Email"].ToString(),
                                    Phone = reader["Phone"].ToString(),
                                    Address = reader["Address"].ToString(),
                                    City = reader["City"].ToString(),
                                    State = reader["State"].ToString(),
                                    PostalCode = reader["PostalCode"].ToString(),
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching personal details by ID: " + ex.Message);
            }

            return details;
        }

        public void UpdatePersonalDetails(PersonalDetails details)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(_connectionString))
                {
                    using (MySqlCommand cmd = new MySqlCommand("UpdatePersonalDetails", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@p_id", details.Id); // Ensure the parameter name matches your stored procedure
                        cmd.Parameters.AddWithValue("@p_firstName", details.FirstName);
                        cmd.Parameters.AddWithValue("@p_lastName", details.LastName);
                        cmd.Parameters.AddWithValue("@p_email", details.Email);
                        cmd.Parameters.AddWithValue("@p_phone", details.Phone);
                        cmd.Parameters.AddWithValue("@p_address", details.Address);
                        cmd.Parameters.AddWithValue("@p_city", details.City);
                        cmd.Parameters.AddWithValue("@p_state", details.State);
                        cmd.Parameters.AddWithValue("@p_postalCode", details.PostalCode);

                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (MySqlException ex) when (ex.Number == 1062)
            {
                throw new DuplicateEntryException("Duplicate entry error: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating personal details: " + ex.Message);
            }
        }

        public bool DeletePersonalDetails(int id)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(_connectionString))
                {
                    using (MySqlCommand cmd = new MySqlCommand("DELETE FROM PersonalDetails WHERE Id = @Id", conn))
                    {
                        cmd.Parameters.AddWithValue("@Id", id);
                        conn.Open();
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting personal details: " + ex.Message);
            }
        }
    }

    public class DuplicateEntryException : Exception
    {
        public DuplicateEntryException(string message) : base(message) { }
    }
}
