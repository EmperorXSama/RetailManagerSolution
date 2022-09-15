using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using Caliburn.Micro;
using Microsoft.Identity.Client;
using RMWPFUserInterface.Library.Helpers;
using RMWPFUserInterface.Library.Models;
using RMWPFUserInterfice.EventModels;
using RMWPFUserInterfice.Views;

namespace RMWPFUserInterfice.ViewModels;

public class LoginViewModel : Screen
{
    private readonly ILoggedInUserModel _loggedInUserModel;
    private readonly IEventAggregator _eventHandler;
    private IApiHelper _apiHelper;

    public LoginViewModel(IApiHelper apiHelper , ILoggedInUserModel loggedInUserModel , IEventAggregator eventHandler)
    {

        _apiHelper = apiHelper;
        _loggedInUserModel = loggedInUserModel;
        _eventHandler = eventHandler;

    }
    public async Task LogIn(object sender, RoutedEventArgs e)
    {
        _apiHelper = new ApiHelper(_loggedInUserModel);
        AuthenticationResult authResult = null;

        var app = App.PublicClientApp;
        

        try
        {
            //ResultText.Text = "";
            authResult = await app.AcquireTokenInteractive(App.ApiScopes)
                .WithParentActivityOrWindow(new WindowInteropHelper(new ShellView()).Handle)
                .ExecuteAsync();
            
            // capture more information about the user
           
            await _apiHelper.GetLoggedInUserInfo(authResult.UniqueId, authResult.AccessToken);
            
            // creating the event handler for the after logged in success 
            await _eventHandler.PublishOnUIThreadAsync(new LogOnEventModel() , new CancellationToken());
        }
        catch (MsalException ex)
        {
            try
            {
                if (ex.Message.Contains("AADB2C90118"))
                {
                    authResult = await app.AcquireTokenInteractive(App.ApiScopes)
                        .WithParentActivityOrWindow(new WindowInteropHelper(new ShellView()).Handle)
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
}































