using System;
using OpenCvSharp;

namespace Lab2;

internal static class ImageLoader
{
    public static Mat LoadImage(string imagePath)
    {
        var img = Cv2.ImRead(imagePath);

        if (img.Empty()) throw new Exception("Ошибка: не удалось загрузить изображение!");

        return img;
    }

    public static Mat LoadGrayscaleImage(string imagePath)
    {
        var img = LoadImage(imagePath);

        var grayImg = new Mat();
        Cv2.CvtColor(img, grayImg, ColorConversionCodes.BGR2GRAY);

        return grayImg;
    }
}