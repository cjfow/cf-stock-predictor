using OxyPlot.Series;
using StockPredictorUI.ViewModels;

namespace xUnitTestLibrary;

public class ChartViewModelTests
{
    [Fact]
    public void Constructor_ShouldCreatePlotModel()
    {
        // arrange (setup the testing objects and prepare the prerequisites)
        string stockTicker = "SPY";
        List<float> stockData = [150.5f, 152.3f, 149.8f];
        int predictionHorizon = 1;

        // act (perform the actual work of the test)
        var viewModel = new ChartViewModel(stockTicker, stockData, predictionHorizon);

        // assert (verify the result)
        Assert.NotNull(viewModel.MyPlotModel);
        Assert.Equal("Predicted Stock Prices for SPY", viewModel.MyPlotModel.Title);
    }

    [Fact]
    public void Constructor_ShouldThrowException_WhenStockDataIsEmpty()
    {
        // arrange
        string stockTicker = "SPY";
        List<float> stockData = [];
        int predictionHorizon = 1;

        // act and assert together
        Assert.Throws<InvalidOperationException>(() => new ChartViewModel(stockTicker, stockData, predictionHorizon));
    }

    [Fact]
    public void Constructor_ShouldThrowException_WhenStockDataIsNull()
    {
        // arrange 
        string stockTicker = "SPY";
        List<float>? stockData = null;
        int predictionHorizon = 1;

        // act and assert together
        Assert.Throws<InvalidOperationException>(() => new ChartViewModel(stockTicker, stockData, predictionHorizon));
    }

    [Fact]
    public void CreatePlotModel_ShouldContainCorrectDataPoints()
    {
        // arrange test objects
        string stockTicker = "SPY";
        List<float> stockData = [150.5f, 152.3f, 149.8f];
        int predictionHorizon = 1;

        // act
        var viewModel = new ChartViewModel(stockTicker, stockData, predictionHorizon);
        var lineSeries = viewModel.MyPlotModel.Series[0] as LineSeries;

        // assert
        Assert.NotNull(lineSeries);
        Assert.Equal(3, lineSeries.Points.Count);
        Assert.Equal(0, lineSeries.Points[0].X);
        Assert.Equal(Math.Floor(150.5 * 100) / 100, lineSeries.Points[0].Y);
    }
}