using StockPredictorUI.ViewModels;
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

        public ChartView(string stockTicker, List<float> predictionData, int predictionHorizon)
        {
            InitializeComponent();
            _chartViewModel = new ChartViewModel(stockTicker, predictionData, predictionHorizon);
            DataContext = _chartViewModel;
        }

        // TODO: make a command for this
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
    }
}
