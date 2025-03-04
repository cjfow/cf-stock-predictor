using StockPredictorUI.ViewModels;
using System.Windows;
using System.Windows.Input;


namespace StockPredictorUI.Views;

/// <summary>
/// Interaction logic for ChartView.xaml
/// </summary>
public partial class ChartView : Window
{
    public ChartView()
    {
        InitializeComponent();
        DataContext = new ChartViewModel();
    }

    private void OnClose_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        if (e.LeftButton == MouseButtonState.Pressed)
        {
            DragMove();
        }
    }
}