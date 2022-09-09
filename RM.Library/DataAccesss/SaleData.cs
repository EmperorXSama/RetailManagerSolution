using System.Data;
using Dapper;
using RetailManagerDataManagemeent.Api;
using RM.Library.Internal.DataAccess;
using RM.Library.Models;


namespace RM.Library.DataAccesss;

public class SaleData : ISaleData
{
    private readonly ISqlDataAccess _db;
    private readonly IProductData _productData;
    private readonly IConfigHelper _configHelper;
    public SaleData(ISqlDataAccess db , IProductData productData , IConfigHelper configHelper)
    {
        _db = db;
        _productData = productData;
        _configHelper = configHelper;
    }

    // create two tables the sale table that will take the saleDbModel 
    // and the sale Details that will hold the saleDetailsDbModel
    public async Task SaveSale(SalesModel saleInfo)
    {
        
        List<SaleDetailsDbModel> details = new List<SaleDetailsDbModel>();
        foreach (var item in saleInfo.SaleDetails)
        {
            var taxRate = _configHelper.GetTaxRate();
            var detail = new SaleDetailsDbModel
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity
            };

            // get information about the product 
            var productInfo = _productData.GetProductById(item.ProductId);

            if (productInfo == null)
            {
                throw new Exception($"The product Id of {detail.ProductId} could not found in the database.");
            }

            detail.PurchasedPrice = (productInfo.RetailPrice * detail.Quantity);
            if (productInfo.IsTaxable)
            {
                detail.Tax = detail.PurchasedPrice * taxRate;
            }

            details.Add(detail);
        }

        SaleDbModel sale = new SaleDbModel
        {
            CashierId = saleInfo.UserModel,
            SubTotal = details.Sum(x=> x.PurchasedPrice),
            Tax = details.Sum(x=>x.Tax),
        
        };
        sale.Total = sale.SubTotal + sale.Tax;

        #region dynamic data to get the id back when storing this record

        var dynamicParameter = new DynamicParameters();
        dynamicParameter.Add("Id",dbType:DbType.Int32 , direction:ParameterDirection.Output);
        dynamicParameter.Add("CashierId" , sale.CashierId);
        dynamicParameter.Add("SaleDate" , sale.SaleDate);
        dynamicParameter.Add("SubTotal" , sale.SubTotal);
        dynamicParameter.Add("Tax" , sale.Tax);
        dynamicParameter.Add("Total" , sale.Total);


        #endregion

        using (_db)
        {
            try
            {
                _db.StartTransaction(StringConstants.SqlConnectionName);
                // creating the sale info record 
                var saleId = await  _db.SaveDataInTransaction("spSale_Insert", dynamicParameter);
                sale.Id = dynamicParameter.Get<int>("Id");

                // creating the sale Detail info record 
                foreach (var item in details)
                {
                    item.SaleId = sale.Id;
                    await _db.SaveDataInTransaction("spSaleDetail_Insert", new
                    {
                        SaleId = item.SaleId,
                        ProductId = item.ProductId,
                        PurchasedPrice = item.PurchasedPrice,
                        Quantity = item.Quantity,
                        Tax = item.Tax
                    });
                }
                _db.CommitTransaction();
            }
            catch (Exception e)
            {
                _db.RollBack();
                throw;
            }
        }

        
        
    }

    public List<SaleReportModel> GetSaleReport()
    {
        var output =
            _db.LoadData<SaleReportModel, dynamic>("spSale_SaleReport", new { }, StringConstants.SqlConnectionName);

        return output;
    }
}


















