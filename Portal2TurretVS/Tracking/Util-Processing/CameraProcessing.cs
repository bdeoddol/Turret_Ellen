using OpenCvSharp.Flann;

public class CameraProcessing //class holding all pixel coordinate to degrees of motion calculations
{
    //to center, 
    public static SerialCommand Center()
    {
        return new SerialCommand('C');
    }
    public static SerialCommand Pause()
    {
        return new SerialCommand('P');
    }
    public static SerialCommand Sweep()
    {
        return new SerialCommand('S');
    }

    //calculate the tilt and pan degrees required to move relative to the image center
    public static SerialCommand calcBoxTravel(ref StateVar stateVar, OpenCvSharp.Point boxCenter)
    {
        //calculate the pixel differences
        int horiPixelDelta = boxCenter.X - stateVar.cameraCalibration._imgCenter.X;
        int vertPixelDelta = stateVar.cameraCalibration._imgCenter.Y - boxCenter.Y;

        //given 5 pixel rise, given 15 pixels per degree, 
        double panDegrees = horiPixelDelta*stateVar.cameraCalibration.HoriDegreePerPixel;
        double tiltDegrees = vertPixelDelta*stateVar.cameraCalibration.VertDegreePerPixel;
        SerialCommand retCommand = new SerialCommand('M', (int)Math.Round(panDegrees), (int)Math.Round(tiltDegrees), 1);

        return retCommand;
    }

    //same math as above, but generalized to two points, location of the reference point, and the location of the mouse cursor (any point rlly)
    public static SerialCommand calcCursorTravel(ref StateVar stateVar, OpenCvSharp.Point refPoint, OpenCvSharp.Point cursorPoint)
    {
        int horiPixelDelta = cursorPoint.X - refPoint.X;
        int vertPixelDelta = refPoint.Y - cursorPoint.Y;

        //a multiplier is necessary for remote control because if we read pixel deltas of at most 1 or -1, our pan/tilt degrees will be rounded to 0 due to our calibrations being too rough (56.068/400)
        double panDegrees = horiPixelDelta*stateVar.cameraCalibration.HoriDegreePerPixel*stateVar.cameraCalibration.HxVRemoteMultiplier.Item1*stateVar.movementGain;
        double tiltDegrees = vertPixelDelta*stateVar.cameraCalibration.VertDegreePerPixel*stateVar.cameraCalibration.HxVRemoteMultiplier.Item2*stateVar.movementGain;
        SerialCommand retCommand = new SerialCommand('M', (int)Math.Round(panDegrees), (int)Math.Round(tiltDegrees), stateVar.movementGain);

        return retCommand;
    }


}