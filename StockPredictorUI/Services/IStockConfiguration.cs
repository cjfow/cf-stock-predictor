namespace StockPredictorUI.Services;

public interface IStockConfiguration
{
    string AlphaVantageApiKey { get; }
    string DataDirectoryPath { get; }
    int TradingDaysPerYear { get; }
    int MinimumDataPointsForTraining { get; }
    double DailyTrendMultiplier { get; }
    double MaxRandomVolatilityFactor { get; }
    double MinRandomVolatilityFactor { get; }
}