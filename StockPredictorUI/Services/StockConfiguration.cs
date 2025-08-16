using System.IO;

namespace StockPredictorUI.Services;

public class StockConfiguration : IStockConfiguration
{
    public string AlphaVantageApiKey { get; } = Environment.GetEnvironmentVariable("ALPHAVANTAGE_API_KEY") ?? "ZGLATOL0IYYJKNIS";
    public string DataDirectoryPath { get; } = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Data");
    public int TradingDaysPerYear { get; } = 252;
    public int MinimumDataPointsForTraining { get; } = 250;
    public double DailyTrendMultiplier { get; } = 0.15;
    public double MaxRandomVolatilityFactor { get; } = 0.0095;
    public double MinRandomVolatilityFactor { get; } = -0.0045;
}