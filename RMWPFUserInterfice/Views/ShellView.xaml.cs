using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using Microsoft.Identity.Client;
using Newtonsoft.Json.Linq;

namespace RMWPFUserInterfice.Views;

public partial class ShellView : Window
{
    HttpClient httpClient = new HttpClient();
    public ShellView()
    {
        InitializeComponent();
    }

    #region Configure login , logout , calling api methods 

    private async void SignOutButton_Click(object sender, RoutedEventArgs e)
    {
        // SingOut will remove tokens from the token cache from ALL accounts, irrespective of user flow
        IEnumerable<IAccount> accounts = await App.PublicClientApp.GetAccountsAsync();
        try
        {
            while (accounts.Any())
            {
                await App.PublicClientApp.RemoveAsync(accounts.FirstOrDefault());
                accounts = await App.PublicClientApp.GetAccountsAsync();
            }
            
        }
        catch (MsalException ex)
        {
            //ResultText.Text = $"Error signing-out user: {ex.Message}";
        }
    }
    private async void SignInButton_Click(object sender, RoutedEventArgs e)
    {
        AuthenticationResult authResult = null;
        var app = App.PublicClientApp;
        try
        {
            //ResultText.Text = "";
            authResult = await app.AcquireTokenInteractive(App.ApiScopes)
                .WithParentActivityOrWindow(new WindowInteropHelper(this).Handle)
                .ExecuteAsync();
            

        }
        catch (MsalException ex)
        {
            try
            {
                if (ex.Message.Contains("AADB2C90118"))
                {
                    authResult = await app.AcquireTokenInteractive(App.ApiScopes)
                        .WithParentActivityOrWindow(new WindowInteropHelper(this).Handle)
                        .WithPrompt(Prompt.SelectAccount)
                        .WithB2CAuthority(App.AuthorityResetPassword)
                        .ExecuteAsync();
                }
                else
                {
                    //ResultText.Text = $"Error Acquiring Token:{Environment.NewLine}{ex}";
                }
            }
            catch (Exception exe)
            {
                //ResultText.Text = $"Error Acquiring Token:{Environment.NewLine}{exe}";
            }
        }
        catch (Exception ex)
        {
            //ResultText.Text = $"Error Acquiring Token:{Environment.NewLine}{ex}";
        }
        
    }
    
    public async Task<string> GetHttpContentWithToken(string url, string token)
    {
        var httpClient = new HttpClient();
        HttpResponseMessage response;
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            response = await httpClient.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            return content;
        }
        catch (Exception ex)
        {
            return ex.ToString();
        }
    }
    private async  void GetUserById(object sender, RoutedEventArgs e)
    {
        AuthenticationResult authResult = null;
        var app = App.PublicClientApp;
        var accounts = await app.GetAccountsAsync(App.PolicySignUpSignIn);
        try
        {
                
            authResult = await app.AcquireTokenSilent(App.ApiScopes, accounts.FirstOrDefault())
                .ExecuteAsync();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authResult.AccessToken);
        }
        catch (MsalUiRequiredException ex)
        {
            // A MsalUiRequiredException happened on AcquireTokenSilentAsync. 
            // This indicates you need to call AcquireTokenAsync to acquire a token
            Debug.WriteLine($"MsalUiRequiredException: {ex.Message}");

            try
            {
                authResult = await app.AcquireTokenInteractive(App.ApiScopes)
                    .WithParentActivityOrWindow(new WindowInteropHelper(this).Handle)
                    .ExecuteAsync();
            }
            catch (MsalException msalex)
            {
                //ResultText.Text = $"Error Acquiring Token:{Environment.NewLine}{msalex}";
            }
        }
        catch (Exception ex)
        {
            //ResultText.Text = $"Error Acquiring Token Silently:{Environment.NewLine}{ex}";
            return;
        }

        if (authResult != null)
        {
            if (string.IsNullOrEmpty(authResult.AccessToken))
            {
                //ResultText.Text = "Access token is null (could be expired). Please do interactive log-in again." ;
            }
            else
            {
                JObject user = ParseIdToken(authResult.IdToken);
                 var response = await GetHttpContentWithToken($"{App.GetUserEndPoint}/{user["oid"]?.ToString()}", authResult.AccessToken);

                 Info.Text += response;
            }
        }
    }



    JObject ParseIdToken(string idToken)
    {
        // Parse the idToken to get user info
        idToken = idToken.Split('.')[1];
        idToken = Base64UrlDecode(idToken);
        return JObject.Parse(idToken);
    }

    private string Base64UrlDecode(string s)
    {
        s = s.Replace('-', '+').Replace('_', '/');
        s = s.PadRight(s.Length + (4 - s.Length % 4) % 4, '=');
        var byteArray = Convert.FromBase64String(s);
        var decoded = Encoding.UTF8.GetString(byteArray, 0, byteArray.Count());
        return decoded;
    }

    #endregion

    
}