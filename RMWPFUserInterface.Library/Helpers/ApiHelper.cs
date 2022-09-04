using System.Configuration;
using System.Net.Http.Headers;
using RMWPFUserInterface.Library.Models;

namespace RMWPFUserInterface.Library.Helpers;

public class ApiHelper : IApiHelper
{
    private  ILoggedInUserModel _loggedInUser;
    private HttpClient _apiClient = new HttpClient();

    public HttpClient ApiClient => _apiClient;

    public ApiHelper(ILoggedInUserModel loggedInUser)
    {
        _loggedInUser = loggedInUser;
        InitializeClient();
    }

    private void InitializeClient()
    {
        string api = ConfigurationManager.AppSettings["api"]!;

        _apiClient.BaseAddress = new Uri(api);
        _apiClient.DefaultRequestHeaders.Accept.Clear();
        _apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

    }

    public async Task<ILoggedInUserModel> GetLoggedInUserInfo(string stringId , string token)
    {
        _apiClient.DefaultRequestHeaders.Clear();
        _apiClient.DefaultRequestHeaders.Accept.Clear();
        _apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        _apiClient.DefaultRequestHeaders.Add("Authorization",$"Bearer {token}");

        using (HttpResponseMessage response = await _apiClient.GetAsync($"api/User/GetUserById/{stringId}"))
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