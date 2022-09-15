using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using AutoMapper;
using Caliburn.Micro;
using RMWPFUserInterface.Library.Helpers;
using RMWPFUserInterface.Library.Models;
using RMWPFUserInterfice.Models;

namespace RMWPFUserInterfice.ViewModels;

public class SalesViewModel : Screen
{
    private readonly IProductEndPoint _productEndPoint;
    private readonly ILoggedInUserModel _loggerUser;

    public SalesViewModel(IProductEndPoint productEndPoint , ILoggedInUserModel loggerUser, ProductDisplayModel selectedProduct,
        IConfigHelper configHelper,ISaleEndPoint saleEndPoint , IMapper mapper , StatusInfoViewModel status , IWindowManager windowManager)
    {
        _productEndPoint = productEndPoint;
        _loggerUser = loggerUser;
        _selectedProduct = selectedProduct;
        _configHelper = configHelper;
        _saleEndPoint = saleEndPoint;
        _mapper = mapper;
        _status = status;
        _windowManager = windowManager;
    }


    protected override async  void OnViewLoaded(object view)
    {
        base.OnViewLoaded(view);
        try
        {
            await LoadData();
        }
        catch (Exception e)
        {
            dynamic sittings = new ExpandoObject();
            sittings.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            sittings.Title = "System Error";
            sittings.ResizeMode = ResizeMode.NoResize;
            // catch the error 
            if (e.Message == "Forbidden")
            {
                _status.UpdateMessage("Unauthorized Access", "you don't have permission to interact with the sales window .");
                await _windowManager.ShowDialogAsync(_status,null,sittings);
            }
            else
            {
                _status.UpdateMessage("Fatal Error", e.Message);
                await _windowManager.ShowDialogAsync(_status,null,sittings);
            }
           
            await TryCloseAsync();
        }
        
    }

    private async Task LoadData()
    {
        var productList = await _productEndPoint.GetAllProducts(_loggerUser.Token);
        // map 
        var products = _mapper.Map<List<ProductDisplayModel>>(productList);
        Products = new BindingList<ProductDisplayModel>(products);
    }
    
    #region Binding List  Properties
    
    private BindingList<ProductDisplayModel> _products;
    public BindingList<ProductDisplayModel> Products
    {
        get => _products;
        set
        {
            _products = value;
            NotifyOfPropertyChange(()=> Products);
        }
    }
    //=======================================
    private readonly IConfigHelper _configHelper;
    private readonly ISaleEndPoint _saleEndPoint;
    private readonly IMapper _mapper;
    private readonly StatusInfoViewModel _status;
    private readonly IWindowManager _windowManager;

    private ProductDisplayModel _selectedProduct;

    public ProductDisplayModel SelectedProduct
    {
        get => _selectedProduct;
        set
        {
            _selectedProduct = value;
            NotifyOfPropertyChange(()=> SelectedProduct);
            NotifyOfPropertyChange(()=> CanAddToCart);
        }
    }

    
    private CartItemDisplayModel _selectedCartItem;

    public CartItemDisplayModel SelectedCartItem
    {
        get => _selectedCartItem;
        set
        {
            _selectedCartItem = value;
            NotifyOfPropertyChange(()=> SelectedCartItem);
            NotifyOfPropertyChange(()=> CanRemoveFromCart);
        }
    }
    //=======================================
    private BindingList<CartItemDisplayModel> _cart = new BindingList<CartItemDisplayModel>();

    public BindingList<CartItemDisplayModel> Cart
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
            bool output = Cart.Count > 0;
            return output;
        }
    }

    public async Task CheckOut()
    {
        // create sale model and post it to the api
        SalesModel sale = new SalesModel();
        sale.UserModel = _loggerUser.Id;
        foreach (var itemModel in Cart)
        {
            sale.SaleDetails.Add(new SaleDetailsModel
            {
                ProductId = itemModel.Product.Id,
                Quantity = itemModel.QuantityInCart
            });
        }

        await _saleEndPoint.PostSale(sale , _loggerUser.Token);
        await ResetSalesViewModel();
    }

    private int _itemQuantity = 1;
    public int ItemQuantity
    {
        get => _itemQuantity;
        set
        {
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
            if (ItemQuantity > 0 && SelectedProduct?.QuantityInStock  >= ItemQuantity)
            {
                return true;
            }
            return output;
        }
    }

    public void AddToCart()
    {

        CartItemDisplayModel existingItem = Cart.FirstOrDefault(x => x.Product == SelectedProduct);
        

        if (existingItem != null)
        {
            existingItem.QuantityInCart += ItemQuantity;
        }
        else
        {
            CartItemDisplayModel item = new CartItemDisplayModel()
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
        NotifyOfPropertyChange(()=> CanCheckOut);
    }

    public bool CanRemoveFromCart
    {
        get
        {
            bool output = false;

            if (SelectedCartItem != null && SelectedCartItem?.QuantityInCart >0)
            {
                // Make sure there is something selected 
                return true;
            }
            return output;
        }
    }

    public void RemoveFromCart()
    {
       
        SelectedCartItem.Product.QuantityInStock += 1;
        if (SelectedCartItem.QuantityInCart >1 )
        {
            SelectedCartItem.QuantityInCart -= 1;
        }
        else
        {
            Cart.Remove(SelectedCartItem);
        }
        
        NotifyOfPropertyChange(()=> SubTotal);
        NotifyOfPropertyChange(()=> Tax);
        NotifyOfPropertyChange(()=> Total);
        NotifyOfPropertyChange(()=> CanCheckOut);
        NotifyOfPropertyChange(()=> CanAddToCart);
    }

    private decimal CalculatingTaxAmount()
    {
        decimal taxAmount = 0;
        decimal taxRate = _configHelper.GetTaxRate()/100;

        foreach (var CartItemDisplayModel in Cart)
        {
            if (CartItemDisplayModel.Product.IsTaxable)
            {
                taxAmount += (CartItemDisplayModel.Product.RetailPrice * CartItemDisplayModel.QuantityInCart * taxRate);
            }
        }

        return taxAmount;
    }

    private decimal CalculatingSubTotal()
    {
        decimal subTotal = 0;

        foreach (var CartItemDisplayModel in Cart)
        {
            subTotal += (CartItemDisplayModel.Product.RetailPrice * CartItemDisplayModel.QuantityInCart);
        }

        return subTotal;
    }

    private async Task  ResetSalesViewModel()
    {
        Cart = new BindingList<CartItemDisplayModel>();
        
        await LoadData();
        
        NotifyOfPropertyChange(()=> SubTotal);
        NotifyOfPropertyChange(()=> Tax);
        NotifyOfPropertyChange(()=> Total);
        NotifyOfPropertyChange(()=> CanCheckOut);
    }
    #endregion
    
}