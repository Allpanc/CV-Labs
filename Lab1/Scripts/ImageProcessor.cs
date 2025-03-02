using OpenCvSharp;

namespace Lab1;

internal static class ImageProcessor
{
    public static void ProcessImage(string path)
    {
        var srcImage = Cv2.ImRead(path);
        Cv2.ImShow("Source", srcImage);

        var grayscaleImage = GetGrayscale(srcImage);
        Cv2.ImShow("Grayscale", grayscaleImage);
        
        var binaryImage = GetBinary(grayscaleImage);
        Cv2.ImShow("Binary", binaryImage);

        var contourImage = GetContour(srcImage, binaryImage);
        Cv2.ImShow("Contour", contourImage);

        Cv2.WaitKey();
    }

    private static Mat GetGrayscale(Mat srcImage)
    {
        var grayscaleImage = new Mat();
        Cv2.CvtColor(srcImage, grayscaleImage, ColorConversionCodes.BGR2GRAY);
        
        return grayscaleImage;
    }

    private static Mat GetBinary(Mat grayscaleImage)
    {
        var binaryImage = new Mat();
        Cv2.Threshold(grayscaleImage, binaryImage, 100, 255, ThresholdTypes.Binary);
        
        return binaryImage;
    }

    private static Mat GetContour(Mat srcImage, Mat binaryImage)
    {
        var contourColor = new Scalar(255, 255, 255);
        var contourThickness = 2;
        
        var contourImage = srcImage.Clone();
        Cv2.FindContours(binaryImage, out var contours, out _, RetrievalModes.CComp,
            ContourApproximationModes.ApproxSimple);
        Cv2.DrawContours(contourImage, contours, -1, contourColor, contourThickness);

        return contourImage;
    }
}