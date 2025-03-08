using OpenCvSharp.ML;

namespace Lab3;

internal class DecisionTreeRunner : BaseRunner<DTrees>
{
    public DecisionTreeRunner(ErrorEvaluator errorEvaluator, PlotHelper plotHelper, ClusterData clusterData,
        DTrees model)
        : base(errorEvaluator, plotHelper, clusterData, model)
    {
    }

    protected override void ConfigureModel()
    {
        _model.MaxDepth = 10;
        _model.MinSampleCount = 1;
        _model.UseSurrogates = false;
        _model.CVFolds = 0;
        _model.MaxCategories = 2;
    }

    protected override string GetModelName()
    {
        return "Decision Tree";
    }
}