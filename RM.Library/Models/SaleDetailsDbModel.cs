namespace RM.Library.Models;

public class SaleDetailsDbModel
{
    public int SaleId { get; set; }
    public int ProductId { get; set; }
    public decimal PurchasedPrice { get; set; }
    public int Quantity { get; set; }

    public decimal Tax { get; set; }
}