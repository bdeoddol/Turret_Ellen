using System.Globalization;
using System.IO.Ports;


class OpenPort
{
    public static void Run()
    {

        String[] names = SerialPort.GetPortNames();
        SerialPort _serialPort = new SerialPort();

        foreach(string val in names)
        {
            Console.WriteLine(val);
        }
        String portName = "COM4";
        _serialPort = new SerialPort(portName);
        
    }
}