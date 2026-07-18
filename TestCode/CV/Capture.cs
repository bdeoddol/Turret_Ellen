using System.Drawing.Imaging.Effects;
using OpenCvSharp;

public class Capture
{
    public static void Run()
    {
        VideoCapture _capture = new VideoCapture(0);
        Mat imgframe = new Mat();
        _capture.Set(VideoCaptureProperties.FrameHeight, 1080);
        _capture.Set(VideoCaptureProperties.FrameWidth, 1920);
        _capture.Read(imgframe);
        Cv2.ImWrite("capturedimg.jpg", imgframe);
        Console.WriteLine("Img height: " + imgframe.Height);
        Console.WriteLine("Img Width: " + imgframe.Width);
        
    }
}