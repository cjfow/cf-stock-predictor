using Accord.Statistics.Models.Regression.Linear;
using CommunityToolkit.Diagnostics;
using StockPredictorUI.Models;

namespace StockPredictorUI.Services;

/// <summary>
/// Class for training and running the Accord machine learning model
/// </summary>
public class AccordMLModel
{
    private static OrdinaryLeastSquares? _olsRegressionModel;

    public static void TrainModel(List<StockModel> stockData)
    {

        Guard.IsNotNull(stockData);
        Guard.HasSizeGreaterThan(stockData, 1);

        var inputs = stockData.Select(x => new double[] { x.Close })
                              .ToArray();

        var outputs = stockData.Select(x => (double)x.Close)
                               .ToArray();

        _olsRegressionModel = new OrdinaryLeastSquares();
        _olsRegressionModel.Learn(inputs, outputs);
    }

    public static List<float> PredictFuturePrices(List<StockModel> stockData, int predictionHorizon)
    {
        double lastKnownPrice = stockData.Last().Close;
        List<float> predictedPrices = [];

        double startPrice = stockData.First().Close;
        double endPrice = stockData.Last().Close;
        double trendPercent = (endPrice - startPrice) / startPrice;
        double dailyTrend = (trendPercent / stockData.Count) * 0.15;

        Random rand = new();

        for (int i = 0; i < predictionHorizon * 252; i++)
        {
            lastKnownPrice *= (1 + dailyTrend);

            double randomFactor = rand.NextDouble() * 0.0095 - 0.0045;
            lastKnownPrice *= (1 + randomFactor);

            predictedPrices.Add((float)lastKnownPrice);
        }
        return predictedPrices;
    }
}
