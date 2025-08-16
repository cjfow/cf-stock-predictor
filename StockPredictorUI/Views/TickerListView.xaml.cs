using StockPredictorUI.ViewModels;
using System.Windows;
using System.Windows.Input;

namespace StockPredictorUI.Views;

public partial class TickerListView : Window
{
    public TickerListView(TickerListViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }

    private void Window_MouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.LeftButton == MouseButtonState.Pressed)
            DragMove();
    }
}
