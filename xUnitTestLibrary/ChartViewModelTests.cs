using OxyPlot.Series;
using StockPredictorUI.ViewModels;

namespace xUnitTestLibrary;

public class ChartViewModelTests
{
    [Fact]
    public void Constructor_ShouldCreatePlotModel()
    {
        string stockTicker = "SPY";
        List<float> stockData = [150.5f, 152.3f, 149.8f];
        int predictionHorizon = 1;
       
        var viewModel = new ChartViewModel(stockTicker, stockData, predictionHorizon);

        Assert.NotNull(viewModel.MyPlotModel);
        Assert.Equal("Predicted Stock Prices for SPY", viewModel.MyPlotModel.Title);
    }

    [Fact]
    public void Constructor_ShouldThrowException_WhenStockDataIsEmpty()
    {
        string stockTicker = "SPY";
        List<float> stockData = [];
        int predictionHorizon = 1;

        Assert.Throws<InvalidOperationException>(() => new ChartViewModel(stockTicker, stockData, predictionHorizon));
    }

    [Fact]
    public void Constructor_ShouldThrowException_WhenStockDataIsNull()
    {
        string stockTicker = "SPY";
        List<float>? stockData = null;
        int predictionHorizon = 1;

        Assert.Throws<ArgumentNullException>(() => new ChartViewModel(stockTicker, stockData!, predictionHorizon));
    }

    [Fact]
    public void CreatePlotModel_ShouldContainCorrectDataPoints()
    {
        string stockTicker = "SPY";
        List<float> stockData = [150.5f, 152.3f, 149.8f];
        int predictionHorizon = 1;

        var viewModel = new ChartViewModel(stockTicker, stockData, predictionHorizon);
        var lineSeries = viewModel.MyPlotModel.Series[0] as LineSeries;

        Assert.NotNull(lineSeries);
        Assert.Equal(3, lineSeries.Points.Count);
        Assert.Equal(0, lineSeries.Points[0].X);
        Assert.Equal(Math.Floor(150.5 * 100) / 100, lineSeries.Points[0].Y);
    }
}