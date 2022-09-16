using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Windows;
using AutoMapper;
using Caliburn.Micro;
using Microsoft.Extensions.Configuration;
using RMWPFUserInterface.Library.Helpers;
using RMWPFUserInterface.Library.Models;
using RMWPFUserInterfice.Models;
using RMWPFUserInterfice.ViewModels;

namespace RMWPFUserInterfice;

public class Bootstrapper : BootstrapperBase
{
    private SimpleContainer _container = new SimpleContainer();
    public Bootstrapper()
    {
       Initialize(); 
    }

    private IMapper ConfigureAutoMapper()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<ProductsModel, ProductDisplayModel>();
            cfg.CreateMap<CartItemModel, CartItemDisplayModel>();
        });

        var output = config.CreateMapper();
        return output;
    }

    private IConfiguration AddConfiguration()
    {
        IConfigurationBuilder builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsittings.json");
        
        #if DEBUG
        builder.AddJsonFile("appsittings.Development.json", optional: true,reloadOnChange:true);
        #else
         builder.AddJsonFile("appsittings.Production.json", optional: true,reloadOnChange:true);
        #endif

        return builder.Build();
    }
    protected override void Configure()
    {
        _container.Instance(ConfigureAutoMapper());
        
        _container.Instance(_container).
            PerRequest<IProductEndPoint ,ProductEndPoint>().
            PerRequest<ISaleEndPoint ,SaleEndPoint>();
        
        _container
            .Singleton<IWindowManager, WindowManager>()
            .Singleton<IEventAggregator, EventAggregator>()
            .Singleton<ILoggedInUserModel,LoggedInUserModel>()
            .Singleton<HttpClient>()
            .Singleton<IApiHelper, ApiHelper>();
        
        _container.RegisterInstance(typeof(IConfiguration),"IConfiguration",AddConfiguration());
        
        GetType().Assembly.GetTypes()
            .Where(type => type.IsClass)
            .Where(type => type.Name.EndsWith("ViewModel"))
            .ToList()
            .ForEach(viewModelType => _container.RegisterPerRequest(
                viewModelType,viewModelType.ToString(),viewModelType));

    }

    protected override async  void OnStartup(object sender, StartupEventArgs e)
    {
        await  DisplayRootViewForAsync<ShellViewModel>();
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