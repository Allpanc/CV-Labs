using OpenCvSharp.ML;

namespace Lab3;

internal abstract class EntryPoint
{
    private static void Main()
    {
        //Comment or uncomment to run selected method
        //RunSupervisedLearning();
        RunUnsupervisedLearning();
    }

    private static void RunSupervisedLearning()
    {
        var plotHelper = new PlotHelper();
        var errorEvaluator = new ErrorEvaluator();
        var clusterDataGenerator = new ClusterDataGenerator();

        ClusterData clusterData = clusterDataGenerator.GenerateClusterData();
        
        var runners = new List<IRunner>
        {
            new SvmRunner(errorEvaluator, plotHelper, clusterData, SVM.Create()),
            new RandomForestRunner(errorEvaluator, plotHelper, clusterData, RTrees.Create()),
            new GradientBoostRunner(errorEvaluator, plotHelper, clusterData, Boost.Create()),
            new DecisionTreeRunner(errorEvaluator, plotHelper, clusterData, DTrees.Create())
        };
        
        foreach (var runner in runners)
        {
            runner.Run();
        }
    }
    
    private static void RunUnsupervisedLearning()
    {
        var fileName = PathProvider.DatasetPath;

        PrepareDataset(fileName);
        RunKMeansClusterization(fileName);
    }

    private static void PrepareDataset(string fileName)
    {
        var numberOfPoints = 150;
        var datasetGenerator = new DatasetGenerator();
        datasetGenerator.GenerateRandomDataset(fileName, numberOfPoints);
    }

    private static void RunKMeansClusterization(string fileName)
    {
        var datasetLoader = new DatasetLoader();
        var plotHelper = new PlotHelper();
        
        float[,] data = datasetLoader.LoadData(fileName);
        
        IRunner kMeansRunner = new KMeansRunner(data, plotHelper);
        kMeansRunner.Run();
    }
}