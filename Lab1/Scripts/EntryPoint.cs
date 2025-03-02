namespace Lab1;

internal static class EntryPoint
{
    private static void Main()
    {
        // Comment or uncomment to use image processor or face detector
        ImageProcessor.ProcessImage(PathProvider.ImagePath);
        //FaceDetector.Detect(PathProvider.ModelPath);
    }
}