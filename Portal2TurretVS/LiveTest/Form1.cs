using System;
using System.Windows.Forms;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using System.Threading;

namespace LiveTest
{
    public partial class Form1 : Form
    {

        private VideoCapture? _captures;
        private Mat? _frame = new Mat();
        private bool _running;
        private bool _alive;
        private Thread? _worker;
        public Form1()
        {
            InitializeComponent();
            Load += Form1_Load;
            Closed += Form1_Closed;
        }

        private void Form1_Load(object? sender, EventArgs e)
        {
            _captures = new VideoCapture(0);
            _frame = new Mat();
            _running = false;
            _worker = new Thread(new ThreadStart(Stream));
            _alive = true;
            _worker.Start();

            if(_captures == null)
            {
                MessageBox.Show("VideoCapture device is not detected");
            }
        }

        private void Form1_Closed(object? sender, EventArgs e)
        {
            _alive = false;
            _worker.Join();
            _captures.Release();
        }



        private void Stream()
       {
            while(_alive == true)
            {
                if (_running == true)
                {
                    _captures.Read(_frame);
                    if (_frame.Empty() || !_captures.IsOpened()) { continue; }
                    pictureBox1.Image = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(_frame);
                    Thread.Sleep(50);
                }   
            }
            return;
        } 

        private void button1_Click(object sender, EventArgs e)
        {
            if(_running == true)
            {
                _running = false;
            }
            else
            {
                _running = true;
            }

            return;
        }
    }
}
