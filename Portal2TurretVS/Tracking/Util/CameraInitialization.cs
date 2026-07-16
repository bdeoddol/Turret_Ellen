using System.Data;

public class CameraInit //holds our camera calibrations
{
    public CameraInit()
    {
        _imgCenter = new OpenCvSharp.Point();
    }
    public OpenCvSharp.Point _imgCenter{get;set;}
    public int imgFrameH{get;set;}
    public int imgFrameW{get;set;}
    public int VertFOV{get;set;}
    public int HoriFOV{get;set;}

    public void update() //update our camera settings when we connect to the camera
    {
        _imgCenter = new OpenCvSharp.Point(imgFrameW/2, imgFrameH/2);

    }

}