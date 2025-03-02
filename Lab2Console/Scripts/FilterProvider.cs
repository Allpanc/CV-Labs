using OpenCvSharp;

namespace Lab2;

internal static class FilterProvider
{
    public static Mat GetLinearFilteredImage(Mat image)
    {
        var kernel = new Mat(3, 3, MatType.CV_32F);
        kernel.Set(0, 0, -0.1f);
        kernel.Set(0, 1, 0.2f);
        kernel.Set(0, 2, -0.1f);
        kernel.Set(1, 0, 0.2f);
        kernel.Set(1, 1, 3.0f);
        kernel.Set(1, 2, 0.2f);
        kernel.Set(2, 0, -0.1f);
        kernel.Set(2, 1, 0.2f);
        kernel.Set(2, 2, -0.1f);

        var linearFilteredImg = new Mat();
        Cv2.Filter2D(image, linearFilteredImg, -1, kernel);

        return linearFilteredImg;
    }


    public static Mat GetBlurredImage(Mat image)
    {
        var blurImg = new Mat();
        Cv2.Blur(image, blurImg, new Size(5, 5));

        return blurImg;
    }

    public static Mat GetErodedImage(Mat image)
    {
        var erodeImg = new Mat();
        var element = Cv2.GetStructuringElement(MorphShapes.Rect, new Size(6, 6));
        Cv2.Erode(image, erodeImg, element);

        return erodeImg;
    }

    public static Mat GetDilatedImage(Mat image)
    {
        var dilateImg = new Mat();
        var element = Cv2.GetStructuringElement(MorphShapes.Rect, new Size(6, 6));
        Cv2.Dilate(image, dilateImg, element);

        return dilateImg;
    }

    public static Mat[] GetSobelFilteredImages(Mat image)
    {
        Mat gradX = new(), gradY = new(), gradXAbs = new(), gradYAbs = new(), grad = new();
        Cv2.Sobel(image, gradX, MatType.CV_16S, 1, 0);
        Cv2.Sobel(image, gradY, MatType.CV_16S, 0, 1);
        Cv2.ConvertScaleAbs(gradX, gradXAbs);
        Cv2.ConvertScaleAbs(gradY, gradYAbs);
        Cv2.AddWeighted(gradXAbs, 0.5, gradYAbs, 0.5, 0, grad);

        return new[]
        {
            gradXAbs,
            gradYAbs,
            grad
        };
    }

    public static Mat GetLaplacianFilteredImage(Mat image)
    {
        var laplacian = new Mat();
        Cv2.Laplacian(image, laplacian, MatType.CV_16S);

        var laplacianAbs = new Mat();
        Cv2.ConvertScaleAbs(laplacian, laplacianAbs);

        return laplacianAbs;
    }

    public static Mat GetCannyEdgesFilteredImage(Mat image)
    {
        var edges = new Mat();
        Cv2.Canny(image, edges, 70, 260);

        return edges;
    }
}