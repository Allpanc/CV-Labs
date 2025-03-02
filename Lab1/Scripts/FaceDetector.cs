using OpenCvSharp;

namespace Lab1;

internal static class FaceDetector
{
    public static void Detect(string modelPath)
    {
        var capture = new VideoCapture(0);
        var cascade = new CascadeClassifier(modelPath);
        var frame = new Mat();
        var rectColor = new Scalar(255, 255, 255);
        var rectThickness = 2;

        while (true)
        {
            capture.Read(frame);
            var gray = new Mat();
            Cv2.CvtColor(frame, gray, ColorConversionCodes.BGR2GRAY);

            Rect[] faces = cascade.DetectMultiScale(gray);
            
            foreach (var face in faces)
            {
                Cv2.Rectangle(frame, face, rectColor, rectThickness);
            }

            Cv2.ImShow("Video Stream", frame);
            
            if (EscapePressed())
            {
                break;
            }
        }

        capture.Release();
        Cv2.DestroyAllWindows();
    }

    private static bool EscapePressed()
    {
        return Cv2.WaitKey(1) == 27;
    }
}