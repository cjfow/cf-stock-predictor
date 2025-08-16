using StockPredictorUI.ViewModels;
using System.Windows;
using System.Windows.Input;

namespace StockPredictorUI.Views;

public partial class HomeView : Window
{
    public HomeView(HomeViewModel viewModel)
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