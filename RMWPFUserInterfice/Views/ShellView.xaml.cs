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

    #endregion

    
}