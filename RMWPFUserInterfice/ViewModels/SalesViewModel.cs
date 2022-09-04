using System;
using System.ComponentModel;
using Caliburn.Micro;

namespace RMWPFUserInterfice.ViewModels;

public class SalesViewModel : Screen
{
    private BindingList<string> _products;

    public BindingList<string> Products
    {
        get => _products;
        set
        {
            _products = value;
            NotifyOfPropertyChange(()=> Products);
        }
    }

    private BindingList<string> _cart;

    public BindingList<string> Cart
    {
        get => _cart;
        set
        {
            _cart = value;
            NotifyOfPropertyChange(()=> Cart);
        }
    }

    public string SubTotal
    {
        get
        {
            return "$0.00";
        }
    } 
    public string Tax
    {
        get
        {
            return "$0.00";
        }
    } 
    public string Total
    {
        get
        {
            return "$0.00";
        }
    }
    public bool CanCheckOut
    {
        get
        {
            bool output = false;
            //Make sure there is something in the cart 
            return output;
        }
    }
    


    private string _itemQuantity;

    public string ItemQuantity
    {
        get => _itemQuantity;
        set
        {
            if (value == _itemQuantity) return;
            _itemQuantity = value;
            NotifyOfPropertyChange(()=> ItemQuantity);
        }
    }
    
    public bool CanAddToCart
    {
        get
        {
            bool output = false;
            
            //Make sure something is selected 
            //Make sure there is an item quantity
            return output;
        }
    }

    public void AddToCart()
    {
        
    }

    public bool CanRemoveFromCart
    {
        get
        {
            bool output = false;

            // Make sure there is something selected 
            return output;
        }
    }

    public void RemoveFromCart()
    {
        
    }
    
    
}