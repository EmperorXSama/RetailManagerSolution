using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RMWPFUserInterfice.Models;

public class ProductDisplayModel : INotifyPropertyChanged
{
    public int Id { get; set; }
    public string ProductName { get; set; } = "";
    public string Description { get; set; } = "";
    public decimal RetailPrice { get; set; }
    public int _quantityInStock;
    public int QuantityInStock
    {
        get => _quantityInStock;
        set
        {
            _quantityInStock = value;
            CallPropertyChanged(nameof(QuantityInStock));
        }
        
    }
    public bool IsTaxable { get; set; }
    
    
    
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void CallPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}