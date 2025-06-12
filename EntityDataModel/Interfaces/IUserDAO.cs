using System.Collections.Generic;

namespace EntityDataModel.Interfaces
{
    public interface IUserDAO
    {
        List<User> GetUsers();
        User GetCurrentUser(string username, string password);
    }
}