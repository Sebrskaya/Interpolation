using System;
using System.Collections.Generic;
using System.Windows.Forms;


namespace Interpolation
{
    public partial class Form1 : Form
    {
        private double minX = -10;
        private double maxX = 10;
        private Func<double, double> originalFunction = x => Math.Abs(x);
        private int dataPointCount = 4;
        private int interpolationPointCount = 100;

        public Form1()
        {
            InitializeComponent();
            chart1.Click += chart1_Click;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Generate data points for the original function
            List<double> xValues = GenerateXValues(minX, maxX, dataPointCount);
            List<double> yValues = GenerateYValues(originalFunction, xValues);

            // Interpolate and plot the function
            InterpolateAndPlot(originalFunction, xValues, yValues);
        }

        private List<double> GenerateXValues(double minX, double maxX, int count)
        {
            double step = (maxX - minX) / (count - 1);
            List<double> xValues = new List<double>();

            for (int i = 0; i < count; i++)
            {
                double x = minX + i * step;
                xValues.Add(x);
            }

            return xValues;
        }

        private List<double> GenerateYValues(Func<double, double> function, List<double> xValues)
        {
            List<double> yValues = new List<double>();

            foreach (double x in xValues)
            {
                double y = function(x);
                yValues.Add(y);
            }

            return yValues;
        }

        private void InterpolateAndPlot(Func<double, double> function, List<double> xValues, List<double> yValues)
        {
            // Generate x values for plotting
            List<double> xPlot = GenerateXValues(minX, maxX, interpolationPointCount);

            // Generate y values for the original function
            List<double> yPlotOriginal = GenerateYValues(function, xPlot);

            // Interpolate y values for plotting using Lagrange interpolation
            List<double> yPlotInterpolation = InterpolateLagrange(xValues, yValues, xPlot);

            // Plot the graphs
            PlotGraph(xPlot.ToArray(), yPlotOriginal.ToArray(), yPlotInterpolation.ToArray(), xValues, yValues);
        }

        private List<double> InterpolateLagrange(List<double> xValues, List<double> yValues, List<double> xInterpolate)
        {
            List<double> yInterpolate = new List<double>();

            for (int i = 0; i < xInterpolate.Count; i++)
            {
                double x = xInterpolate[i];
                double y = 0;

                for (int j = 0; j < xValues.Count; j++)
                {
                    double term = yValues[j];

                    for (int k = 0; k < xValues.Count; k++)
                    {
                        if (k != j)
                        {
                            term *= (x - xValues[k]) / (xValues[j] - xValues[k]);
                        }
                    }

                    y += term;
                }

                yInterpolate.Add(y);
            }

            return yInterpolate;
        }

        private void PlotGraph(double[] x, double[] yOriginal, double[] yInterpolation, List<double> xValues, List<double> yValues)
        {
            chart1.Series["OriginalFunction"].Points.Clear();
            chart1.Series["Interpolation"].Points.Clear();
            chart1.Series["InterpolationPoints"].Points.Clear();

            for (int i = 0; i < x.Length; i++)
            {
                chart1.Series["OriginalFunction"].Points.AddXY(x[i], yOriginal[i]);
                chart1.Series["Interpolation"].Points.AddXY(x[i], yInterpolation[i]);
            }

            for (int i = 0; i < xValues.Count; i++)
            {
                chart1.Series["InterpolationPoints"].Points.AddXY(xValues[i], yValues[i]);
            }
        }

        private void chart1_Click(object sender, EventArgs e)
        {
            // Generate data points for the original function
            List<double> xValues = GenerateXValues(minX, maxX, dataPointCount);
            List<double> yValues = GenerateYValues(originalFunction, xValues);

            // Interpolate and plot the function
            InterpolateAndPlot(originalFunction, xValues, yValues);
        }
    }
}
