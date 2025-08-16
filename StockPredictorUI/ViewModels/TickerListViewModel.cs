using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StockPredictorUI.Services;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using StockPredictorUI.Models;

namespace StockPredictorUI.ViewModels;

public class TickerListViewModel : ObservableObject
{
    private readonly ITickerDataService _tickerDataService;

    public ObservableCollection<string> ETFList { get; } = [];
    public ObservableCollection<string> IndexFundList { get; } = [];
    public ICommand CloseCommand { get; }

    public TickerListViewModel(ITickerDataService tickerDataService)
    {
        _tickerDataService = tickerDataService;
        CloseCommand = new RelayCommand(OnClose);

        LoadTickerDataAsync();
    }

    private async void LoadTickerDataAsync()
    {
        try
        {
            List<TickerInfo> etfs = await _tickerDataService.GetTickersByCategoryAsync("ETF");
            List<TickerInfo> indexFunds = await _tickerDataService.GetTickersByCategoryAsync("IndexFund");

            foreach (var etf in etfs)
                ETFList.Add(etf.Symbol);

            foreach (var indexFund in indexFunds)
                IndexFundList.Add(indexFund.Symbol);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to load ticker data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void OnClose()
    {
        foreach (Window window in Application.Current.Windows)
        {
            if (window.DataContext == this)
            {
                window.Close();
                break;
            }
        }
    }
}