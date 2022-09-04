using RetailManagerDataManagemeent.Api;
using RM.Library.Internal.DataAccess;
using RM.Library.Models;

namespace RM.Library.DataAccesss;

public class ProductData : IProductData
{
    private readonly ISqlDataAccess _db;

    public ProductData(ISqlDataAccess db)
    {
        _db = db;
    }
    
    // get all the products from our sql database 
    public List<ProductModel> GetAllProducts()
    {
        var output = _db.LoadData<ProductModel,dynamic>("dbo.spProduct_GetAll", new { }, StringConstants.SqlConnectionName);

        return output;
    }
     
}