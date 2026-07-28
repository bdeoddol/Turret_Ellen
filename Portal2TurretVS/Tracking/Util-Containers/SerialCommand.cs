public class SerialCommand //payload class (this is what we will be using to hold our serial data to be sent)
{

    /*
    There will be 3 Commands represented in bytes
    CENTER = C;
    STOP = N;
    SWEEP = S;
    MOVE = M:[pan]x[tilt];

    */

    public SerialCommand()
    {
        cmd = '0';
        pan = 0;
        tilt = 0;
    }
    public SerialCommand(char Cmd)
    {
        cmd = Cmd;
        pan = 0;
        tilt = 0;
    }

    public SerialCommand(char Cmd, int panDegrees, int tiltDegrees)
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
    private char cmd{get;}
    // public bool centered{get;set;}
    
}