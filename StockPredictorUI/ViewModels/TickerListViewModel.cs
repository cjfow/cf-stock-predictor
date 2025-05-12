using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace StockPredictorUI.ViewModels;

/// <summary>
/// displays valid tickers for the prediction model
/// </summary>
class TickerListViewModel : ObservableObject
{
    public TickerListViewModel()
    {
        CloseCommand = new RelayCommand(OnClose);

        ETFList = ["SPY", "QQQ", "IWM", "EFA", "EEM"];
        IndexFundList = ["VFIAX", "FXAIX", "VTSAX", "VTIAX", "SWPPX"];
    }

    public ICommand CloseCommand { get; }

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

    public ObservableCollection<string>? ETFList { get; }
    public ObservableCollection<string>? IndexFundList { get; }
}