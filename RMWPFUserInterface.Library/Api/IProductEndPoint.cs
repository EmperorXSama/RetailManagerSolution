using RMWPFUserInterface.Library.Models;

namespace RMWPFUserInterface.Library.Helpers;

public interface IProductEndPoint
{
    Task<List<ProductsModel>> GetAllProducts(string token);
}