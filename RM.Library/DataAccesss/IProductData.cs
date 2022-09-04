using RM.Library.Models;

namespace RM.Library.DataAccesss;

public interface IProductData
{
    List<ProductModel> GetAllProducts();
}