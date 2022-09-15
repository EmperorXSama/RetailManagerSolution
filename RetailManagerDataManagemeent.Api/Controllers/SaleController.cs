using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using RM.Library.DataAccesss;
using RM.Library.Models;

namespace RetailManagerDataManagemeent.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class SaleController : Controller
{
    private readonly ISaleData _saleData;

    public SaleController(ISaleData saleData)
    {
        _saleData = saleData;
    }
    
    
    [HttpPost]
    [Authorize(Policy = "Cashier")]
    public async Task  Post(SalesModel sale)
    {
        await _saleData.SaveSale(sale);
    }

    [HttpGet]
    [Route("GetSalesReport")]
    [Authorize(Policy = "Admin")]
    public List<SaleReportModel> GetSaleReport()
    {
        return _saleData.GetSaleReport();
    }

}