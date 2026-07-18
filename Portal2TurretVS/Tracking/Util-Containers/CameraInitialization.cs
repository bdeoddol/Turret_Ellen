using System.ComponentModel;
using System.Data;

public class CameraCalib //holds our camera calibrations 
{
    //update our camera settings when we connect to a camera
    public CameraCalib()
    {
        imgFrameH = 0;
        imgFrameW = 0;
        VertFOV = 1;
        HoriFOV = 1;

    }

    public CameraCalib(int imgHeight, int imgWidth, int imgVertFOV, int imgHoriFOV)
    {
        imgFrameH = imgHeight;
        imgFrameW = imgWidth;
        VertFOV = imgVertFOV;
        HoriFOV = imgHoriFOV;
    }

    //these are initializing variables    
    public int imgFrameH{get;set;} 
    public int imgFrameW{get;set;}
    public double VertFOV{get;set;}
    public double HoriFOV{get;set;}

    //these are derived variables determined by initializing variables
    public OpenCvSharp.Point _imgCenter => new OpenCvSharp.Point(imgFrameW/2, imgFrameW/2); //updates our camera settings when we connect to the camera

    public int HoriPixelPerDegree => (int)(imgFrameW/HoriFOV);
    public int VertPixelPerDegree => (int)(imgFrameH/VertFOV);



}