namespace StockPredictorUI.Services;

/// <summary>
/// Data access interface
/// </summary>
public interface IDataAccess
{
    Task<List<double>> GetStockDataAsync(string ticker, int predictionHorizon);
}
