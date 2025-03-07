using Accord.Statistics.Models.Regression.Linear;
using StockPredictorUI.Models;

namespace StockPredictorUI.Services;

/// <summary>
/// Class for training and running the Accord machine learning model
/// </summary>
public class AccordMLModel
{
    private static OrdinaryLeastSquares? _olsModel;
    private static MultipleLinearRegression? _regressionModel;

    public static void TrainModel(List<StockModel> stockData)
    {
        // train the data using close prices over 20 years
        var inputs = stockData.Select(x => new double[] { (double)x.Close }).ToArray();
        var outputs = stockData.Select(x => (double)x.Close).ToArray();

        _olsModel = new OrdinaryLeastSquares();
        _regressionModel = _olsModel.Learn(inputs, outputs);
    }

    public static List<float> PredictFuturePrices(List<StockModel> stockData, int predictionHorizon)
    {
        double lastKnownPrice = stockData.Last().Close;
        List<float> predictedPrices = [];

        // calculate the overall trend
        double startPrice = stockData.First().Close;
        double endPrice = stockData.Last().Close;
        double trendPercent = (endPrice - startPrice) / startPrice;
        double dailyTrend = (trendPercent / stockData.Count) * 0.15;

        Random rand = new();

        for (int i = 0; i < predictionHorizon * 252; i++)
        {
            // apply the trend
            lastKnownPrice *= (1 + dailyTrend);
            
            double randomFactor = rand.NextDouble() * 0.0095 - 0.0045;
            lastKnownPrice *= (1 + randomFactor);

            predictedPrices.Add((float)lastKnownPrice);
        }
        return predictedPrices;
    }
}
