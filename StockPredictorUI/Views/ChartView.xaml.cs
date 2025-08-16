using Microsoft.Extensions.DependencyInjection;
using StockPredictorUI.Services;
using StockPredictorUI.ViewModels;
using System.Windows;
using System.Windows.Input;

namespace StockPredictorUI.Views;

public partial class ChartView : Window
{
    public ChartView(string stockTicker, List<double> predictionData, int predictionHorizon)
    {
        InitializeComponent();

        IStockConfiguration? configuration = ((App)Application.Current).Host?.Services.GetRequiredService<IStockConfiguration>();
        ChartViewModel chartViewModel = new(stockTicker, predictionData, predictionHorizon, configuration!);
        DataContext = chartViewModel;
    }

    private void Window_MouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.LeftButton == MouseButtonState.Pressed)
            DragMove();
    }
}