using StockPredictorUI.Models;
using StockPredictorUI.Services;

namespace xUnitTestLibrary;

public class AccordMLModelTests
{
    private readonly StockConfiguration _configuration = new();
    private readonly AccordMLModel _model;

    public AccordMLModelTests()
    {
        _model = new AccordMLModel(_configuration);
    }

    [Fact]
    public void TrainModel_ShouldNotThrowException_WithValidData()
    {
        List<StockModel> stockData =
        [
            new() { Close = 100 },
            new() { Close = 105 },
            new() { Close = 110 }
        ];

        var exception = Record.Exception(() => _model.TrainModel(stockData));
        Assert.Null(exception);
    }

    [Fact]
    public void TrainModel_ShouldThrowArgumentException_WithEmptyData()
    {
        List<StockModel> stockData = [];

        Assert.Throws<ArgumentException>(() => _model.TrainModel(stockData));
    }

    [Fact]
    public void PredictFuturePrices_ShouldReturnCorrectCount()
    {
        List<StockModel> stockData =
        [
            new() { Close = 100 },
            new() { Close = 105 },
            new() { Close = 110 }
        ];
        int predictionHorizon = 2;

        _model.TrainModel(stockData);
        List<double> predictions = _model.PredictFuturePrices(stockData, predictionHorizon);

        Assert.NotNull(predictions);
        Assert.Equal(predictionHorizon * _configuration.TradingDaysPerYear, predictions.Count);
    }

    [Fact]
    public void PredictFuturePrices_ShouldThrowException_WhenStockDataIsEmpty()
    {
        List<StockModel> stockData = [];
        int predictionHorizon = 2;

        Assert.Throws<InvalidOperationException>(() => _model.PredictFuturePrices(stockData, predictionHorizon));
    }
}