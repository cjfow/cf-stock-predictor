using OxyPlot.Series;
using StockPredictorUI.Services;
using StockPredictorUI.ViewModels;

namespace xUnitTestLibrary;

public class ChartViewModelTests
{
    private readonly IStockConfiguration _mockConfiguration = new StockConfiguration();

    [Fact]
    public void Constructor_ShouldCreatePlotModel()
    {
        string stockTicker = "SPY";
        List<double> stockData = [150.5, 152.3, 149.8];
        int predictionHorizon = 1;

        ChartViewModel viewModel = new(stockTicker, stockData, predictionHorizon, _mockConfiguration);

        Assert.NotNull(viewModel.MyPlotModel);
        Assert.Equal("Predicted Stock Prices for SPY", viewModel.MyPlotModel.Title);
    }

    [Fact]
    public void Constructor_ShouldThrowException_WhenStockDataIsEmpty()
    {
        string stockTicker = "SPY";
        List<double> stockData = [];
        int predictionHorizon = 1;

        Assert.Throws<InvalidOperationException>(() => new ChartViewModel(stockTicker, stockData, predictionHorizon, _mockConfiguration));
    }

    [Fact]
    public void Constructor_ShouldThrowException_WhenStockDataIsNull()
    {
        string stockTicker = "SPY";
        List<double>? stockData = null;
        int predictionHorizon = 1;

        Assert.Throws<ArgumentNullException>(() => new ChartViewModel(stockTicker, stockData!, predictionHorizon, _mockConfiguration));
    }

    [Fact]
    public void CreatePlotModel_ShouldContainCorrectDataPoints()
    {
        string stockTicker = "SPY";
        List<double> stockData = [150.5, 152.3, 149.8];
        int predictionHorizon = 1;

        ChartViewModel viewModel = new(stockTicker, stockData, predictionHorizon, _mockConfiguration);
        LineSeries? lineSeries = viewModel.MyPlotModel.Series[0] as LineSeries;

        Assert.NotNull(lineSeries);
        Assert.Equal(3, lineSeries.Points.Count);
        Assert.Equal(0, lineSeries.Points[0].X);
        Assert.Equal(Math.Floor(150.5 * 100) / 100, lineSeries.Points[0].Y);
    }
}