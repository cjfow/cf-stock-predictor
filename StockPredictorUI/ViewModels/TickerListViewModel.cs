using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace StockPredictorUI.ViewModels;

class TickerListViewModel : ObservableObject
{
    public TickerListViewModel()
    {
        ETFList = ["SPY", "QQQ", "IWM", "EFA", "EEM"];
        IndexFundList = ["VFIAX", "FXAIX", "VTSAX", "VTIAX", "SWPPX"];
    }
    public ObservableCollection<string>? ETFList { get; }
    public ObservableCollection<string>? IndexFundList { get; }
}
