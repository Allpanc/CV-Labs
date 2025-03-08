using OpenCvSharp.ML;

namespace Lab3;

internal abstract class EntryPoint
{
    private static void Main()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Select a learning type:");
            Console.WriteLine("1 - Supervised Learning");
            Console.WriteLine("2 - Unsupervised Learning");
            Console.WriteLine("Backspace - Exit");

            var key = Console.ReadKey(true).Key;

            if (key == ConsoleKey.Backspace)
            {
                break;
            }

            switch (key)
            {
                case ConsoleKey.D1:
                case ConsoleKey.NumPad1:
                    RunSupervisedLearning();
                    break;

                case ConsoleKey.D2:
                case ConsoleKey.NumPad2:
                    RunUnsupervisedLearning();
                    break;

                default:
                    Console.WriteLine("Invalid input. Try again.");
                    Pause();
                    break;
            }
        }
    }

    private static void RunSupervisedLearning()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Select a supervised learning method:");
            Console.WriteLine("1 - Support Vector Machine");
            Console.WriteLine("2 - Random Forest");
            Console.WriteLine("3 - Gradient Boost");
            Console.WriteLine("4 - Decision Tree");
            Console.WriteLine("Backspace - Back");

            var key = Console.ReadKey(true).Key;

            if (key == ConsoleKey.Backspace)
            {
                return;
            }

            var plotHelper = new PlotHelper();
            var errorEvaluator = new ErrorEvaluator();
            var clusterDataGenerator = new ClusterDataGenerator();

            var clusterData = clusterDataGenerator.GenerateClusterData();

            IRunner? runner;
            switch (key)
            {
                case ConsoleKey.D1 or ConsoleKey.NumPad1:
                    runner = new SvmRunner(errorEvaluator, plotHelper, clusterData, SVM.Create());
                    break;
                case ConsoleKey.D2 or ConsoleKey.NumPad2:
                    runner = new RandomForestRunner(errorEvaluator, plotHelper, clusterData, RTrees.Create());
                    break;
                case ConsoleKey.D3 or ConsoleKey.NumPad3:
                    runner = new GradientBoostRunner(errorEvaluator, plotHelper, clusterData, Boost.Create());
                    break;
                case ConsoleKey.D4 or ConsoleKey.NumPad4:
                    runner = new DecisionTreeRunner(errorEvaluator, plotHelper, clusterData, DTrees.Create());
                    break;
                default:
                    runner = null;
                    break;
            }

            if (runner != null)
            {
                runner.Run();
                Console.WriteLine("\nAlgorithm execution completed. Press any key to return to the menu.");
                Console.ReadKey(true);
            }
            else
            {
                Console.WriteLine("Invalid input. Try again.");
                Pause();
            }
        }
    }

    private static void RunUnsupervisedLearning()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Select an unsupervised learning method:");
            Console.WriteLine("1 - K Means");
            Console.WriteLine("Backspace - Back");

            var key = Console.ReadKey(true).Key;

            switch (key)
            {
                case ConsoleKey.Backspace:
                    return;
                case ConsoleKey.D1:
                case ConsoleKey.NumPad1:
                    RunKMeansClusterization();
                    Console.WriteLine("\nAlgorithm execution completed. Press any key to return to the menu.");
                    Console.ReadKey(true);
                    break;
                default:
                    Console.WriteLine("Invalid input. Try again.");
                    Pause();
                    break;
            }
        }
    }

    private static void RunKMeansClusterization()
    {
        var fileName = PathProvider.DatasetPath;

        PrepareDataset(fileName);

        var datasetLoader = new DatasetLoader();
        var plotHelper = new PlotHelper();

        var data = datasetLoader.LoadData(fileName);

        IRunner kMeansRunner = new KMeansRunner(data, plotHelper);
        kMeansRunner.Run();
    }

    private static void PrepareDataset(string fileName)
    {
        var numberOfPoints = 150;
        var datasetGenerator = new DatasetGenerator();
        datasetGenerator.GenerateRandomDataset(fileName, numberOfPoints);
    }

    private static void Pause()
    {
        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey(true);
    }
}