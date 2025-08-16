using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using StockPredictorUI.Services;
using StockPredictorUI.Views;
using System.ComponentModel;
using System.Windows;

namespace StockPredictorUI.ViewModels;

public class HomeViewModel : INotifyPropertyChanged
{
    private string _stockTicker = "QQQ";
    private int _predictionHorizon = 1;
    private readonly IDataAccess _dataAccess;
    private readonly ITickerDataService _tickerDataService;

    public string StockTicker
    {
        get => _stockTicker;
        set
        {
            if (_stockTicker != value)
            {
                _stockTicker = value;
                OnPropertyChanged(nameof(StockTicker));
            }
        }
    }

    public int PredictionHorizon
    {
        get => _predictionHorizon;
        set
        {
            if (_predictionHorizon != value)
            {
                _predictionHorizon = value;
                OnPropertyChanged(nameof(PredictionHorizon));
            }
        }
    }

    public RelayCommand OpenChartCommand { get; }
    public RelayCommand OpenTickerListCommand { get; }
    public RelayCommand MinimizeCommand { get; }
    public RelayCommand CloseCommand { get; }

    public HomeViewModel(IDataAccess dataAccess, ITickerDataService tickerDataService)
    {
        _dataAccess = dataAccess;
        _tickerDataService = tickerDataService;
        OpenChartCommand = new RelayCommand(OpenChart);
        OpenTickerListCommand = new RelayCommand(OpenTickerList);
        MinimizeCommand = new RelayCommand(OnMinimize);
        CloseCommand = new RelayCommand(OnClose);
    }

    private async void OpenChart()
    {
        try
        {
            List<string> validTickers = await _tickerDataService.GetValidTickerSymbolsAsync();
            HashSet<string> validTickerSet = validTickers.ToHashSet(StringComparer.OrdinalIgnoreCase);

            if (!validTickerSet.Contains(StockTicker))
            {
                MessageBox.Show("Invalid stock ticker. Please enter a supported ticker.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            List<double> predictionData = await _dataAccess.GetStockDataAsync(StockTicker, PredictionHorizon);
            ChartView chartView = new(StockTicker, predictionData, PredictionHorizon);
            chartView.Show();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to fetch stock data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void OpenTickerList()
    {
        App app = (App)Application.Current;
        TickerListView? tickerListView = app.Host?.Services.GetRequiredService<TickerListView>();
        tickerListView?.Show();
    }

    private void OnMinimize() => Application.Current.MainWindow.WindowState = WindowState.Minimized;

    private void OnClose() => Application.Current.Shutdown();

    public event PropertyChangedEventHandler? PropertyChanged;
    
    protected virtual void OnPropertyChanged(string propertyName) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}