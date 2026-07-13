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
        String portName = "hi";
        _serialPort = new SerialPort(portName);
        _serialPort.BaudRate = 9600;

        _serialPort.Open();
        // _serialPort.WriteLine("hi testing");
        _serialPort.Close();
        
    }
}