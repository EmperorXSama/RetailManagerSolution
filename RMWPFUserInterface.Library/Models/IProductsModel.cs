namespace RMWPFUserInterface.Library.Models;

public interface IProductsModel
{
    int Id { get; set; }
    string ProductName { get; set; }
    string Description { get; set; }
    decimal RetailPrice { get; set; }
    int QuantityInStock { get; set; }
}