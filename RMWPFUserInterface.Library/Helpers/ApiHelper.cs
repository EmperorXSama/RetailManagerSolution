using System.Configuration;
using System.Net.Http.Headers;
using RMWPFUserInterface.Library.Models;

namespace RMWPFUserInterface.Library.Helpers;

public class ApiHelper : IApiHelper
{
    private  ILoggedInUserModel _loggedInUser;
    public HttpClient ApiClient = new HttpClient();

    public ApiHelper(ILoggedInUserModel loggedInUser)
    {
        _loggedInUser = loggedInUser;
        InitializeClient();
    }

    public void InitializeClient()
    {
        string api = ConfigurationManager.AppSettings["api"]!;

        ApiClient.BaseAddress = new Uri(api);
        ApiClient.DefaultRequestHeaders.Accept.Clear();
        ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

    }

    public async Task<ILoggedInUserModel> GetLoggedInUserInfo(string stringId , string token)
    {
        ApiClient.DefaultRequestHeaders.Clear();
        ApiClient.DefaultRequestHeaders.Accept.Clear();
        ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        ApiClient.DefaultRequestHeaders.Add("Authorization",$"Bearer {token}");

        using (HttpResponseMessage response = await ApiClient.GetAsync($"api/User/GetUserById/{stringId}"))
        {
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsAsync<LoggedInUserModel>();

                _loggedInUser.Token = token;
                _loggedInUser.FirstName = result.FirstName;
                _loggedInUser.LastName = result.LastName;
                _loggedInUser.EmailAddress = result.EmailAddress;
                _loggedInUser.Id = result.Id;
                _loggedInUser.CreatedDate = result.CreatedDate;
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }
        }

        return _loggedInUser;
    }
}