using Microsoft.ML.Data;

namespace StockPredictorUI.Models;

/// <summary>
/// Represents the prediction output
/// </summary>
public class StockPredictionModel
{
    [ColumnName("PredictedClose")]
    public float FutureClose { get; set; }
}
