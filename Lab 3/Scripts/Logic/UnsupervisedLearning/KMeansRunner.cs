using OpenCvSharp;

namespace Lab3;

internal class KMeansRunner : IRunner
{
    private const int NumberOfClusters = 3;

    private readonly float[,] _data;
    private readonly PlotHelper _plotHelper;

    public KMeansRunner(float[,] data, PlotHelper plotHelper)
    {
        _data = data;
        _plotHelper = plotHelper;
    }

    public void Run()
    {
        Mat dataMat = ConvertToMat(_data);
        var (labels, centers) = PerformKMeansClustering(dataMat, NumberOfClusters);
        
        _plotHelper.VisualizeClusters(_data, labels, centers, "K-Means Clustering");
    }

    private Mat ConvertToMat(float[,] data)
    {
        int rowCount = data.GetLength(0);
        int colCount = data.GetLength(1);

        Mat dataMat = new Mat(rowCount, colCount, MatType.CV_32FC1);
        for (int i = 0; i < rowCount; i++)
        {
            for (int j = 0; j < colCount; j++)
            {
                dataMat.Set(i, j, data[i, j]);
            }
        }

        return dataMat;
    }

    private (int[] labels, Mat centers) PerformKMeansClustering(Mat dataMat, int numClusters)
    {
        TermCriteria criteria = new TermCriteria(CriteriaTypes.Eps | CriteriaTypes.MaxIter, 100, 0.2);

        Mat labelsMat = new Mat();
        Mat centers = new Mat();

        Cv2.Kmeans(dataMat, numClusters, labelsMat, criteria, 10, KMeansFlags.RandomCenters, centers);

        int[] labels = ExtractLabelsFromMat(labelsMat, dataMat.Rows);
        return (labels, centers);
    }

    private int[] ExtractLabelsFromMat(Mat labelsMat, int rowCount)
    {
        int[] labelsArray = new int[rowCount];
        for (int i = 0; i < rowCount; i++)
        {
            labelsArray[i] = labelsMat.At<int>(i, 0);
        }
        return labelsArray;
    }
}