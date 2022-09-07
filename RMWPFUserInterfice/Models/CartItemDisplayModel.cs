using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RMWPFUserInterfice.Models;

public class CartItemDisplayModel : INotifyPropertyChanged
{
    public ProductDisplayModel Product { get; set; } = new();
    public int _quantityInCart;
    public int QuantityInCart
    {
        get => _quantityInCart;
        set
        {
            _quantityInCart = value;
            CallPropertyChanged(nameof(QuantityInCart));
            CallPropertyChanged(nameof(DisplayText));
        }
        
    }

    public string DisplayText
    {
        get { return $"{Product.ProductName} ({QuantityInCart})"; }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void CallPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}