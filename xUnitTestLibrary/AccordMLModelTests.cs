using StockPredictorUI.Models;
using StockPredictorUI.Services;

namespace xUnitTestLibrary;

public class AccordMLModelTests
{
    [Fact]
    public void TrainModel_ShouldNotThrowException_WithValidData()
    {
        // arrange
        var stockData = new List<StockModel>
    {
        new() { Close = 100 },
        new() { Close = 105 },
        new() { Close = 110 }
    };

        // act and assert
        var exception = Record.Exception(() => AccordMLModel.TrainModel(stockData));
        Assert.Null(exception);
    }

    [Fact]
    public void TrainModel_ShouldThrowException_WithEmptyData()
    {
        // arrange
        var stockData = new List<StockModel>();

        // act and assert
        Assert.Throws<InvalidOperationException>(() => AccordMLModel.TrainModel(stockData));
    }

    [Fact]
    public void PredictFuturePrices_ShouldReturnCorrectCount()
    {
        // arrange
        var stockData = new List<StockModel>
    {
        new() { Close = 100 },
        new() { Close = 105 },
        new() { Close = 110 }
    };
        int predictionHorizon = 2;

        AccordMLModel.TrainModel(stockData);

        // act
        var predictions = AccordMLModel.PredictFuturePrices(stockData, predictionHorizon);

        // assert
        Assert.NotNull(predictions);
        Assert.Equal(predictionHorizon * 252, predictions.Count);
    }

    [Fact]
    public void PredictFuturePrices_ShouldThrowException_WhenStockDataIsEmpty()
    {
        // arrange
        var stockData = new List<StockModel>();
        int predictionHorizon = 2;

        // act and assert
        Assert.Throws<InvalidOperationException>(() => AccordMLModel.PredictFuturePrices(stockData, predictionHorizon));
    }
}