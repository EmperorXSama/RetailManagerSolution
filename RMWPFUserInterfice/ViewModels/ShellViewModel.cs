using Caliburn.Micro;
using RMWPFUserInterfice.EventModels;

namespace RMWPFUserInterfice.ViewModels;

public sealed class ShellViewModel : Conductor<object> , IHandle<LogOnEventModel>
{
    private readonly IEventAggregator _eventHandler;
    private readonly SalesViewModel _salesVm;

    public ShellViewModel(IEventAggregator eventHandler , SalesViewModel salesVm)
    {
        _eventHandler = eventHandler;
        _salesVm = salesVm;

        _eventHandler.Subscribe(this);
        
        ActivateItem(IoC.Get<LoginViewModel>());
    }

    public void Handle(LogOnEventModel message)
    {
        ActivateItem(_salesVm);
    }
}