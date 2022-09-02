using RM.Library.Models;

namespace RM.Library.DataAccesss;

public interface IUserData
{
    UserModel GetUserById(string id); 
    Task<int> CreateUser(UserModel user);
}