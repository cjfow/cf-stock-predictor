using CommunityToolkit.Mvvm.Input;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot;
using System.Windows.Input;
using System.Windows;
using CommunityToolkit.Diagnostics;

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

    // displays prediction graph
    private PlotModel CreatePlotModel(string stockTicker, List<float> predictedPrices, int predictionHorizon)
    {
        Guard.IsNotNull(predictedPrices);

        if (predictedPrices.Count == 0)
        {
            throw new InvalidOperationException("Predicted stock data cannot be empty.");
        }

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

        // start at day 0 (today) and plot future prediction
        for (int i = 0; i < maxDays; i++)
        {
            // truncate the predicted price to 2 decimal places
            var truncatedPrice = Math.Floor(predictedPrices[i] * 100) / 100;
            predictionLineSeries.Points.Add(new DataPoint(i, truncatedPrice));
        }

        model.Series.Add(predictionLineSeries);

        // y axis scaling based on predicted prices
        double minY = predictedPrices.Min() - 10;
        double maxY = predictedPrices.Max() + 10;

        // x axis format
        var xAxis = new LinearAxis
        {
            Position = AxisPosition.Bottom,
            Title = "Time Horizon",
            TitleFontSize = 16,
            FontSize = 12,
            Minimum = 0,
            Maximum = maxDays - 1,

            // 1 trading month
            MajorStep = 21,

            MinorStep = 1,
            MajorGridlineStyle = LineStyle.None,
            MinorGridlineStyle = LineStyle.None,
            AxislineStyle = LineStyle.None,
            AbsoluteMinimum = 0,
            AbsoluteMaximum = maxDays - 1,

            // label the x axis with months
            LabelFormatter = (val) =>
                {
                    int months = (int)(val / 21);
                    return $"{months}m";
                }
        };

        // y axis format
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
