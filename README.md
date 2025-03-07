# CF Stock Predictor

A stock prediction tool designed to predict stock prices using historical data via the AlphaVantage stock API and a machine learning model powered by Accord.NET with data visualization from OxyPlot.

## Work in Progress
This project is currently a work in progress. Features are being added and the tool is under active development.

### Steps to Install

1. Clone the repository:
   ```bash
   git clone https://github.com/cjfow/cf-stock-predictor.git
   ```
   
2. Open the project in Visual Studio:

    Launch Visual Studio.
    Open the cloned repository by selecting Open a project or solution and navigating to the folder where you cloned the repository.

3. Restore dependencies:

    In Visual Studio, go to Tools > NuGet Package Manager > Package Manager Console.
    Run the following command to restore dependencies:

    ```bash
    dotnet restore
    ```

4. Build and run the application:

    Press F5 or click Start in Visual Studio to build and run the application.

### Usage

Once the app is running, you will be able to input a stock ticker and prediction horizon slider (1 - 5 years) to generate stock price predictions.
The predictions will be visualized using a chart.
The application supports a list common index funds and ETFs that can be found by clicking 'View Ticker List' (more to come).
