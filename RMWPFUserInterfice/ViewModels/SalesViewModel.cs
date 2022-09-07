﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
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
        IConfigHelper configHelper,ISaleEndPoint saleEndPoint , IMapper mapper)
    {
        _productEndPoint = productEndPoint;
        _loggerUser = loggerUser;
        _selectedProduct = selectedProduct;
        _configHelper = configHelper;
        _saleEndPoint = saleEndPoint;
        _mapper = mapper;
    }


    protected override async  void OnViewLoaded(object view)
    {
        base.OnViewLoaded(view);
        await LoadData();
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
    private ProductDisplayModel _selectedProduct;
    private readonly IConfigHelper _configHelper;
    private readonly ISaleEndPoint _saleEndPoint;
    private readonly IMapper _mapper;

    public ProductDisplayModel SelectedProduct
    {
        get => _selectedProduct;
        set
        {
            _selectedProduct = value;
            NotifyOfPropertyChange(()=> SelectedProduct);
            NotifyOfPropertyChange(()=> CanAddToCarts);
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
            NotifyOfPropertyChange(()=> CanAddToCarts);
        }
    }

    #endregion

    #region Funtions 
    public bool CanAddToCarts
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

            // Make sure there is something selected 
            return output;
        }
    }

    public void RemoveFromCart()
    {
        NotifyOfPropertyChange(()=> SubTotal);
        NotifyOfPropertyChange(()=> Tax);
        NotifyOfPropertyChange(()=> Total);
        NotifyOfPropertyChange(()=> CanCheckOut);
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
    #endregion
    
}