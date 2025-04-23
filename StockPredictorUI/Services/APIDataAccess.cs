using StockPredictorUI.Models;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Net.Http;
using System.Text;

namespace StockPredictorUI.Services;

/// <summary>
/// Fetches historical data via the AlphaVantage stock API
/// </summary>
public class APIDataAccess : IDataAccess
{
    private const string _csvFileName = "stock_data.csv";
    private static readonly HttpClient client = new();
    private const string _alphaVantageApiKey = "ZGLATOL0IYYJKNIS";
    private const string _dataFolderPath = @"C:\Users\cfowl\source\repos\CF_StockPredictor\StockPredictorUI\Resources\Data";

    public async Task<List<float>> GetStockDataAsync(string ticker, int predictionHorizon)
    {
        try
        {
            if (IsTickerInCsv(ticker, out List<StockModel> stockData))
            {
                Console.WriteLine($"Loaded {stockData.Count} records from CSV for {ticker}");
            }
            else
            {
                stockData = await FetchStockDataFromAlphaVantageAsync(ticker);

                if (stockData.Count == 0)
                    throw new Exception("No data available for this ticker.");

                await SaveStockDataToCsvAsync(stockData);
            }

            Console.WriteLine($"Test for valid data: {stockData.Count}");

            AccordMLModel.TrainModel(stockData);

            List<float> predictions = AccordMLModel.PredictFuturePrices(stockData, predictionHorizon);

            return predictions;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Error occurred while fetching and predicting stock data.", ex);
        }
    }

    private static async Task<List<StockModel>> FetchStockDataFromAlphaVantageAsync(string ticker)
    {
        var stockData = new List<StockModel>();
        var apiUrl = $"https://www.alphavantage.co/query?function=TIME_SERIES_DAILY&symbol={ticker}&apikey={_alphaVantageApiKey}&outputsize=full";

        await FetchDataForPeriodAsync(apiUrl, stockData);

        return stockData;
    }

    private static async Task FetchDataForPeriodAsync(string url, List<StockModel> stockData)
    {
        var response = await client.GetAsync(url);
        string responseContent = await response.Content.ReadAsStringAsync();

        Console.WriteLine($"API Response: {responseContent}");

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Failed to fetch stock data: {response.StatusCode}");
        }

        var jsonData = await response.Content.ReadAsStringAsync();
        var jsonObject = JObject.Parse(jsonData);

        // check if the data exists
        var timeSeries = jsonObject["Time Series (Daily)"] ?? throw new Exception("No time series data found.");

        foreach (var dayData in timeSeries)
        {
            string dateStr = dayData.Path.Split('.').Last();
            if (!DateTime.TryParse(dateStr, out DateTime date))
                continue;

            JToken? dayInfo = dayData.First;
            if (dayInfo == null)
                continue;

            stockData.Add(new StockModel
            {
                Ticker = (string?)jsonObject["Meta Data"]?["2. Symbol"],
                Date = date,
                Close = TryParseFloat((string?)dayInfo["4. close"])
            });
        }

    }

    private bool IsTickerInCsv(string ticker, out List<StockModel> stockData)
    {
        stockData = [];

        string fullFilePath = Path.Combine(_dataFolderPath, _csvFileName);

        if (!File.Exists(fullFilePath))
            return false;

        var lines = File.ReadAllLines(fullFilePath);

        foreach (var line in lines.Skip(1))
        {
            var parts = line.Split(',');

            if (parts.Length >= 3 && parts[0].Trim() == ticker)
            {
                stockData.Add(new StockModel
                {
                    Ticker = parts[0].Trim(),
                    Date = DateTime.Parse(parts[1].Trim()),
                    Close = float.Parse(parts[2].Trim())
                });
            }
        }
        stockData.Reverse(); 
        return stockData.Count > 0;
    }

    public async Task SaveStockDataToCsvAsync(List<StockModel> stockData)
    {
        if (stockData.Count == 0) return;

        if (!Directory.Exists(_dataFolderPath))
        {
            Directory.CreateDirectory(_dataFolderPath);
        }

        string fullFilePath = Path.Combine(_dataFolderPath, _csvFileName);

        var sb = new StringBuilder();
        sb.AppendLine("Ticker,Date,Close");

        foreach (var stock in stockData)
        {
            sb.AppendLine($"{stock.Ticker},{stock.Date:yyyy-MM-dd},{stock.Close}");
        }

        await File.WriteAllTextAsync(fullFilePath, sb.ToString());
        Console.WriteLine($"Stock data saved to {fullFilePath}");
    }

    // helper method to safely parse a float from a jtoken
    private static float TryParseFloat(string? value)
    {
        return value != null && float.TryParse(value, out float result) ? result : 0f;
    }
}
