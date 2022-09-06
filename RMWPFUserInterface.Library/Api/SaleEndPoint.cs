using System.Net.Http.Headers;
using RMWPFUserInterface.Library.Models;

namespace RMWPFUserInterface.Library.Helpers;

public class SaleEndPoint : ISaleEndPoint
{
    private readonly IApiHelper _apiHelper;

    public SaleEndPoint(IApiHelper apiHelper)
    {
        _apiHelper = apiHelper;
    }

    public async Task PostSale(SalesModel sale , string token)
    {
        _apiHelper.ApiClient.DefaultRequestHeaders.Clear();
        _apiHelper.ApiClient.DefaultRequestHeaders.Accept.Clear();
        _apiHelper.ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        _apiHelper.ApiClient.DefaultRequestHeaders.Add("Authorization",$"Bearer {token}");
        
        
        using (HttpResponseMessage response = await _apiHelper.ApiClient.PostAsJsonAsync("/api/Sale",sale)) 
        {
            if (response.IsSuccessStatusCode)
            {
                // Todo : Log successful call !
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }
        }
    }
    
}