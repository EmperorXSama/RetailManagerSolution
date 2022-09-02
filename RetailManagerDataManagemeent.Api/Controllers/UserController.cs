using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.Resource;
using Newtonsoft.Json.Linq;
using RM.Library.DataAccesss;
using RM.Library.Internal.DataAccess;
using RM.Library.Models;

namespace RetailManagerDataManagemeent.Api.Controllers;
[Route("api/[controller]")]
[RequiredScope("data.view")]
[ApiController]
[Authorize]
public class UserController : Controller
{
    private readonly IUserData _userData;

    public UserController(IUserData userData)
    {
        _userData = userData;
    }
    
    
    //get a user from the data base 
    [HttpGet("GetUserById/{id}")]
    public UserModel GetById(string id)
    {
        var output = _userData.GetUserById(id);
        return output;
    }
    
    //get a user from the data base 
    [HttpPost("CreateUser/{authenticatedUser}")]
    public async Task<bool>  CreateNewUser(JObject authenticatedUser)
    {
        UserModel newUser = new UserModel();
        // create the user 
        newUser.Id = authenticatedUser["oid"].ToString();
        newUser.FirstName = authenticatedUser["given_name"].ToString();
        newUser.LastName = authenticatedUser["family_name"].ToString();
        if (authenticatedUser["emails"] is JArray emails)
        {
            newUser.EmailAddress = emails[0].ToString();
        }
        
      var result = await _userData.CreateUser(newUser);

      return result != 0;
    }
        
        
    
}