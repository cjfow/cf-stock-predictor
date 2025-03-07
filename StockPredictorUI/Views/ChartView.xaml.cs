using StockPredictorUI.ViewModels;
using StockPredictorUI.Models;
using System.Windows;
using System.Windows.Input;

namespace StockPredictorUI.Views
{
    /// <summary>
    /// Interaction logic for ChartView.xaml
    /// </summary>
    public partial class ChartView : Window
    {
        private readonly ChartViewModel _chartViewModel;

        // Constructor that accepts stock data
        public ChartView(string stockTicker, List<float> predictionData, int predictionHorizon)
        {
            InitializeComponent();
            _chartViewModel = new ChartViewModel(stockTicker, predictionData, predictionHorizon); // Pass the stock data to the ViewModel
            DataContext = _chartViewModel; // Set the DataContext to the ViewModel
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
    }
}
