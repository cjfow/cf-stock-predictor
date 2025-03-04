using CommunityToolkit.Mvvm.Input;
using System.Windows;
using StockPredictorUI.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Input;

namespace StockPredictorUI.ViewModels;

public class HomeViewModel : ObservableObject
{
    public HomeViewModel()
    {
        OpenChartCommand = new RelayCommand(OpenChart);
        OpenTickerListCommand = new RelayCommand(OpenTickerList);
        MinimizeCommand = new RelayCommand(OnMinimize);
        CloseCommand = new RelayCommand(OnClose);
    }

    public RelayCommand OpenChartCommand { get; }
    public RelayCommand OpenTickerListCommand { get; }
    public ICommand MinimizeCommand { get; }
    public ICommand CloseCommand { get; }

    private void OpenTickerList()
    {
        TickerListView tickerListView = new();
        tickerListView.Show();
    }

    private void OpenChart()
    {
        ChartView chartView = new();
        chartView.Show();   
    }

    private void OnMinimize()
    {
        Window window = Application.Current.MainWindow;
        if (window != null)
        {
            window.WindowState = WindowState.Minimized;
        }
    }

    private void OnClose()
    {
        Application.Current.Shutdown();
    }
}
