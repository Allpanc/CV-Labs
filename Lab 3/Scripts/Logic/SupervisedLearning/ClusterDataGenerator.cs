using OpenCvSharp;

namespace Lab3;

internal class ClusterDataGenerator
{
    private const int FeatureDimension = 2;
    private const int TotalSamples = 2000;
    private const int TrainingSamples = 1000;
    private const int Seed = 2323;
    private readonly Random _random = new(Seed);

    public ClusterData GenerateClusterData()
    {
        var samples = GenerateSamples(TotalSamples);
        var labels = AssignLabels(samples);

        var trainSamplesMat = CreateSampleMatrix(samples, 0, TrainingSamples);
        var testSamplesMat = CreateSampleMatrix(samples, TrainingSamples, TotalSamples - TrainingSamples);
        var trainLabelsMat = CreateLabelMatrix(labels, 0, TrainingSamples);
        var testLabelsMat = CreateLabelMatrix(labels, TrainingSamples, TotalSamples - TrainingSamples);

        return new ClusterData
        {
            trainSamplesMat = trainSamplesMat,
            testSamplesMat = testSamplesMat,
            trainLabelsMat = trainLabelsMat,
            testLabelsMat = testLabelsMat,
            dimensions = FeatureDimension,
            samplesCount = TrainingSamples,
            samples = samples,
            labels = labels
        };
    }

    private float[,] GenerateSamples(int count)
    {
        var samples = new float[count, FeatureDimension];
        for (var i = 0; i < count; i++)
        {
            samples[i, 0] = (float)_random.NextDouble();
            samples[i, 1] = (float)_random.NextDouble();
        }

        return samples;
    }

    private int[] AssignLabels(float[,] samples)
    {
        var labels = new int[samples.GetLength(0)];
        
        for (var i = 0; i < samples.GetLength(0); i++)
        {
            labels[i] = DetermineLabel(samples[i, 0], samples[i, 1]);
        }

        return labels;
    }

    private int DetermineLabel(float x, float y)
    {
        return (x < 0.5f && y < 0.5f) || (x > 0.5f && y > 0.5f) ? 1 : 0;
    }

    private Mat CreateSampleMatrix(float[,] samples, int startIndex, int count)
    {
        var mat = new Mat(count, FeatureDimension, MatType.CV_32FC1);
        for (var i = 0; i < count; i++)
        {
            mat.Set(i, 0, samples[i + startIndex, 0]);
            mat.Set(i, 1, samples[i + startIndex, 1]);
        }

        return mat;
    }

    private Mat CreateLabelMatrix(int[] labels, int startIndex, int count)
    {
        var mat = new Mat(count, 1, MatType.CV_32SC1);
        
        for (var i = 0; i < count; i++)
        {
            mat.Set(i, 0, labels[i + startIndex]);
        }

        return mat;
    }
}