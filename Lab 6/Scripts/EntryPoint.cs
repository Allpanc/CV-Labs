using System.Diagnostics;
using ImageMagick;
using ImageMagick.Drawing;
using OpenCvSharp;

namespace Lab6;

internal static class EntryPoint
{
    private static void Main()
    {
        CompareLibraries(PathProvider.ImagePath);
        Cv2.WaitKey(0);
    }

    private static void CompareLibraries(string imagePath)
    {
        var cvImage = Cv2.ImRead(imagePath);
        using var mgImage = new MagickImage(imagePath);

        MeasureTime("Median filter (OpenCvSharp)", () => { Cv2.MedianBlur(cvImage, cvImage, 5); });
        ShowImage("Median filter (OpenCvSharp)", cvImage);

        MeasureTime("Median filter (Magick.NET)", () => { mgImage.MedianFilter(5); });
        ShowImage("Median filter (Magick.NET)", mgImage);

        MeasureTime("Erosion (OpenCvSharp)", () =>
        {
            var kernel = Cv2.GetStructuringElement(MorphShapes.Rect, new Size(3, 3));
            Cv2.Erode(cvImage, cvImage, kernel);
        });
        ShowImage("Erosion (OpenCvSharp)", cvImage);

        MeasureTime("Erosion (Magick.NET)", () =>
        {
            mgImage.Morphology(new MorphologySettings
            {
                Method = MorphologyMethod.Erode,
                Kernel = Kernel.Rectangle
            });
        });
        ShowImage("Erosion (Magick.NET)", mgImage);

        MeasureTime("Dilation (OpenCvSharp)", () =>
        {
            var kernel = Cv2.GetStructuringElement(MorphShapes.Rect, new Size(3, 3));
            Cv2.Dilate(cvImage, cvImage, kernel);
        });
        ShowImage("Dilation (OpenCvSharp)", cvImage);

        MeasureTime("Dilation (Magick.NET)", () =>
        {
            mgImage.Morphology(new MorphologySettings
            {
                Method = MorphologyMethod.Dilate,
                Kernel = Kernel.Rectangle
            });
        });
        ShowImage("Dilation (Magick.NET)", mgImage);

        MeasureTime("Histogram (OpenCvSharp)", () =>
        {
            var gray = new Mat();
            Cv2.CvtColor(cvImage, gray, ColorConversionCodes.BGR2GRAY);
            var hist = new Mat();
            Cv2.CalcHist(new[] { gray }, new[] { 0 }, null, hist, 1, new[] { 256 }, new Rangef[] { new(0, 256) });
            ShowHistogram("Histogram (OpenCvSharp)", hist);
        });

        MeasureTime("Histogram (Magick.NET)", () =>
        {
            mgImage.ColorSpace = ColorSpace.Gray;
            IReadOnlyDictionary<IMagickColor<byte>, uint> histogram = mgImage.Histogram();
            ShowHistogram("Histogram (Magick.NET)", histogram);
        });
    }

    private static void MeasureTime(string operationName, Action action)
    {
        var stopwatch = Stopwatch.StartNew();
        action();
        stopwatch.Stop();
        Console.WriteLine($"{operationName}: {stopwatch.ElapsedMilliseconds} ms");
    }

    private static void ShowImage(string title, Mat image)
    {
        Cv2.ImShow(title, image);
    }

    private static void ShowImage(string title, MagickImage image)
    {
        string tempFile = Path.Combine(Path.GetTempPath(), $"{title}.png");
        image.Write(tempFile);
        Process.Start(new ProcessStartInfo(tempFile) { UseShellExecute = true });
    }

    private static void ShowHistogram(string title, Mat histogram)
    {
        var histImage = new Mat(300, 256, MatType.CV_8UC3, new Scalar(255, 255, 255));
        Cv2.Normalize(histogram, histogram, 0, histImage.Rows, NormTypes.MinMax);
        for (int i = 1; i < histogram.Rows; i++)
        {
            Cv2.Line(histImage, new Point(i - 1, 300 - (int)histogram.At<float>(i - 1)),
                     new Point(i, 300 - (int)histogram.At<float>(i)),
                     new Scalar(0, 0, 0), 2);
        }
        Cv2.ImShow(title, histImage);
    }
    
    private static void ShowHistogram(string title, IReadOnlyDictionary<IMagickColor<byte>, uint> histogram)
    {
        uint histWidth = 256;
        uint histHeight = 300;
        
        string tempFile = Path.Combine(Path.GetTempPath(), $"{title}.png");
        using var histImage = new MagickImage(MagickColors.White, histWidth, histHeight);

        int x = 0;
        uint previousEndY = histHeight;
        
        foreach (var kvp in histogram)
        {
            uint height = (uint)Math.Round((float)kvp.Value / 2);
            uint currentEndY = histHeight - height;

            histImage.Draw(
                new DrawableStrokeColor(MagickColors.Black),
                new DrawableLine(x, previousEndY, x, currentEndY)
            );
            previousEndY = currentEndY;
            x++;
            if (x >= histWidth) break;
        }
    
        histImage.Write(tempFile);
        Process.Start(new ProcessStartInfo(tempFile) { UseShellExecute = true });
    }
}
