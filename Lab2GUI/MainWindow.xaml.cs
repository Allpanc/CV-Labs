using System.Windows;
using OpenCvSharp;
using OpenCvSharp.WpfExtensions;

namespace Lab2;

public partial class MainWindow
{
    private Mat _grayImage;
    private Mat _sourceImage;

    public MainWindow()
    {
        InitializeComponent();
        LoadImages();
        ShowSourceImage(sender: null, e: null);
    }


    private void LoadImages()
    {
        _sourceImage = ImageLoader.LoadImage(PathProvider.ImagePath);
        _grayImage = ImageLoader.LoadGrayscaleImage(PathProvider.ImagePath);
    }

    private void ShowSourceImage(object sender, RoutedEventArgs e)
    {
        DisplayImage(_sourceImage);
    }
    
    private void ShowGrayscaleImage(object sender, RoutedEventArgs e)
    {
        DisplayImage(_grayImage);
    }

    private void ApplyLinearFilter(object sender, RoutedEventArgs e)
    {
        var filteredImage = FilterProvider.GetLinearFilteredImage(_sourceImage);
        DisplayImage(filteredImage);
    }

    private void ApplyBlur(object sender, RoutedEventArgs e)
    {
        var blurredImage = FilterProvider.GetBlurredImage(_sourceImage);
        DisplayImage(blurredImage);
    }

    private void ApplyErosion(object sender, RoutedEventArgs e)
    {
        var erodedImage = FilterProvider.GetErodedImage(_sourceImage);
        DisplayImage(erodedImage);
    }

    private void ApplyDilation(object sender, RoutedEventArgs e)
    {
        var dilatedImage = FilterProvider.GetDilatedImage(_sourceImage);
        DisplayImage(dilatedImage);
    }

    private void ApplyHorizontalSobel(object sender, RoutedEventArgs e)
    {
        var sobelImages = FilterProvider.GetSobelFilteredImages(_grayImage);
        DisplayImage(sobelImages[0]);
    }

    private void ApplyVerticalSobel(object sender, RoutedEventArgs e)
    {
        var sobelImages = FilterProvider.GetSobelFilteredImages(_grayImage);
        DisplayImage(sobelImages[1]);
    }

    private void ApplyCombinedSobel(object sender, RoutedEventArgs e)
    {
        var sobelImages = FilterProvider.GetSobelFilteredImages(_grayImage);
        DisplayImage(sobelImages[2]);
    }

    private void ApplyLaplacian(object sender, RoutedEventArgs e)
    {
        var laplacianImage = FilterProvider.GetLaplacianFilteredImage(_grayImage);
        DisplayImage(laplacianImage);
    }

    private void ApplyCanny(object sender, RoutedEventArgs e)
    {
        var cannyImage = FilterProvider.GetCannyEdgesFilteredImage(_grayImage);
        DisplayImage(cannyImage);
    }

    private void ApplyHistogram(object sender, RoutedEventArgs e)
    {
        var histogram = HistogramProvider.GetHistogram(_sourceImage);
        DisplayImage(histogram);
    }
    
    private void ApplyEqualizedHistogram(object sender, RoutedEventArgs e)
    {
        var equalizedHistogramImage = HistogramProvider.GetEqualizedHistogramImage(_grayImage);
        DisplayImage(equalizedHistogramImage);
    }

    private void DisplayImage(Mat image)
    {
        ProcessedImage.Source = image.ToBitmapSource();
    }
}