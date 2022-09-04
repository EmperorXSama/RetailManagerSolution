using System.Net.Http.Headers;
using RMWPFUserInterface.Library.Models;

namespace RMWPFUserInterface.Library.Helpers;

public class ProductEndPoint : IProductEndPoint
{
    private readonly IApiHelper _apiHelper;


    public ProductEndPoint(IApiHelper apiHelper)
    {
        _apiHelper = apiHelper;
    }
    public async Task<List<ProductsModel>> GetAllProducts(string token)
    {
        
        _apiHelper.ApiClient.DefaultRequestHeaders.Clear();
        _apiHelper.ApiClient.DefaultRequestHeaders.Accept.Clear();
        _apiHelper.ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        _apiHelper.ApiClient.DefaultRequestHeaders.Add("Authorization",$"Bearer {token}");
        
        using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync($"api/Products/GetAllProducts"))
        {
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsAsync<List<ProductsModel>>();
                
                return result;
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }
        }


    }
}