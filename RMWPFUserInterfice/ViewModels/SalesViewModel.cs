using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using Caliburn.Micro;
using RMWPFUserInterface.Library.Helpers;
using RMWPFUserInterface.Library.Models;

namespace RMWPFUserInterfice.ViewModels;

public class SalesViewModel : Screen
{
    private readonly IProductEndPoint _productEndPoint;
    private readonly ILoggedInUserModel _loggerUser;

    public SalesViewModel(IProductEndPoint productEndPoint , ILoggedInUserModel loggerUser, ProductsModel selectedProduct,IConfigHelper configHelper)
    {
        _productEndPoint = productEndPoint;
        _loggerUser = loggerUser;
        _selectedProduct = selectedProduct;
        _configHelper = configHelper;
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
    
    #region Binding List  Properties
    
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
    //=======================================
    private ProductsModel _selectedProduct;
    private readonly IConfigHelper _configHelper;

    public ProductsModel SelectedProduct
    {
        get => _selectedProduct;
        set
        {
            _selectedProduct = value;
            NotifyOfPropertyChange(()=> SelectedProduct);
            NotifyOfPropertyChange(()=> CanAddToCart);
        }
    }
    //=======================================
    private BindingList<CartItemModel> _cart = new BindingList<CartItemModel>();

    public BindingList<CartItemModel> Cart
    {
        get => _cart;
        set
        {
            _cart = value;
            NotifyOfPropertyChange(()=> Cart);
        }
    }

    #endregion

    #region Binding Properties

    public string SubTotal => CalculatingSubTotal().ToString("C");
    public string Tax => CalculatingTaxAmount().ToString("C");

    public string Total => (CalculatingSubTotal() + CalculatingTaxAmount()).ToString("C");

    public bool CanCheckOut
    {
        get
        {
            bool output = false;
            //Make sure there is something in the cart 
            return output;
        }
    }
    


    private int _itemQuantity = 1;
    public int ItemQuantity
    {
        get => _itemQuantity;
        set
        {
            if (value == _itemQuantity) return;
            _itemQuantity = value;
            NotifyOfPropertyChange(()=> ItemQuantity);
            NotifyOfPropertyChange(()=> CanAddToCart);
        }
    }

    #endregion

    #region Funtions 
    public bool CanAddToCart
    {
        get
        {
            bool output = false;
            
            //Make sure something is selected 
            //Make sure there is an item quantity
            if (ItemQuantity > 0 && SelectedProduct?.QuantityInStock  >= ItemQuantity )
            {
                return true;
            }
            return output;
        }
    }

    public void AddToCart()
    {

        CartItemModel existingItem = Cart.FirstOrDefault(x => x.Product == SelectedProduct);
        

        if (existingItem != null)
        {
            existingItem.QuantityInCart += ItemQuantity;
            Cart.Remove(existingItem);
            Cart.Add(existingItem);
        }
        else
        {
            CartItemModel item = new CartItemModel()
            {
                Product = SelectedProduct,
                QuantityInCart = ItemQuantity
            };
            Cart.Add(item);
        }

        SelectedProduct.QuantityInStock -= ItemQuantity;
        ItemQuantity = 1;
        NotifyOfPropertyChange(()=> SubTotal);
        NotifyOfPropertyChange(()=> Tax);
        NotifyOfPropertyChange(()=> Total);
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
        NotifyOfPropertyChange(()=> SubTotal);
        NotifyOfPropertyChange(()=> Tax);
        NotifyOfPropertyChange(()=> Total);
    }

    private decimal CalculatingTaxAmount()
    {
        decimal taxAmount = 0;
        decimal taxRate = _configHelper.GetTaxRate()/100;

        foreach (var cartItemModel in Cart)
        {
            if (cartItemModel.Product.IsTaxable)
            {
                taxAmount += (cartItemModel.Product.RetailPrice * cartItemModel.QuantityInCart * taxRate);
            }
        }

        return taxAmount;
    }

    private decimal CalculatingSubTotal()
    {
        decimal subTotal = 0;

        foreach (var cartItemModel in Cart)
        {
            subTotal += (cartItemModel.Product.RetailPrice * cartItemModel.QuantityInCart);
        }

        return subTotal;
    }
    #endregion
    
}