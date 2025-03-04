using StockPredictorUI.ViewModels;
using System.Windows;
using System.Windows.Input;

namespace StockPredictorUI.Views;

/// <summary>
/// Interaction logic for TickerListView.xaml
/// </summary>
public partial class TickerListView : Window
{
    public TickerListView()
    {
        InitializeComponent();
        DataContext = new TickerListViewModel();
    }

    private void Window_MouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.LeftButton == MouseButtonState.Pressed)
        {
            DragMove();
        }
    }

    private void OnClose(object sender, RoutedEventArgs e)
    {
        Close();
    }
}
