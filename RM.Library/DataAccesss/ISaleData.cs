using RM.Library.Models;

namespace RM.Library.DataAccesss;

public interface ISaleData
{
    Task SaveSale(SalesModel saleInfo);
    List<SaleReportModel> GetSaleReport();
}