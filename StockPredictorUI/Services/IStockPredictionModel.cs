using StockPredictorUI.Models;

namespace StockPredictorUI.Services;

public interface IStockPredictionModel
{
    void TrainModel(List<StockModel> stockData);
    List<double> PredictFuturePrices(List<StockModel> stockData, int predictionHorizon);
}