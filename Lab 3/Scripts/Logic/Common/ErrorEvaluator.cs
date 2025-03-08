using OpenCvSharp;
using OpenCvSharp.ML;

namespace Lab3;

internal class ErrorEvaluator
{
    public float EvaluateError(StatModel model, Mat trainSamples, Mat trainLabels)
    {
        var trainPredictions = new Mat();
        model.Predict(trainSamples, trainPredictions);

        float trainError = 0;
        var trainSamplesRows = trainSamples.Rows;

        for (var i = 0; i < trainSamplesRows; i++)
            if (IsTrainError(trainLabels, trainPredictions, i))
                trainError++;

        var normalizedTrainError = trainError / trainSamplesRows;

        return normalizedTrainError;
    }

    private bool IsTrainError(Mat trainLabels, Mat trainPredictions, int i)
    {
        var trainPrediction = (int)trainPredictions.At<float>(i, 0);
        var trainLabel = trainLabels.At<int>(i, 0);

        return trainPrediction != trainLabel;
    }
}