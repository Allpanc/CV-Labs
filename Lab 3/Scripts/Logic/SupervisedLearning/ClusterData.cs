using OpenCvSharp;

namespace Lab3;

internal class ClusterData
{
    public int dimensions;
    public int[] labels;
    public float[,] samples;
    public int samplesCount;
    public Mat testLabelsMat;
    public Mat testSamplesMat;
    public Mat trainLabelsMat;
    public Mat trainSamplesMat;
}