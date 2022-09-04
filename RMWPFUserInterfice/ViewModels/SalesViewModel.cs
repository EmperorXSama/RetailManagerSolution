using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Caliburn.Micro;
using RMWPFUserInterface.Library.Helpers;
using RMWPFUserInterface.Library.Models;

namespace RMWPFUserInterfice.ViewModels;

public class SalesViewModel : Screen
{
    private readonly IProductEndPoint _productEndPoint;
    private readonly ILoggedInUserModel _loggerUser;

    public SalesViewModel(IProductEndPoint productEndPoint , ILoggedInUserModel loggerUser)
    {
        _productEndPoint = productEndPoint;
        _loggerUser = loggerUser;
    }

    protected override async  void OnViewLoaded(object view)
    {
        base.OnViewLoaded(view);
        await LoadData();
    }

    private async Task LoadData()
    {
        var productList = await _productEndPoint.GetAllProducts(_loggerUser.Token);
        Products = new BindingList<ProductsModel>(productList);
    }
    
    private BindingList<ProductsModel> _products;
    public BindingList<ProductsModel> Products
    {
        get => _products;
        set
        {
            _products = value;
            NotifyOfPropertyChange(()=> Products);
        }
    }

    
    private BindingList<ProductsModel> _cart;

    public BindingList<ProductsModel> Cart
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