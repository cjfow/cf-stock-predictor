using CommunityToolkit.Mvvm.Input;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System.Windows;
using System.Windows.Input;

namespace StockPredictorUI.ViewModels;

/// <summary>
/// Creates and manages the chart for displaying stock prediction prices
/// </summary>
public class ChartViewModel
{
    public PlotModel MyPlotModel { get; private set; }

    public ChartViewModel(string stockTicker, List<float> stockData, int predictionHorizon)
    {
        CloseCommand = new RelayCommand(OnClose);
        MyPlotModel = CreatePlotModel(stockTicker, stockData, predictionHorizon);
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

    private static PlotModel CreatePlotModel(string stockTicker, List<float> predictedPrices, int predictionHorizon)
    {
        // throw if null or empty to make sure the list is usable below
        if (predictedPrices == null)
            throw new ArgumentNullException(nameof(predictedPrices), "Predicted stock data cannot be null.");

        if (predictedPrices.Count == 0)
            throw new InvalidOperationException("Predicted stock data cannot be empty.");

        int maxDays = predictionHorizon * 252;
        maxDays = Math.Min(maxDays, predictedPrices.Count);

        var model = new PlotModel { Title = $"Predicted Stock Prices for {stockTicker}" };

        var predictionLineSeries = new LineSeries
        {
            Title = "Predicted Stock Price",
            Color = OxyColors.BlueViolet,
            StrokeThickness = 2,
            MarkerType = MarkerType.None,
            RenderInLegend = false
        };

        for (int i = 0; i < maxDays; i++)
        {
            var truncatedPrice = Math.Floor(predictedPrices[i] * 100) / 100;
            predictionLineSeries.Points.Add(new DataPoint(i, truncatedPrice));
        }

        model.Series.Add(predictionLineSeries);

        double minY = predictedPrices.Min() - 10;
        double maxY = predictedPrices.Max() + 10;

        var xAxis = new LinearAxis
        {
            Position = AxisPosition.Bottom,
            Title = "Time Horizon",
            TitleFontSize = 16,
            FontSize = 12,
            Minimum = 0,
            Maximum = maxDays - 1,
            MajorStep = 21,
            MinorStep = 1,
            MajorGridlineStyle = LineStyle.None,
            MinorGridlineStyle = LineStyle.None,
            AxislineStyle = LineStyle.None,
            AbsoluteMinimum = 0,
            AbsoluteMaximum = maxDays - 1,
            LabelFormatter = val => $"{(int)(val / 21)}m"
        };

        var yAxis = new LinearAxis
        {
            Position = AxisPosition.Left,
            Title = "Stock Price ($)",
            TitleFontSize = 16,
            FontSize = 12,
            Minimum = minY,
            Maximum = maxY,
            MajorGridlineStyle = LineStyle.Solid,
            MinorGridlineStyle = LineStyle.None,
            AxislineStyle = LineStyle.None,
            IsZoomEnabled = true,
            IsPanEnabled = true,
            AbsoluteMinimum = minY,
            AbsoluteMaximum = maxY,
            StringFormat = "0"
        };

        model.Axes.Add(xAxis);
        model.Axes.Add(yAxis);

        return model;
    }

}
