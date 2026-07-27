using OpenCvSharp.Flann;

public class CameraProcessing //class holding all pixel coordinate to degrees of motion calculations
{
    public static SerialCommand Center()
    {
        SerialCommand retCommand = new SerialCommand(90,90);
        return retCommand;
    }

    //calculate the tilt and pan degrees required to move relative to the image center
    public static SerialCommand calcBoxTravel(CameraCalib calibrations, OpenCvSharp.Point boxCenter)
    {
        //calculate the pixel differences
        int horiPixelDelta = boxCenter.X - calibrations._imgCenter.X;
        int vertPixelDelta = calibrations._imgCenter.Y - boxCenter.Y;

        //given 5 pixel rise, given 15 pixels per degree, 
        double tiltDegrees = horiPixelDelta*calibrations.HoriDegreePerPixel;
        double panDegrees = vertPixelDelta*calibrations.VertDegreePerPixel;
        SerialCommand retCommand = new SerialCommand((int)Math.Round(panDegrees), (int)Math.Round(tiltDegrees));

        return retCommand;
    }

    //same math as above, but generalized to two points, location of the reference point, and the location of the mouse cursor (any point rlly)
    public static SerialCommand calcCursorTravel(CameraCalib calibrations, OpenCvSharp.Point refPoint, OpenCvSharp.Point cursorPoint)
    {
        int horiPixelDelta = refPoint.X - cursorPoint.X;
        int vertPixelDelta = cursorPoint.Y - refPoint.X;

        double tiltDegrees = horiPixelDelta*calibrations.HoriDegreePerPixel;
        double panDegrees = vertPixelDelta*calibrations.VertDegreePerPixel;
        SerialCommand retCommand = new SerialCommand((int)Math.Round(panDegrees), (int)Math.Round(tiltDegrees));

        return retCommand;
    }


}