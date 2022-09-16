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
[ApiController]
[RequiredScope("data.view")]
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
   
        
        
    
}