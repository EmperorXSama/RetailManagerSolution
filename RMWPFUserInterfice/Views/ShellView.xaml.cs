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
using RMWPFUserInterface.Library.Helpers;
using RMWPFUserInterface.Library.Models;

namespace RMWPFUserInterfice.Views;

public partial class ShellView : Window
{
    
    private readonly ILoggedInUserModel _loggedInUserModel = new LoggedInUserModel();
    private IApiHelper _apiHelper;
    private HttpClient _httpClient =  new HttpClient();

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
        _apiHelper = new ApiHelper(_loggedInUserModel);
        AuthenticationResult authResult = null;
        var app = App.PublicClientApp;
        try
        {
            //ResultText.Text = "";
            authResult = await app.AcquireTokenInteractive(App.ApiScopes)
                .WithParentActivityOrWindow(new WindowInteropHelper(this).Handle)
                .ExecuteAsync();
            
            // capture more information about the user 
            await _apiHelper.GetLoggedInUserInfo(authResult.UniqueId, authResult.AccessToken);

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

    #endregion

    
}