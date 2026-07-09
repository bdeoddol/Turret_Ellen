using System.IO.Ports

class OpenPort
{
    public static void Run()
    {
        SerialPort _serialPort = new SerialPort();
        SerialPort.GetPortNames();
    }
}