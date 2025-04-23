/* using the AAA pattern (arrange, act, assert)
 * basically:
 * Arrange - setup objects/data
 * Act - call the method or perform an action
 * Assert - check that the result matches the expected outcome
 * side note: sometimes act and assert are done on the same line in anon methods
 */ 

using StockPredictorUI.Models;
using StockPredictorUI.Services;

namespace xUnitTestLibrary;

public class AccordMLModelTests
{
    [Fact]
    public void TrainModel_ShouldNotThrowException_WithValidData()
    {
        var stockData = new List<StockModel>
    {
        new() { Close = 100 },
        new() { Close = 105 },
        new() { Close = 110 }
    };

        var exception = Record.Exception(() => AccordMLModel.TrainModel(stockData));
        Assert.Null(exception);
    }

    [Fact]
    public void TrainModel_ShouldThrowException_WithEmptyData()
    {
        var stockData = new List<StockModel>();

        // an example of act and assert combined
        Assert.Throws<InvalidOperationException>(() => AccordMLModel.TrainModel(stockData));
    }

    [Fact]
    public void PredictFuturePrices_ShouldReturnCorrectCount()
    {
        var stockData = new List<StockModel>
    {
        new() { Close = 100 },
        new() { Close = 105 },
        new() { Close = 110 }
    };
        int predictionHorizon = 2;

        AccordMLModel.TrainModel(stockData);

        var predictions = AccordMLModel.PredictFuturePrices(stockData, predictionHorizon);

        Assert.NotNull(predictions);
        Assert.Equal(predictionHorizon * 252, predictions.Count);
    }

    [Fact]
    public void PredictFuturePrices_ShouldThrowException_WhenStockDataIsEmpty()
    {
        var stockData = new List<StockModel>();
        int predictionHorizon = 2;

        Assert.Throws<InvalidOperationException>(() => AccordMLModel.PredictFuturePrices(stockData, predictionHorizon));
    }
}