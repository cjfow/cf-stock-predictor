namespace StockPredictorUI.Services;

/// <summary>
/// Data access interface
/// </summary>
public interface IDataAccess
{
    Task<List<float>> GetStockDataAsync(string ticker, int predictionHorizon);
}
