using System.Globalization;
using CsvHelper;

namespace Lab3;

internal class DatasetGenerator
{
    private const int Seed = 42;
    private readonly Random _random = new(Seed);

    public void GenerateRandomDataset(string filename, int numberOfPoints)
    {
        var clusterCenters = GetClusterCenters();
        var clusterSizes = GetUnequalClusterSizes(numberOfPoints);

        using var writer = new StreamWriter(filename);
        using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

        GenerateClusteredPoints(csv, clusterCenters, clusterSizes);
        GenerateNoisePoints(csv, numberOfPoints / 20);

        Console.WriteLine(
            $"Generated file {filename} with {numberOfPoints} random points in 3 intersecting clusters");
    }

    private void GenerateClusteredPoints(CsvWriter csv, double[,] clusterCenters, int[] clusterSizes)
    {
        for (var clusterIndex = 0; clusterIndex < 3; clusterIndex++)
        {
            var clusterSpread = GetClusterSpread(clusterIndex);
            var spreadX = clusterSpread.Item1;
            var spreadY = clusterSpread.Item2;
            var useGaussian = clusterSpread.Item3;

            for (var i = 0; i < clusterSizes[clusterIndex]; i++)
            {
                double x, y;

                if (useGaussian)
                {
                    var gaussianPoint = GenerateGaussianPoint(clusterCenters, clusterIndex, spreadX, spreadY);
                    x = gaussianPoint.Item1;
                    y = gaussianPoint.Item2;
                }
                else
                {
                    var uniformPoint = GenerateUniformPoint(clusterCenters, clusterIndex, spreadX, spreadY);
                    x = uniformPoint.Item1;
                    y = uniformPoint.Item2;
                }

                if (IsOutlier())
                {
                    var outlierPoint = AddOutlierNoise(x, y);
                    x = outlierPoint.Item1;
                    y = outlierPoint.Item2;
                }

                WriteCsvRow(csv, x, y);
            }
        }
    }

    private void GenerateNoisePoints(CsvWriter csv, int noisePointsCount)
    {
        for (var i = 0; i < noisePointsCount; i++)
        {
            var x = _random.NextDouble() * 8;
            var y = _random.NextDouble() * 8;
            WriteCsvRow(csv, x, y);
        }
    }

    private (double, double, bool) GetClusterSpread(int clusterIndex)
    {
        switch (clusterIndex)
        {
            case 0:
                return (0.8, 0.4, true);
            case 1:
                return (0.5, 0.5, true);
            case 2:
                return (0.7, 0.7, false);
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private (double, double) GenerateGaussianPoint(double[,] clusterCenters, int clusterIndex, double spreadX,
        double spreadY)
    {
        var transform = BoxMullerTransform();
        var z1 = transform.Item1;
        var z2 = transform.Item2;

        var x = clusterCenters[clusterIndex, 0] + z1 * spreadX;
        var y = clusterCenters[clusterIndex, 1] + (clusterIndex == 0 ? z1 * 0.3 + z2 * spreadY : z2 * spreadY);

        return (x, y);
    }

    private (double, double) GenerateUniformPoint(double[,] clusterCenters, int clusterIndex, double spreadX,
        double spreadY)
    {
        var x = clusterCenters[clusterIndex, 0] + (_random.NextDouble() * 2 - 1) * spreadX;
        var y = clusterCenters[clusterIndex, 1] + (_random.NextDouble() * 2 - 1) * spreadY;

        return (x, y);
    }

    private (double, double) BoxMullerTransform()
    {
        var u1 = _random.NextDouble();
        var u2 = _random.NextDouble();
        var z1 = Math.Sqrt(-2 * Math.Log(u1)) * Math.Cos(2 * Math.PI * u2);
        var z2 = Math.Sqrt(-2 * Math.Log(u1)) * Math.Sin(2 * Math.PI * u2);

        return (z1, z2);
    }

    private bool IsOutlier()
    {
        return _random.NextDouble() < 0.02;
    }

    private (double, double) AddOutlierNoise(double x, double y)
    {
        x += (_random.NextDouble() * 2 - 1) * 1.5;
        y += (_random.NextDouble() * 2 - 1) * 1.5;

        return (x, y);
    }

    private void WriteCsvRow(CsvWriter csv, double x, double y)
    {
        csv.WriteField(x.ToString(CultureInfo.InvariantCulture));
        csv.WriteField(y.ToString(CultureInfo.InvariantCulture));
        csv.NextRecord();
    }

    private double[,] GetClusterCenters()
    {
        return new[,]
        {
            { 3.5, 3.0 },
            { 5.5, 4.0 },
            { 4.5, 5.5 }
        };
    }

    private int[] GetUnequalClusterSizes(int numberOfPoints)
    {
        var cluster1Size = numberOfPoints / 2;
        var cluster2Size = numberOfPoints / 3;
        var cluster3Size = numberOfPoints - cluster1Size - cluster2Size;

        return new[] { cluster1Size, cluster2Size, cluster3Size };
    }
}