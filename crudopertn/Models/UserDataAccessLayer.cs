using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace crudopertn.Models
{
    public class UserDataAccessLayer
    {

        private readonly string _connectionString;

        // Constructor reads appsettings.json
        public UserDataAccessLayer(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        //string connectionString = @"Server=localhost\SQLEXPRESS;Database=MyDB;Trusted_Connection=True;";

        public IEnumerable<User> GetAllUsers()
        {
            List<User> users = new List<User>();

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("spGetAllUsers", con);
                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    User user = new User();
                    user.ID = Convert.ToInt32(rdr["UserID"]);
                    user.Username = rdr["Username"].ToString();
                    user.Email = rdr["Email"].ToString();
                    user.Password = rdr["Passw"].ToString();

                    users.Add(user);
                }
            }
            return users;

        }

        public void AddUser(User user)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("spAddUser", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Username", user.Username);
                cmd.Parameters.AddWithValue("@Email", user.Email);
                cmd.Parameters.AddWithValue("@Passw", user.Password);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateUser(User user)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("spUpdateUser", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@UserID", user.ID);
                cmd.Parameters.AddWithValue("@Username", user.Username);
                cmd.Parameters.AddWithValue("@Email", user.Email);
                cmd.Parameters.AddWithValue("@Password", user.Password);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }



        public User GetUserById(int id)
        {
            User user = null;

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                string sqlQuery = "SELECT * FROM tblUser WHERE UserID = @UserID";
                SqlCommand cmd = new SqlCommand(sqlQuery, con);
                cmd.Parameters.AddWithValue("@UserID", id);

                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    // Instantiate the user object before mapping
                    user = new User
                    {
                        ID = rdr["UserID"] != DBNull.Value ? Convert.ToInt32(rdr["UserID"]) : 0,
                        Username = rdr["Username"] != DBNull.Value ? rdr["Username"].ToString() : string.Empty,
                        Email = rdr["Email"] != DBNull.Value ? rdr["Email"].ToString() : string.Empty,
                        Password = rdr["Passw"] != DBNull.Value ? rdr["Passw"].ToString() : string.Empty
                    };
                }
                //if (rdr.Read())
                //{
                //    user.ID = Convert.ToInt32(rdr["UserID"]);
                //    user.Username = rdr["Username"].ToString();
                //    user.Email = rdr["Email"].ToString();
                //    user.Password = rdr["Password"].ToString();
                //}
            }
            return user;
        }



        public bool UpdatedUser(User user)
        {
            var existingUser = GetUserById(user.ID);
            if (existingUser == null)
                return false;

            using (SqlConnection con = new SqlConnection(_connectionString))
            {

                string sqlQuery = $@"UPDATE tbluser
                       SET Username = @Username,
                           Email = @Email,
                           Passw = @Password
                       WHERE UserID = @UserID";

                SqlCommand cmd = new SqlCommand(sqlQuery, con);
                cmd.Parameters.AddWithValue("@UserID", user.ID);
                cmd.Parameters.AddWithValue("@Username", user.Username);
                cmd.Parameters.AddWithValue("@Email", user.Email);
                cmd.Parameters.AddWithValue("@Password", user.Password);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            return true;
        }



        public bool DeletedUser(int id)
        {
            // 1. Check if user exists
            var existingUser = GetUserById(id);
            if (existingUser == null)
                return false;

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                string sqlQuery = "DELETE FROM tblUser WHERE UserID = @UserID";
                SqlCommand cmd = new SqlCommand(sqlQuery, con);
                cmd.Parameters.AddWithValue("@UserID", id);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            return true; // delete successful
        }




        public void DeleteUser(int id)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("spDeleteUser", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@UserID", id);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

    }
}
