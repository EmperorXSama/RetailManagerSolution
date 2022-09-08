using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Caliburn.Micro;
using Microsoft.Identity.Client;
using RMWPFUserInterface.Library.Models;
using RMWPFUserInterfice.EventModels;

namespace RMWPFUserInterfice.ViewModels;

public sealed class ShellViewModel : Conductor<object> , IHandle<LogOnEventModel>
{
    private readonly IEventAggregator _eventHandler;
    private readonly SalesViewModel _salesVm;
    private readonly ILoggedInUserModel _loggedInUser;

    public ShellViewModel(IEventAggregator eventHandler , SalesViewModel salesVm , ILoggedInUserModel loggedInUser)
    {
        _eventHandler = eventHandler;
        _salesVm = salesVm;
        _loggedInUser = loggedInUser;

        _eventHandler.Subscribe(this);
        
        ActivateItem(IoC.Get<LoginViewModel>());
    }

    public void Handle(LogOnEventModel message)
    {
        ActivateItem(_salesVm);
        NotifyOfPropertyChange(() => IsAccountVisible);
    }

    public void ExitApplication()
    {
        TryClose();
    }

    public bool IsAccountVisible
    {
        get
        {
            bool output = string.IsNullOrWhiteSpace(_loggedInUser.Token) == false;

            return output;
        }
    }

    public  async void Logout(object sender, RoutedEventArgs e)
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
        
        _loggedInUser.ResetUserWhenLogout();
        ActivateItem(IoC.Get<LoginViewModel>());
        NotifyOfPropertyChange(() => IsAccountVisible);
    }
}