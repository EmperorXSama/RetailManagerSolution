using RetailManagerDataManagemeent.Api;
using RM.Library.Internal.DataAccess;
using RM.Library.Models;

namespace RM.Library.DataAccesss;

public class InventoryData : IInventoryData
{
    private readonly ISqlDataAccess _db;
    private readonly IProductData _productData;
    private readonly IConfigHelper _configHelper;
    public InventoryData(ISqlDataAccess db , IProductData productData , IConfigHelper configHelper)
    {
        _db = db;
        _productData = productData;
        _configHelper = configHelper;
    }


    public List<InventoryModel> GetInventory()
    {
        var output = _db.LoadData<InventoryModel, dynamic>("spInventory_GetAll", new { },
            StringConstants.SqlConnectionName);

        return output;
    }

    public async Task SaveInventoryRecord(InventoryModel inventoryItem)
    {
       await  _db.SaveData("spInventory_Insert", inventoryItem, StringConstants.SqlConnectionName);
        
    }
}