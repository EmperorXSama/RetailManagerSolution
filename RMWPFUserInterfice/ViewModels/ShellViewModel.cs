using Caliburn.Micro;
using RMWPFUserInterfice.EventModels;

namespace RMWPFUserInterfice.ViewModels;

public sealed class ShellViewModel : Conductor<object> , IHandle<LogOnEventModel>
{
    private readonly IEventAggregator _eventHandler;
    private readonly SalesViewModel _salesVm;
    private readonly SimpleContainer _container;

    public ShellViewModel(IEventAggregator eventHandler , SalesViewModel salesVm, 
        SimpleContainer container)
    {
        _eventHandler = eventHandler;
        _salesVm = salesVm;
        _container = container;
        
        _eventHandler.Subscribe(this);
        
        ActivateItem(_container.GetInstance<LoginViewModel>());
    }

    public void Handle(LogOnEventModel message)
    {
        ActivateItem(_salesVm);
    }
}