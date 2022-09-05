namespace RMWPFUserInterface.Library.Models;

public class CartItemModel
{
    public ProductsModel Product { get; set; } = new();
    public int QuantityInCart { get; set; }

    public string DisplayText
    {
        get { return $"{Product.ProductName} ({QuantityInCart})"; }
    }
}