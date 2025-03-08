using OpenCvSharp.ML;

namespace Lab3;

internal class GradientBoostRunner : BaseRunner<Boost>
{
    public GradientBoostRunner(ErrorEvaluator errorEvaluator, PlotHelper plotHelper, ClusterData clusterData,
        Boost model)
        : base(errorEvaluator, plotHelper, clusterData, model)
    {
    }

    protected override void ConfigureModel()
    {
        _model.BoostType = Boost.Types.Gentle;
        _model.WeakCount = 100;
        _model.MaxDepth = 3;
        _model.WeightTrimRate = 0.95;
    }

    protected override string GetModelName()
    {
        return "Gradient Boosting Decision Boundary";
    }
}