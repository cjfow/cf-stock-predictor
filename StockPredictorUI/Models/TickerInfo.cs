namespace StockPredictorUI.Models;

/// <summary>
/// Stock ticker information
/// </summary>
public class TickerInfo
{
    public string Symbol { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}