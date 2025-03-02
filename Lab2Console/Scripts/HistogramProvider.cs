using OpenCvSharp;

namespace Lab2;

internal static class HistogramProvider
{
    public static Mat GetHistogram(Mat image)
    {
        // Разделение каналов BGR
        var bgrChannels = new Mat[3];
        Cv2.Split(image, out bgrChannels);

        // Параметры гистограммы
        var histSize = 256;
        var histRange = new Rangef(0, 256);
        Mat bHist = new(), gHist = new(), rHist = new();

        // Вычисление гистограммы для каждого канала
        Cv2.CalcHist(new[] { bgrChannels[0] }, new[] { 0 }, null, bHist, 1, new[] { histSize }, new[] { histRange });
        Cv2.CalcHist(new[] { bgrChannels[1] }, new[] { 0 }, null, gHist, 1, new[] { histSize }, new[] { histRange });
        Cv2.CalcHist(new[] { bgrChannels[2] }, new[] { 0 }, null, rHist, 1, new[] { histSize }, new[] { histRange });

        // Нормализация гистограммы
        int histWidth = 512, histHeight = 400;
        var histImg = new Mat(histHeight, histWidth, MatType.CV_8UC3, Scalar.All(0));
        Cv2.Normalize(bHist, bHist, 0, histImg.Rows, NormTypes.MinMax);
        Cv2.Normalize(gHist, gHist, 0, histImg.Rows, NormTypes.MinMax);
        Cv2.Normalize(rHist, rHist, 0, histImg.Rows, NormTypes.MinMax);

        // Отрисовка гистограммы
        var binWidth = (int)Math.Round((double)histWidth / histSize);
        Scalar[] colors = { Scalar.Blue, Scalar.Green, Scalar.Red };

        for (var i = 1; i < histSize; i++)
        {
            Cv2.Line(histImg,
                new Point(binWidth * (i - 1), histHeight - (int)Math.Round(bHist.At<float>(i - 1))),
                new Point(binWidth * i, histHeight - (int)Math.Round(bHist.At<float>(i))),
                colors[0], 2);
            Cv2.Line(histImg,
                new Point(binWidth * (i - 1), histHeight - (int)Math.Round(gHist.At<float>(i - 1))),
                new Point(binWidth * i, histHeight - (int)Math.Round(gHist.At<float>(i))),
                colors[1], 2);
            Cv2.Line(histImg,
                new Point(binWidth * (i - 1), histHeight - (int)Math.Round(rHist.At<float>(i - 1))),
                new Point(binWidth * i, histHeight - (int)Math.Round(rHist.At<float>(i))),
                colors[2], 2);
        }

        return histImg;
    }
    
    public static Mat GetEqualizedHistogramImage(Mat image)
    {
        Mat equalizedImg = new Mat();
        Cv2.EqualizeHist(image, equalizedImg);

        return equalizedImg;
    }
}