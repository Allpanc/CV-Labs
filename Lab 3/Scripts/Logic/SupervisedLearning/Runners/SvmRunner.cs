using OpenCvSharp.ML;

namespace Lab3;

internal class SvmRunner : BaseRunner<SVM>
{
    public SvmRunner(ErrorEvaluator errorEvaluator, PlotHelper plotHelper, ClusterData clusterData, SVM model)
        : base(errorEvaluator, plotHelper, clusterData, model)
    {
    }

    protected override void ConfigureModel()
    {
        _model.Type = SVM.Types.CSvc;
        _model.KernelType = SVM.KernelTypes.Rbf;
        _model.Gamma = 1.0;
        _model.C = 1.0;
    }

    protected override string GetModelName()
    {
        return "Support Vector Machine Classification";
    }
}