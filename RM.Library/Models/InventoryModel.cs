namespace RM.Library.Models;

public class InventoryModel
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal PurchasedPrice { get; set; }
    public DateTime PurchasedDate { get; set; }
}