using OpenCvSharp;
using OpenCvSharp.ML;
using ScottPlot;

namespace Lab3;

internal abstract class BaseRunner<TStatModel> : IRunner where TStatModel : StatModel
{
    protected readonly TStatModel _model;
    
    private readonly ClusterData _clusterData;
    private readonly ErrorEvaluator _errorEvaluator;
    private readonly PlotHelper _plotHelper;

    protected BaseRunner(ErrorEvaluator errorEvaluator, PlotHelper plotHelper, ClusterData clusterData,
        TStatModel model)
    {
        _errorEvaluator = errorEvaluator;
        _plotHelper = plotHelper;
        _clusterData = clusterData;
        _model = model;
    }

    public void Run()
    {
        TrainModel();
        EvaluateModel();
        VisualizeResults();
    }

    protected abstract void ConfigureModel();

    private void TrainModel()
    {
        ConfigureModel();
        _model.Train(_clusterData.trainSamplesMat, SampleTypes.RowSample, _clusterData.trainLabelsMat);
    }

    private void EvaluateModel()
    {
        var trainError =
            _errorEvaluator.EvaluateError(_model, _clusterData.trainSamplesMat, _clusterData.trainLabelsMat);
        var testError = _errorEvaluator.EvaluateError(_model, _clusterData.testSamplesMat, _clusterData.testLabelsMat);

        Console.WriteLine($"Error on the training set = {trainError:F4}");
        Console.WriteLine($"Error on the test set = {testError:F4}");
    }

    private void VisualizeResults()
    {
        var gridSize = 100;
        var (xx, yy, gridSamplesMat) = _plotHelper.CreateGrid(gridSize, _clusterData.dimensions);

        var gridPredictions = new Mat();
        _model.Predict(gridSamplesMat, gridPredictions);

        var plt = new Plot();
        _plotHelper.VisualizeDecisionBoundary(
            plt,
            xx,
            yy,
            gridPredictions,
            gridSize,
            _clusterData.samples,
            _clusterData.labels,
            _clusterData.samplesCount,
            GetModelName());
    }

    protected abstract string GetModelName();
}