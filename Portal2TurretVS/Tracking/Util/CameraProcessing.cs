using OpenCvSharp.Flann;

public class CameraProcessing //class holding all pixel coordinate to degrees of motion calculations
{
    public static SerialCommand Center()
    {
        SerialCommand retCommand = new SerialCommand(90,90);
        return retCommand;
    }


}