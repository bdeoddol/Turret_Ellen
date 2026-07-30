public class SerialCommand //payload class (this is what we will be using to hold our serial data to be sent)
{

    /*
    There will be 3 Commands represented as text
    CENTER = C;
    STOP = N;
    SWEEP = S;
    MOVE = M:[pan]x[tilt]x[gain];

    Here pan and tilt are int values casted to characters when transmitted


    ok so if i were to use bytes as our serial protocol, the values at most, would be -180 to 180 theoretically.
    However this is usually not the case because the camera field of vision is almost never one to one with the size of the image frame.
    Assuming that they are not 180 or -180, typically it will probably mch smaller as we are transmitting relative movement,
    so smaller increments at faster speeds (depending on the polling rate, we transmit tilt = +5, pan = -2 as an example).
    This means that I can represent tilt/pan values using sbyte which occupy 1 byte of memoery.
    When i transmit them, I will cast them to int8_t within the arduino program because arduino is written in cpp
     https://www.reddit.com/r/cpp_questions/comments/q7mgpa/difference_between_char_signed_char_unsigned_char/
     https://www.reddit.com/r/cpp_questions/comments/q7mgpa/comment/hgjnyzs/?utm_source=share&utm_medium=web3x&utm_name=web3xcss&utm_term=1&utm_content=share_button

    While preparing them, c# represents signed integers in byte form using "sbyte" https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/integral-numeric-types

    However, let's say that the values could be -180 to 180 for a very very expensive camera
    This means I'd have to represent the movement values as 2byte integers. In C# this is "short". In Cpp, this is "int16_t" or "short" as well
    */

    public SerialCommand()
    {
        cmd = '0';
        pan = 0;
        tilt = 0;
        gain = 0;
    }
    public SerialCommand(char Cmd)
    {
        cmd = Cmd;
        pan = 0;
        tilt = 0;
        gain = 0;
    }

    public SerialCommand(char Cmd, int panDegrees, int tiltDegrees, int setGain)
    {
        cmd = Cmd;
        pan = panDegrees;
        tilt = tiltDegrees;
        gain = setGain;
    }

    public string getString()
    {
        return cmd + ":" + pan + "x" + tilt + "x" + gain;
    }

    private int pan{get;}
    private int tilt{get;}
    private int gain{get;}
    private char cmd{get;}
    
    // public bool centered{get;set;}
    
}