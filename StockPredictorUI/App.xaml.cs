using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StockPredictorUI.Services;
using StockPredictorUI.ViewModels;
using StockPredictorUI.Views;
using System.Windows;

namespace StockPredictorUI;

public partial class App : Application
{
    private IHost? _host;
    public IHost? Host => _host;

    protected override void OnStartup(StartupEventArgs e)
    {
        _host = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder()
            .ConfigureServices(ConfigureServices)
            .Build();

        var mainWindow = _host.Services.GetRequiredService<HomeView>();
        mainWindow.Show();

        base.OnStartup(e);
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<IStockConfiguration, StockConfiguration>();
        services.AddSingleton<IStockPredictionModel, AccordMLModel>();
        services.AddSingleton<IDataAccess, APIDataAccess>();
        services.AddSingleton<ITickerDataService, TickerDataService>();
        services.AddTransient<HomeViewModel>();
        services.AddTransient<HomeView>();
        services.AddTransient<TickerListViewModel>();
        services.AddTransient<TickerListView>();
    }

    protected override void OnExit(ExitEventArgs e)
    {
        _host?.Dispose();
        base.OnExit(e);
    }
}

