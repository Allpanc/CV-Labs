using OpenCvSharp;
using ScottPlot;

namespace Lab3;

internal class PlotHelper
{
    private const int PlotWidth = 800;
    private const int PlotHeight = 600;

    public (float[,], float[,], Mat) CreateGrid(int gridSize, int dimensions)
    {
        var xx = new float[gridSize, gridSize];
        var yy = new float[gridSize, gridSize];

        FillGridCoordinates(xx, yy, gridSize);
        var gridSamplesMat = CreateGridSamplesMat(xx, yy, gridSize, dimensions);

        return (xx, yy, gridSamplesMat);
    }

    public void VisualizeDecisionBoundary(
        Plot plt,
        float[,] xx,
        float[,] yy,
        Mat gridPredictions,
        int gridSize,
        float[,] samples,
        int[] labels,
        int sampleCount,
        string title)
    {
        PlotPredictionGrid(plt, xx, yy, gridPredictions, gridSize);
        PlotSamples(plt, samples, labels, sampleCount);
        plt.Title(title);

        SavePlotToPng(plt, title);
    }

    public void VisualizeClusters(float[,] data, int[] labels, Mat centers, string title)
    {
        var plt = new Plot();

        var numClusters = centers.Rows;
        
        for (var i = 0; i < numClusters; i++)
        {
            var (xValues, yValues) = ExtractClusterPoints(data, labels, i);
            AddClusterScatterPlot(plt, xValues, yValues, i);
        }
        
        var (centerX, centerY) = ExtractCentroids(centers);
        AddCentroidsScatterPlot(plt, centerX, centerY);
        
        ConfigurePlot(plt, title);
        SavePlotToPng(plt, title);
    }

    private (double[] xValues, double[] yValues) ExtractClusterPoints(float[,] data, int[] labels, int clusterIndex)
    {
        var clusterPoints = data.Cast<float>()
            .Select((val, idx) => new { Value = val, Index = idx })
            .Where(x => labels[x.Index / 2] == clusterIndex && x.Index % 2 == 0)
            .Select(x => new { X = x.Value, Y = data[x.Index / 2, 1] })
            .ToArray();

        var xValues = clusterPoints.Select(p => (double)p.X).ToArray();
        var yValues = clusterPoints.Select(p => (double)p.Y).ToArray();

        return (xValues, yValues);
    }

    private void AddClusterScatterPlot(Plot plt, double[] xValues, double[] yValues, int clusterIndex)
    {
        var scatter = plt.Add.Scatter(xValues, yValues);
        scatter.LegendText = $"Cluster {clusterIndex}";
        scatter.LineWidth = 0;

        scatter.Color = clusterIndex switch
        {
            0 => Colors.RebeccaPurple,
            1 => Colors.Olive,
            2 => Colors.FireBrick,
            _ => Colors.GoldenRod
        };
    }

    private (double[] centerX, double[] centerY) ExtractCentroids(Mat centers)
    {
        var numClusters = centers.Rows;
        var centerX = new double[numClusters];
        var centerY = new double[numClusters];

        for (var i = 0; i < numClusters; i++)
        {
            centerX[i] = centers.At<float>(i, 0);
            centerY[i] = centers.At<float>(i, 1);
        }

        return (centerX, centerY);
    }

    private void AddCentroidsScatterPlot(Plot plt, double[] centerX, double[] centerY)
    {
        var centroids = plt.Add.Scatter(centerX, centerY);
        centroids.MarkerShape = MarkerShape.Asterisk;
        centroids.MarkerSize = 10;
        centroids.Color = Colors.LightCoral;
        centroids.LegendText = "Centroids";
        centroids.LineWidth = 0;
    }

    private void ConfigurePlot(Plot plt, string title)
    {
        plt.Title(title);
        plt.XLabel("X");
        plt.YLabel("Y");
        plt.Legend.IsVisible = true;
        plt.Grid.MajorLineColor = Colors.Black.WithAlpha(0.2);
        plt.ShowGrid();
    }

    private void FillGridCoordinates(float[,] xx, float[,] yy, int gridSize)
    {
        for (var i = 0; i < gridSize; i++)
        for (var j = 0; j < gridSize; j++)
        {
            xx[i, j] = (float)j / (gridSize - 1);
            yy[i, j] = (float)i / (gridSize - 1);
        }
    }

    private Mat CreateGridSamplesMat(float[,] xx, float[,] yy, int gridSize, int dimensions)
    {
        var gridSamplesMat = new Mat(gridSize * gridSize, dimensions, MatType.CV_32FC1);

        for (var i = 0; i < gridSize; i++)
        for (var j = 0; j < gridSize; j++)
        {
            var idx = i * gridSize + j;
            gridSamplesMat.Set(idx, 0, xx[i, j]);
            gridSamplesMat.Set(idx, 1, yy[i, j]);
        }

        return gridSamplesMat;
    }

    private void PlotPredictionGrid(Plot plt, float[,] xx, float[,] yy, Mat gridPredictions, int gridSize)
    {
        for (var i = 0; i < gridSize; i++)
        for (var j = 0; j < gridSize; j++)
        {
            var idx = i * gridSize + j;
            var prediction = gridPredictions.At<float>(idx, 0);
            var rect = plt.Add.Polygon(
                new[] { xx[i, j], xx[i, j] + 1.0f / gridSize, xx[i, j] + 1.0f / gridSize, xx[i, j] },
                new[] { yy[i, j], yy[i, j], yy[i, j] + 1.0f / gridSize, yy[i, j] + 1.0f / gridSize }
            );
            rect.FillStyle.Color =
                prediction > 0.5 ? Colors.Plum.WithAlpha(0.3) : Colors.LightGoldenRodYellow.WithAlpha(0.3);
            rect.LineWidth = 0;
        }
    }

    private void PlotSamples(Plot plt, float[,] samples, int[] labels, int sampleCount)
    {
        for (var i = 0; i < sampleCount; i++)
        {
            var x = samples[i, 0];
            var y = samples[i, 1];
            var label = labels[i];

            var marker = plt.Add.Marker(x, y, MarkerShape.FilledDiamond, 6);
            marker.Color = label == 1 ? Colors.DarkGoldenRod : Colors.DarkMagenta;
            marker.LineWidth = 0;
        }
    }

    private static void SavePlotToPng(Plot plt, string title)
    {
        plt.SavePng($"{PathProvider.ResourcesPath}\\{title}.png", PlotWidth, PlotHeight);
    }
}