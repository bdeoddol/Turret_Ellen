using System.ComponentModel;
using System.Data;
using System.DirectoryServices;

public class CameraCalib //holds our camera calibrations 
{
    //update our camera settings when we connect to a camera
    public CameraCalib()
    {
        imgFrameH = -1;
        imgFrameW = -1;
        VertFOV = -1;
        HoriFOV = -1;
    }

    public CameraCalib(int imgHeight, int imgWidth, double imgVertFOV, double imgHoriFOV)
    {
        imgFrameH = imgHeight;
        imgFrameW = imgWidth;
        VertFOV = imgVertFOV;
        HoriFOV = imgHoriFOV;
    }

    //these are initializing variables    
    private int imgFrameH; 
    private int imgFrameW;
    private double VertFOV;
    private double HoriFOV;

    //these are derived variables determined by initializing variables
    public OpenCvSharp.Point _imgCenter => new OpenCvSharp.Point(imgFrameW/2, imgFrameH/2); //updates our camera settings when we connect to the camera
    public int HoriPixelPerDegree => (int)(imgFrameW/HoriFOV);
    public int VertPixelPerDegree => (int)(imgFrameH/VertFOV);
    public double HoriDegreePerPixel => HoriFOV/imgFrameW;
    public double VertDegreePerPixel => VertFOV/imgFrameH;


    // Originally, our deltas are at most 1, thus,
    // 1 * HoriDegreePerPixel(HDPP) = required movement.
    //However, pixel to degree values vary significantly (180 degrees of motion to 400 pixels of image width)
    //This means that our calculated required movement, simply HDPP will be very very small. We typically round our values and write whole degrees as required by the
    //Arduino Servo.h library. That said, our required movement will almost always be rounded down to 0. 
    //To compensate, we include a multiplier such that our required movement is >= abs(1).
    //the calculated multiplier is simply the reciprocal of HoriDegreePerPixel,
    // however because round our calculated pan/tilt degrees to the next integer,  so we only need the multiplier to be at least >0.5, this is why we use 0.53         
    public Tuple<double, double> HxVRemoteMultiplier => new Tuple<double, double>(0.53*HoriPixelPerDegree, 0.53*VertPixelPerDegree);  



    public Tuple<int,int> HxVPixelPerDegree => new Tuple<int, int>((int)(imgFrameW/HoriFOV), (int)(imgFrameH/VertFOV));
    public Tuple<double,double> HxVDegreePerPixel => new Tuple<double, double>( HoriFOV/imgFrameW, VertFOV/imgFrameH);
    


}