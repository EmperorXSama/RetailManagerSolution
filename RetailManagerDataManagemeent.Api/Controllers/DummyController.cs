using Microsoft.AspNetCore.Mvc;

namespace RetailManagerDataManagemeent.Api.Controllers;


[Route("api/[controller]")]
[ApiController]
public class DummyController : Controller
{
    //get a user from the data base 
    [HttpGet]
    public string Get()
    {
        return "hi";
    }
}