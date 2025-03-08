using OpenCvSharp;

namespace Lab2;

internal static class EntryPoint
{
    private static void Main()
    {
        var sourceImage = ImageLoader.LoadImage(PathProvider.ImagePath);
        var grayScaleImage = ImageLoader.LoadGrayscaleImage(PathProvider.ImagePath);

        Cv2.ImShow("Source", sourceImage);
        Cv2.ImShow("Grayscale", grayScaleImage);

        var linearFilteredImg = FilterProvider.GetLinearFilteredImage(sourceImage);
        Cv2.ImShow("Linear", linearFilteredImg);

        var blurredImage = FilterProvider.GetBlurredImage(sourceImage);
        Cv2.ImShow("Blur", blurredImage);

        var erodedImage = FilterProvider.GetErodedImage(sourceImage);
        Cv2.ImShow("Erosion", erodedImage);

        var dilatedImage = FilterProvider.GetDilatedImage(sourceImage);
        Cv2.ImShow("Dilation", dilatedImage);

        var sobelFilteredImages = FilterProvider.GetSobelFilteredImages(grayScaleImage);
        Cv2.ImShow("Sobel X", sobelFilteredImages[0]);
        Cv2.ImShow("Sobel Y", sobelFilteredImages[1]);
        Cv2.ImShow("Sobel", sobelFilteredImages[2]);

        var laplacianFilteredImage = FilterProvider.GetLaplacianFilteredImage(grayScaleImage);
        Cv2.ImShow("Laplacian", laplacianFilteredImage);

        var cannyEdgesFilteredImage = FilterProvider.GetCannyEdgesFilteredImage(grayScaleImage);
        Cv2.ImShow("Canny", cannyEdgesFilteredImage);

        var histogram = HistogramProvider.GetHistogram(sourceImage);
        Cv2.ImShow("Histogram", histogram);

        var equalizedHistogramImage = HistogramProvider.GetEqualizedHistogramImage(grayScaleImage);
        Cv2.ImShow("Equalized histogram image", equalizedHistogramImage);

        Cv2.WaitKey();
    }
}