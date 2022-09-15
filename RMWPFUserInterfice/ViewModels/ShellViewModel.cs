using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Caliburn.Micro;
using Microsoft.Identity.Client;
using RMWPFUserInterface.Library.Models;
using RMWPFUserInterfice.EventModels;

namespace RMWPFUserInterfice.ViewModels;

public sealed class ShellViewModel : Conductor<object> , IHandle<LogOnEventModel>
{
    private readonly IEventAggregator _eventHandler;
    private readonly ILoggedInUserModel _loggedInUser;

    public ShellViewModel(IEventAggregator eventHandler , ILoggedInUserModel loggedInUser)
    {
        _eventHandler = eventHandler;
        _loggedInUser = loggedInUser;

        _eventHandler.SubscribeOnPublishedThread(this);
        
        ActivateItemAsync(IoC.Get<LoginViewModel>(), new CancellationToken());
    }

    public void ExitApplication()
    {
        TryCloseAsync();
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
        await ActivateItemAsync(IoC.Get<LoginViewModel>() , new CancellationToken());
        NotifyOfPropertyChange(() => IsAccountVisible);
    }

    public async  Task HandleAsync(LogOnEventModel message, CancellationToken cancellationToken)
    {
        await ActivateItemAsync(IoC.Get<SalesViewModel>(), cancellationToken);
        NotifyOfPropertyChange(() => IsAccountVisible);
    }
}