using Newtonsoft.Json.Linq;
using StockPredictorUI.Models;
using System.IO;
using System.Net.Http;
using System.Text;

namespace StockPredictorUI.Services;

public class APIDataAccess(IStockConfiguration configuration, IStockPredictionModel predictionModel) : IDataAccess
{
    private const string _csvFileName = "stock_data.csv";
    private static readonly HttpClient _httpClient = new();
    private readonly IStockConfiguration _configuration = configuration;
    private readonly IStockPredictionModel _predictionModel = predictionModel;

    public async Task<List<double>> GetStockDataAsync(string ticker, int predictionHorizon)
    {
        try
        {
            List<StockModel> stockData = IsTickerInCsv(ticker, out List<StockModel>? cachedData) 
                ? cachedData 
                : await FetchAndCacheStockDataAsync(ticker);

            if (stockData.Count < _configuration.MinimumDataPointsForTraining)
                throw new InvalidOperationException($"Insufficient data points for ticker {ticker}. Required: {_configuration.MinimumDataPointsForTraining}, Available: {stockData.Count}");

            _predictionModel.TrainModel(stockData);
            return _predictionModel.PredictFuturePrices(stockData, predictionHorizon);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Error occurred while fetching and predicting stock data for {ticker}.", ex);
        }
    }

    private async Task<List<StockModel>> FetchAndCacheStockDataAsync(string ticker)
    {
        List<StockModel> stockData = await FetchStockDataFromAlphaVantageAsync(ticker);
        if (stockData.Count == 0)
            throw new InvalidOperationException($"No data available for ticker {ticker}.");

        await SaveStockDataToCsvAsync(stockData);
        return stockData;
    }

    private async Task<List<StockModel>> FetchStockDataFromAlphaVantageAsync(string ticker)
    {
        string apiUrl = $"https://www.alphavantage.co/query?function=TIME_SERIES_DAILY&symbol={ticker}&apikey={_configuration.AlphaVantageApiKey}&outputsize=full";
        return await FetchDataForPeriodAsync(apiUrl);
    }

    private static async Task<List<StockModel>> FetchDataForPeriodAsync(string url)
    {
        var response = await _httpClient.GetAsync(url);
        if (!response.IsSuccessStatusCode)
            throw new HttpRequestException($"Failed to fetch stock data: {response.StatusCode}");

        string jsonData = await response.Content.ReadAsStringAsync();
        JObject jsonObject = JObject.Parse(jsonData);
        JToken timeSeries = jsonObject["Time Series (Daily)"]
            ?? throw new InvalidOperationException("No time series data found in API response.");

        List<StockModel> stockData = [];

        foreach (var dayData in timeSeries)
        {
            string dateStr = dayData.Path.Split('.').Last();
            if (!DateTime.TryParse(dateStr, out var date))
                continue;

            JToken? dayInfo = dayData.First;
            if (dayInfo is null)
                continue;

            stockData.Add(new StockModel
            {
                Ticker = (string?)jsonObject["Meta Data"]?["2. Symbol"],
                Date = date,
                Close = TryParseDouble((string?)dayInfo["4. close"])
            });
        }
        return stockData;
    }

    private bool IsTickerInCsv(string ticker, out List<StockModel> stockData)
    {
        stockData = [];
        string fullFilePath = Path.Combine(_configuration.DataDirectoryPath, _csvFileName);

        if (!File.Exists(fullFilePath))
            return false;

        string[] lines = File.ReadAllLines(fullFilePath);
        foreach (var line in lines.Skip(1))
        {
            string[] parts = line.Split(',');
            if (parts.Length >= 3 && parts[0].Trim() == ticker)
            {
                stockData.Add(new StockModel
                {
                    Ticker = parts[0].Trim(),
                    Date = DateTime.Parse(parts[1].Trim()),
                    Close = double.Parse(parts[2].Trim())
                });
            }
        }
        stockData.Reverse();
        return stockData.Count > 0;
    }

    private async Task SaveStockDataToCsvAsync(List<StockModel> stockData)
    {
        if (stockData.Count == 0)
            return;

        if (!Directory.Exists(_configuration.DataDirectoryPath))
            Directory.CreateDirectory(_configuration.DataDirectoryPath);

        string fullFilePath = Path.Combine(_configuration.DataDirectoryPath, _csvFileName);
        StringBuilder csvContent = new();
        csvContent.AppendLine("Ticker,Date,Close");

        foreach (var stock in stockData)
            csvContent.AppendLine($"{stock.Ticker},{stock.Date:yyyy-MM-dd},{stock.Close}");

        await File.WriteAllTextAsync(fullFilePath, csvContent.ToString());
    }

    private static double TryParseDouble(string? value) =>
        value is not null && double.TryParse(value, out double result) ? result : 0.0;
}