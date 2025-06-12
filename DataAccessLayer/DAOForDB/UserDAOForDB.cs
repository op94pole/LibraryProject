using EntityDataModel;
using EntityDataModel.Interfaces;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace DataAccessLayer
{
    public class UserDAOForDB : IUserDAO
    {
        private string _connectionString;

        public UserDAOForDB(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<User> GetUsers()
        {
            var users = new List<User>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (var command = new SqlCommand("SP_GetUsers", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var user = new User();

                            DALUtility<User>.UsersRead(user, reader);
                            users.Add(user);
                        }
                    }
                }
            }

            return users;
        }

        public User GetCurrentUser(string username, string password)
        {
            User currentUser = new User();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (var command = new SqlCommand("SP_GetCurrentUser", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Password", password);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DALUtility<User>.UsersRead(currentUser, reader);

                            return currentUser;
                        }
                    }
                }
            }

            return null;
        }
    }
}