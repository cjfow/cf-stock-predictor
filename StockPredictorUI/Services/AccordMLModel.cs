using Accord.Statistics.Models.Regression.Linear;
using CommunityToolkit.Diagnostics;
using StockPredictorUI.Models;

namespace StockPredictorUI.Services;

public class AccordMLModel(IStockConfiguration configuration) : IStockPredictionModel
{
    private readonly IStockConfiguration _configuration = configuration;
    private OrdinaryLeastSquares? _olsRegressionModel;

    public void TrainModel(List<StockModel> stockData)
    {
        Guard.IsNotNull(stockData);
        Guard.HasSizeGreaterThan(stockData, 1);

        double[][] inputs = [.. stockData.Select(x => new double[] { x.Close })];
        double[] outputs = [.. stockData.Select(x => (double)x.Close)];
        _olsRegressionModel = new OrdinaryLeastSquares();
        _olsRegressionModel.Learn(inputs, outputs);
    }

    public List<double> PredictFuturePrices(List<StockModel> stockData, int predictionHorizon)
    {
        Random random = new();
        List<double> predictedPrices = [];

        double lastKnownPrice = stockData.Last().Close;
        double startPrice = stockData.First().Close;
        double endPrice = stockData.Last().Close;
        double trendPercent = (endPrice - startPrice) / startPrice;
        double dailyTrend = (trendPercent / stockData.Count) * _configuration.DailyTrendMultiplier;
        int totalPredictionDays = predictionHorizon * _configuration.TradingDaysPerYear;

        for (var i = 0; i < totalPredictionDays; i++)
        {
            lastKnownPrice *= (1 + dailyTrend);
            double randomFactor = random.NextDouble() * _configuration.MaxRandomVolatilityFactor + _configuration.MinRandomVolatilityFactor;
            lastKnownPrice *= (1 + randomFactor);
            predictedPrices.Add(lastKnownPrice);
        }
        return predictedPrices;
    }
}