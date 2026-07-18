using OpenCvSharp.Flann;

public class CameraProcessing //class holding all pixel coordinate to degrees of motion calculations
{
    public static SerialCommand Center()
    {
        SerialCommand retCommand = new SerialCommand(90,90);
        return retCommand;
    }

    public static SerialCommand calcBoxTravel(CameraCalib calibrations, OpenCvSharp.Point boxCenter)
    {
        //calculate the pixel differences
        int horiPDelta = boxCenter.X - calibrations._imgCenter.X;
        int vertPDelta = calibrations._imgCenter.Y - boxCenter.Y;

        //given 5 pixel rise, given 15 pixels per degree, 
        int tiltDegrees = horiPDelta/calibrations.HoriPixelPerDegree;
        int panDegrees = vertPDelta/calibrations.VertPixelPerDegree;
        SerialCommand retCommand = new SerialCommand(panDegrees, tiltDegrees);

        return retCommand;
    }


}