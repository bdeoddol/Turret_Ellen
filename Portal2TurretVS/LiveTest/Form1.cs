using System;
using System.Windows.Forms;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using System.Threading;
using System.Text;

namespace LiveTest
{
    public partial class Form1 : Form
    {
        //via https://elbruno.com/2020/11/16/dotnet-display-the-%F0%9F%8E%A6-camera-feed-in-a-winform-using-opencv-and-net5/

        private VideoCapture? _captures;
        private Mat? _frame = new Mat();
        private Bitmap? _displayFrame;
        private Bitmap? _oldFrame;
        private bool _running;
        private bool _alive;
        private Thread? _worker;
        public Form1()
        {
            InitializeComponent();
            Load += Form1_Load;
            FormClosed += Form1_Closed;
            FormClosing += Form1_FormClosing;
        }

        private void Form1_Load(object? sender, EventArgs e)
        {
            ConnectCamera.Enabled = true;
            DisconnectCamera.Enabled = false;
            StartStream.Enabled = false;
            StartStream.Visible = false;

            _running = false;
            _worker = new Thread(new ThreadStart(Stream));
            _worker.IsBackground = true;
            _alive = true;
            _worker.Start();
        }

        private void Form1_Closed(object? sender, EventArgs e)
        {
            //once form is closed, release other resources
            if(_captures != null)
            {
                _captures?.Release();
                _captures?.Dispose();
            }
            if(_frame != null)
            {
                _frame?.Release();
                _frame?.Dispose();
            }
        }

        private void Form1_FormClosing(object? sender, FormClosingEventArgs e) 
            //kill the worker as we are closing the form, otherwise it will keep running in the background and cause memory leaks
        {
            _running = false;
            _alive = false;
            if (_worker != null && _worker.IsAlive) { _worker?.Join(500); }

        }



        private void Stream()
        {
            while (_alive == true)
            {
                Thread.Sleep(100);
                if (_running == true)
                {
                    if (_frame == null || _captures == null || !_captures.IsOpened()) { continue; }
                    _captures?.Read(_frame); //decode the next frame from the video stream and store it in _frame

                    if (pictureBox1.InvokeRequired == true && !pictureBox1.IsDisposed) //required as per https://www.visioforge.com/help/docs/dotnet/general/code-samples/draw-video-picturebox/
                    {
                        //invoke marshals the frame swapping to the UI thread, ensuring thread safety when updating the PictureBox control
                        //we basically delegate the tasks within swapFrames to a UI thread instead of direct access via this worker thread.
                        pictureBox1.Invoke(new Action(swapFrames));
                    }
                }
                
            }
            return;
        }

        private void swapFrames()
        {
            if (_frame == null || _frame.Empty() || pictureBox1.IsDisposed) { return; }
            //swap the old frame with new one, display new frame, free memory of the old frame
            if (_displayFrame != null) { _oldFrame = _displayFrame; }
            _displayFrame = BitmapConverter.ToBitmap(_frame);
            pictureBox1.Image = _displayFrame;
            _oldFrame?.Dispose();
        }

        private void StartStream_Click(object sender, EventArgs e) //toggle
        {
            if (_running == true) { _running = false; }
            else { _running = true; }


            return;
        }

        private void ConnectCamera_Click(object sender, EventArgs e)
        {
            _captures = new VideoCapture(0);
            _frame = new Mat();
            if (_captures.IsOpened() == false)
            {
                MessageBox.Show("Could not connect to camera. Please connect a camera and try again.");
                return;
            }
            _worker = new Thread(new ThreadStart(Stream));
            _alive = true;
            _worker.Start();

            ConnectCamera.Enabled = false;
            DisconnectCamera.Enabled = true;
            StartStream.Enabled = true;
            StartStream.Visible = true;
            return;
        }

        private void DisconnectCamera_Click(object sender, EventArgs e)
        {
            _running = false;
            _alive = false;

            if (_worker != null && _worker.IsAlive) {_worker.Join(500);}
            _captures?.Release();
            _captures?.Dispose();
            _captures = null;
            _frame?.Release();
            _frame?.Dispose();
            _frame = null;

            ConnectCamera.Enabled = true;
            DisconnectCamera.Enabled = false;
            StartStream.Enabled = false;
            StartStream.Visible = false;
            return;
        }
    }
}
