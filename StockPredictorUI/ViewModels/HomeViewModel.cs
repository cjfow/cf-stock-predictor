using System.ComponentModel;
using System.Windows;
using StockPredictorUI.Views;
using StockPredictorUI.Services;
using CommunityToolkit.Mvvm.Input;

namespace StockPredictorUI.ViewModels;

/// <summary>
/// gathers and manages user input for the ticker and prediction horizon, handles UI commands
/// </summary>
public class HomeViewModel : INotifyPropertyChanged
{
    private string _stockTicker = "QQQ";
    private int _predictionHorizon = 1;

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

    private readonly APIDataAccess _apiDataAccess;

    public HomeViewModel()
    {
        OpenChartCommand = new RelayCommand(OpenChart);
        OpenTickerListCommand = new RelayCommand(OpenTickerList);
        MinimizeCommand = new RelayCommand(OnMinimize);
        CloseCommand = new RelayCommand(OnClose);

        _apiDataAccess = new APIDataAccess();
    }

    private async void OpenChart()
    {
        // TODO: pull relevant tickers from a db
        var validTickers = new HashSet<string> { "SPY", "QQQ", "IWM", "EFA", "EEM", "VFIAX", "FXAIX", "VTSAX", "VTIAX", "SWPPX" };

        if (!validTickers.Contains(StockTicker))
        {
            MessageBox.Show("Invalid stock ticker. Please enter a supported ticker.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        List<float> predictionData = await _apiDataAccess.GetStockDataAsync(StockTicker, PredictionHorizon);

        if (predictionData == null || predictionData.Count < PredictionHorizon * 250)
        {
            MessageBox.Show("Failed to fetch valid stock data. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        ChartView chartView = new(StockTicker, predictionData, PredictionHorizon);
        chartView.Show();
    }

    private void OpenTickerList()
    {
        TickerListView tickerListView = new();
        tickerListView.Show();
    }

    private void OnMinimize()
    {
        Application.Current.MainWindow.WindowState = WindowState.Minimized;
    }

    private void OnClose()
    {
        Application.Current.Shutdown();
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
