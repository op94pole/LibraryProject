using DataAccessLayer;
using EntityDataModel;
using EntityDataModel.Interfaces;
using System;

namespace BusinessLogic
{
    public class UserBL
    {
        //private static string _xmlPath;
        //private IUserDAO userDAO;

        //public UserBL(string xmlPath)
        //{
        //    _xmlPath = xmlPath;
        //    userDAO = new UserDAOForXML(_xmlPath);
        //}

        private static string _connectionString;
        private IUserDAO userDAO;

        public UserBL(string connectionString)
        {
            _connectionString = connectionString;
            userDAO = new UserDAOForDB(_connectionString);
        }

        public bool ValidateLogin(string username, string password, out User currentUser)
        {
            currentUser = new User();

            try
            {
                currentUser = userDAO.GetCurrentUser(username, password);
            }
            catch (Exception ex) { }

            if (currentUser == null)
            {
                throw new Exception("No matching username and password.");
            }

            return true;
        }
    }
}