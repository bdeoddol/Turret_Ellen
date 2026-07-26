using System.Globalization;
using System.IO.Ports;


class OpenPort
{
    public static void Run()
    {

        String[] names = SerialPort.GetPortNames();
        SerialPort _serialPort = new SerialPort();
        String portName = "hi";

        foreach(string val in names)
        {
            Console.WriteLine(val);
            portName = val;
        }
        try
        {
            _serialPort = new SerialPort(portName);
            _serialPort.BaudRate = 9600;

            _serialPort.Open();
            Console.WriteLine("i'm working");
        }
        catch
        {
            Console.WriteLine("nope");
            return;
        }

        _serialPort.WriteLine("Remote"); //note that c# strings are not null terminated
        // _serialPort.WriteLine("hello world");
        _serialPort.Close();

        return;
    }
}