using CommunityToolkit.Mvvm.Input;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using StockPredictorUI.Services;
using System.Windows;
using System.Windows.Input;

namespace StockPredictorUI.ViewModels;

public class ChartViewModel
{
    private readonly IStockConfiguration _configuration;

    public PlotModel MyPlotModel { get; private set; }
    public ICommand CloseCommand { get; }

    public ChartViewModel(string stockTicker, List<double> stockData, int predictionHorizon, IStockConfiguration configuration)
    {
        _configuration = configuration;
        CloseCommand = new RelayCommand(OnClose);
        MyPlotModel = CreatePlotModel(stockTicker, stockData, predictionHorizon);
    }

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

    private PlotModel CreatePlotModel(string stockTicker, List<double> predictedPrices, int predictionHorizon)
    {
        if (predictedPrices is null)
            throw new ArgumentNullException(nameof(predictedPrices), "Predicted stock data cannot be null.");

        if (predictedPrices.Count == 0)
            throw new InvalidOperationException("Predicted stock data cannot be empty.");

        int maxDays = Math.Min(predictionHorizon * _configuration.TradingDaysPerYear, predictedPrices.Count);
        PlotModel model = new() { Title = $"Predicted Stock Prices for {stockTicker}" };

        LineSeries predictionLineSeries = new()
        {
            Title = "Predicted Stock Price",
            Color = OxyColors.BlueViolet,
            StrokeThickness = 2,
            MarkerType = MarkerType.None,
            RenderInLegend = false
        };

        for (var i = 0; i < maxDays; i++)
        {
            double truncatedPrice = Math.Floor(predictedPrices[i] * 100) / 100;
            predictionLineSeries.Points.Add(new DataPoint(i, truncatedPrice));
        }

        model.Series.Add(predictionLineSeries);
        const int monthInDays = 21;
        const int chartPadding = 10;
        double minY = predictedPrices.Min() - chartPadding;
        double maxY = predictedPrices.Max() + chartPadding;

        LinearAxis xAxis = new()
        {
            Position = AxisPosition.Bottom,
            Title = "Time Horizon",
            TitleFontSize = 16,
            FontSize = 12,
            Minimum = 0,
            Maximum = maxDays - 1,
            MajorStep = monthInDays,
            MinorStep = 1,
            MajorGridlineStyle = LineStyle.None,
            MinorGridlineStyle = LineStyle.None,
            AxislineStyle = LineStyle.None,
            AbsoluteMinimum = 0,
            AbsoluteMaximum = maxDays - 1,
            LabelFormatter = val => $"{(int)(val / monthInDays)}m"
        };

        LinearAxis yAxis = new()
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