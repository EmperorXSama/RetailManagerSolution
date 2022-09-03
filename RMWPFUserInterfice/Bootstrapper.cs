﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Windows;
using Caliburn.Micro;
using RMWPFUserInterface.Library.Helpers;
using RMWPFUserInterface.Library.Models;
using RMWPFUserInterfice.ViewModels;

namespace RMWPFUserInterfice;

public class Bootstrapper : BootstrapperBase
{
    private SimpleContainer _container = new SimpleContainer();
    public Bootstrapper()
    {
       Initialize(); 
    }
    protected override void Configure()
    {
        _container.Instance(_container);
        _container
            .Singleton<IWindowManager, WindowManager>()
            .Singleton<IEventAggregator, EventAggregator>()
            .Singleton<ILoggedInUserModel,LoggedInUserModel>()
            .Singleton<HttpClient>()
            .Singleton<IApiHelper, ApiHelper>();
        
        GetType().Assembly.GetTypes()
            .Where(type => type.IsClass)
            .Where(type => type.Name.EndsWith("ViewModel"))
            .ToList()
            .ForEach(viewModelType => _container.RegisterPerRequest(
                viewModelType,viewModelType.ToString(),viewModelType));
    }

    protected override void OnStartup(object sender, StartupEventArgs e)
    {
        DisplayRootViewFor<ShellViewModel>();
    }

    protected override object GetInstance(Type service, string key)
    {
        return _container.GetInstance(service, key);
    }

    protected override IEnumerable<object> GetAllInstances(Type service)
    {
        return _container.GetAllInstances(service);
    }

    protected override void BuildUp(object instance)
    {
        _container.BuildUp(instance);
    }


}