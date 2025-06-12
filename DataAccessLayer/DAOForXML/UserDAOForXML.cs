using EntityDataModel;
using EntityDataModel.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace DataAccessLayer
{
    public class UserDAOForXML : IUserDAO
    {
        private readonly string _xmlPath;

        public UserDAOForXML(string xmlPath)
        {
            _xmlPath = xmlPath;
        }

        public List<User> GetUsers()
        {
            List<User> usersList = DALUtility<User>.ReadFile("Users", "User"); 

            return usersList;
        }

        public User GetCurrentUser(string username, string password)
        {
            User currentUser = GetUsers().Where(u => u.Username == username && u.Password == password).FirstOrDefault(); 

            return currentUser;
        }
    }
}