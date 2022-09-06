namespace RMWPFUserInterface.Library.Models;

public class ProductsModel : IProductsModel
{
    public int Id { get; set; }
    public string ProductName { get; set; } = "";
    public string Description { get; set; } = "";
    public decimal RetailPrice { get; set; }
    public int QuantityInStock { get; set; }
    public bool IsTaxable { get; set; }
}