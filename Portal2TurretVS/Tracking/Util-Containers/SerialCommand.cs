public class SerialCommand //payload class (this is what we will be using to hold our serial data to be sent)
{

    /*
    There will be 3 Commands represented in bytes
    CENTER = 0x03;
    STOP = 0x06;
    SWEEP = 0x09;
    MOVE = 0x0C[pan][tilt];

    */

    public SerialCommand()
    {
        cmd = 0x00;
        pan = 0;
        tilt = 0;
    }
    public SerialCommand(byte Cmd)
    {
        cmd = Cmd;
        pan = 0;
        tilt = 0;
    }

    public SerialCommand(byte Cmd, int panDegrees, int tiltDegrees)
    {
        cmd = Cmd;
        pan = panDegrees;
        tilt = tiltDegrees;
    }

    public string getString()
    {
        return cmd + ":" + pan + "x" + tilt;
    }

    private int pan{get;}
    private int tilt{get;}
    private byte cmd{get;}
    // public bool centered{get;set;}
    
}