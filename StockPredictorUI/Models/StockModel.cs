using Microsoft.ML.Data;

namespace StockPredictorUI.Models;

/// <summary>
/// Represents important elements of a stock
/// </summary>
public class StockModel
{
    [ColumnName("Ticker")]
    public string? Ticker { get; set; }

    [ColumnName("Date")]
    public DateTime Date { get; set; }

    [ColumnName("Close")]
    public float Close { get; set; }

}
