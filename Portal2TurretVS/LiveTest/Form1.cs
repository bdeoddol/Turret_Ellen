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
        private Mat? _frame;
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
            _frame = new Mat();
            _frame.ConvertTo(_frame, MatType.CV_32F); //convert the frame_ matrix into a 32-bit float type casted to int = 5;
            SetCameraButtonActive();

            _running = false;
            _worker?.IsBackground = true;
        }

        private void Form1_Closed(object? sender, EventArgs e)
        {
            //once form is closed, release other resources
            if (_captures != null)
            {
                _captures?.Release();
                _captures?.Dispose();
            }
            if (_frame != null)
            {
                _frame?.Dispose();
                _frame = null;
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
                    if (_frame == null || _captures == null || !_captures.IsOpened() || !pictureBox1.IsHandleCreated) { continue; }
                    _captures?.Read(_frame); //decode the next frame from the video stream and store it in _frame

                    if (pictureBox1.InvokeRequired == true && !pictureBox1.IsDisposed) //required as per https://www.visioforge.com/help/docs/dotnet/general/code-samples/draw-video-picturebox/
                    {
                        //invoke marshals the frame swapping to the UI thread, ensuring thread safety when updating the PictureBox control
                        //we basically delegate the tasks within swapFrames to a UI thread instead of direct access via this worker thread.
                        pictureBox1.BeginInvoke(new Action(swapFrames)); //https://stackoverflow.com/questions/229554/whats-the-difference-between-invoke-and-begininvoke
                    }
                }

            }
            return;
        }

        private void swapFrames()
        {

            if (!_alive || _frame == null || _frame.Empty() || pictureBox1.IsDisposed) { return; } //bail out of the method if any of these are true.
            //swap the old frame with new one, display new frame, free memory of the old frame
            if (_displayFrame != null) { _oldFrame = _displayFrame; }
            _displayFrame = BitmapConverter.ToBitmap(_frame);
            pictureBox1.Image = _displayFrame;
            _oldFrame?.Dispose();
        }

        private void StartStream_Click(object sender, EventArgs e) //toggle
        {
            if (_running == true) { _running = false; }
            else
            {
                pictureBox1.Show();
                _running = true;
            }


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


            SetDisconnectButtonActive();
            return;
        }

        private void DisconnectCamera_Click(object sender, EventArgs e)
        {
            _running = false;
            _alive = false;

            if (_worker != null && _worker.IsAlive) { _worker.Join(500); }
            _captures?.Release();
            _captures?.Dispose();
            _captures = null;
            _frame?.Release();
            _frame?.Dispose();
            _frame = null;

            pictureBox1.Hide();
            SetCameraButtonActive();
            return;
        }

        private void SetDisconnectButtonActive()
        {
            ConnectCamera.Enabled = false;
            DisconnectCamera.Enabled = true;
            StartStream.Enabled = true;
            StartStream.Visible = true;
        }

        private void SetCameraButtonActive()
        {
            ConnectCamera.Enabled = true;
            DisconnectCamera.Enabled = false;
            StartStream.Enabled = false;
            StartStream.Visible = false;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
