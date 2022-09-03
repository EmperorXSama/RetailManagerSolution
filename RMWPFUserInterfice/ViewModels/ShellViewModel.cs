using Caliburn.Micro;

namespace RMWPFUserInterfice.ViewModels;

public sealed class ShellViewModel : Conductor<object>
{
    private readonly LoginViewModel _loginVm;

    public ShellViewModel(LoginViewModel loginVM)
    {
        _loginVm = loginVM;
        ActivateItem(loginVM);
    }
}