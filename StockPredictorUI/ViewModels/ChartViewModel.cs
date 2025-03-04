using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Linq;

namespace StockPredictorUI.ViewModels
{
    public class ChartViewModel
    {
        public PlotModel MyPlotModel { get; set; }

        public ChartViewModel()
        {
            MyPlotModel = CreatePlotModel();
        }

        private static PlotModel CreatePlotModel()
        {
            int pointCount = 252 * 5; // 5 years of trading days cause the market is closed on weekends :(
            double[] xs = Consecutive(pointCount);
            double[] ys = RandomWalk(pointCount);

            double minY = ys.Min() - 50;
            double maxY = ys.Max() + 50;
            double yRange = maxY - minY;
            double xRange = pointCount;

            var lineSeries = new LineSeries
            {
                Title = "Stock Price Prediction (5 Years)",
                Color = OxyColors.Blue,
                StrokeThickness = 2,
                MarkerType = MarkerType.None,
                RenderInLegend = false
            };

            for (int i = 0; i < pointCount; i++)
            {
                lineSeries.Points.Add(new DataPoint(xs[i], ys[i]));
            }

            var model = new PlotModel { Title = $"Stock Price Prediction (Next 5 Years)" };
            model.Series.Add(lineSeries);

            // x axis: trading days (in months)
            var xAxis = new LinearAxis
            {
                Position = AxisPosition.Bottom,
                Title = "Time (Months)",
                TitleFontSize = 16,
                FontSize = 12,
                Minimum = 0,
                Maximum = pointCount,
                MajorStep = 21,
                MinorStep = 1,
                MajorGridlineStyle = LineStyle.None,
                MinorGridlineStyle = LineStyle.None,
                AxislineStyle = LineStyle.None,
                LabelFormatter = day => $"{(day / 21) + 1:0}",

                IsZoomEnabled = true,
                IsPanEnabled = true,

                AbsoluteMinimum = 0,
                AbsoluteMaximum = pointCount,

                // symmetrical zoom in limit (minimum 4 monhs of data)
                MinimumRange = 4 * 21
            };

            // y axis: stock price in $
            var yAxis = new LinearAxis
            {
                Position = AxisPosition.Left,
                Title = "Stock Price ($)",
                TitleFontSize = 16,
                FontSize = 12,
                Minimum = minY,
                Maximum = maxY,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.None,
                AxislineStyle = LineStyle.None,
                IsZoomEnabled = true,
                IsPanEnabled = true,

                AbsoluteMinimum = minY,
                AbsoluteMaximum = maxY,

                // Ensure y axis zoom limits match the x axis zooming proportionally
                MinimumRange = (4.0 / xRange) * yRange // same relative zoom as x axis
            };

            model.Axes.Add(xAxis);
            model.Axes.Add(yAxis);

            return model;
        }

        private static double[] Consecutive(int count)
        {
            double[] values = new double[count];
            for (int i = 0; i < count; i++)
                values[i] = i;
            return values;
        }

        private static double[] RandomWalk(int count)
        {
            Random rand = new();
            double[] values = new double[count];
            values[0] = 500;
            for (int i = 1; i < count; i++)
            {
                double changePercent = (rand.NextDouble() - 0.5) * 0.01;
                double newPrice = values[i - 1] * (1 + changePercent);
                values[i] = newPrice;
            }
            return values;
        }
    }
}
