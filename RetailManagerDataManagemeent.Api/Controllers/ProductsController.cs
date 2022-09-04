using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using RM.Library.DataAccesss;
using RM.Library.Models;

namespace RetailManagerDataManagemeent.Api.Controllers;


[ApiController]
[Route("api/[controller]")]
[Authorize]
[RequiredScope("data.view")]
public class ProductsController : Controller
{
    private readonly IProductData _productData;

    public ProductsController(IProductData productData)
    {
        _productData = productData;
    }

    [HttpGet("GetAllProducts")]
    public List<ProductModel> GetProduct()
    {
        var output = _productData.GetAllProducts();

        return output;
    }

}