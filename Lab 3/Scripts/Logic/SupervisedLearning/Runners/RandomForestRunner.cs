using OpenCvSharp;
using OpenCvSharp.ML;

namespace Lab3;

internal class RandomForestRunner : BaseRunner<RTrees>
{
    public RandomForestRunner(ErrorEvaluator errorEvaluator, PlotHelper plotHelper, ClusterData clusterData,
        RTrees model)
        : base(errorEvaluator, plotHelper, clusterData, model)
    {
    }

    protected override void ConfigureModel()
    {
        _model.MaxDepth = 5;
        _model.ActiveVarCount = 2;
        _model.TermCriteria = new TermCriteria(CriteriaTypes.MaxIter, 100, 0);
        _model.MinSampleCount = 2;
        _model.CalculateVarImportance = true;
    }

    protected override string GetModelName()
    {
        return "Random Forest Decision Boundary";
    }
}