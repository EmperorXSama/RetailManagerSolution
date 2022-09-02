using RM.Library.Internal.DataAccess;
using RM.Library.Models;
using Newtonsoft.Json.Linq;
using RetailManagerDataManagemeent.Api;

namespace RM.Library.DataAccesss;

// Crud Operation for the user model 
public class UserData : IUserData
{
    private readonly ISqlDataAccess _db;

    public UserData(ISqlDataAccess db)
    {
        _db = db;
    }
    //get user by id
    public UserModel GetUserById(string id)
    {

        var p = new { Id = id };
        var output = _db.LoadData<UserModel, dynamic>("dbo.spUserLookUp", p, StringConstants.SqlConnectionName).FirstOrDefault();

        return output;


    }

    //post user 
    public async Task<int> CreateUser(UserModel user)
    {
        var person = new
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            EmailAddress = user.EmailAddress
        };

       var result =  await _db.SaveData("dbo.spUser_Insert", person, StringConstants.SqlConnectionName);
       return result;
    }
    
}