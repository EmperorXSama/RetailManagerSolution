using Microsoft.AspNetCore.Mvc;
using RM.Library.DataAccesss;
using RM.Library.Models;

namespace RetailManagerDataManagemeent.Api.Controllers;


[Route("api/[controller]")]
[ApiController]
public class InventoryController : Controller
{
    private readonly IInventoryData _inventoryData;

    public InventoryController(IInventoryData inventoryData)
    {
        _inventoryData = inventoryData;
    }

    
    [HttpGet]
    [Route("GetInventory")]
    public List<InventoryModel> GetInventory()
    {
        return _inventoryData.GetInventory();
    }

    [HttpPost]
    [Route("CreateInventory")]
    public async Task Post(InventoryModel item)
    {
         await _inventoryData.SaveInventoryRecord(item);
    }
}