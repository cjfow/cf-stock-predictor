using StockPredictorUI.Models;

namespace StockPredictorUI.Services;

public interface ITickerDataService
{
    Task<List<TickerInfo>> GetAllTickersAsync();
    Task<List<TickerInfo>> GetTickersByCategoryAsync(string category);
    Task<List<string>> GetValidTickerSymbolsAsync();
}