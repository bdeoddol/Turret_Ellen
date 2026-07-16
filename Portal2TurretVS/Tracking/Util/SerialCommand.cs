public class SerialCommand //payload class (this is what we will be using to hold our serial data to be sent)
{
    public SerialCommand()
    {
        pan = 0;
        tilt = 0;
    }
    public SerialCommand(int panDegrees, int tiltDegrees)
    {
        if(panDegrees == 90 && tiltDegrees == 90){ centered = true;}
        pan = panDegrees;
        tilt = tiltDegrees;

    }

    public int pan{get;set;}
    public int tilt{get;set;}
    public bool centered{get;set;}
    
}