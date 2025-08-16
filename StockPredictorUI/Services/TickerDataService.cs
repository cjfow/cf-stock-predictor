using StockPredictorUI.Models;
using System.IO;

namespace StockPredictorUI.Services;

public class TickerDataService(IStockConfiguration configuration) : ITickerDataService
{
    private readonly IStockConfiguration _configuration = configuration;
    private const string _tickerDataFileName = "tickersByCategory.csv";

    public async Task<List<TickerInfo>> GetAllTickersAsync()
    {
        string filePath = Path.Combine(_configuration.DataDirectoryPath, _tickerDataFileName);
        
        if (!File.Exists(filePath))
            throw new FileNotFoundException($"Ticker data file not found: {filePath}");

        string[] lines = await File.ReadAllLinesAsync(filePath);
        List<TickerInfo> tickers = [];

        foreach (var line in lines.Skip(1))
        {
            if (string.IsNullOrWhiteSpace(line))
                continue;

            string[] parts = line.Split(',');
            if (parts.Length >= 4)
            {
                tickers.Add(new TickerInfo
                {
                    Symbol = parts[0].Trim(),
                    Name = parts[1].Trim(),
                    Category = parts[2].Trim(),
                    Description = parts[3].Trim()
                });
            }
        }

        return tickers;
    }

    public async Task<List<TickerInfo>> GetTickersByCategoryAsync(string category)
    {
        List<TickerInfo> allTickers = await GetAllTickersAsync();
        return [.. allTickers.Where(t => string.Equals(t.Category, category, StringComparison.OrdinalIgnoreCase))];
    }

    public async Task<List<string>> GetValidTickerSymbolsAsync()
    {
        List<TickerInfo> allTickers = await GetAllTickersAsync();
        return [.. allTickers.Select(t => t.Symbol)];
    }
}