using ByteTrackCSharp;
using Microsoft.ML.OnnxRuntime;
using OpenCvSharp;
using OpenCvSharp.Dnn;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Immutable;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.IO.Ports;
using System.Numerics.Tensors;
using System.Runtime.InteropServices.Marshalling;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Windows.Forms;




namespace Tracking
{
    public partial class Form1 : Form
    {

        private VideoCapture? _captures;
        private Mat? _srcFrame;
        private Mat? _processedFrame;
        private Bitmap? _displayFrame;
        private Bitmap? _oldFrame;

        private bool _running;
        private bool _alive;
        private Thread? _swapThread;
        private Thread? _captureThread;


        private bool _trackingMode;
        private BYTETracker? _trackingSession;
        private InferenceSession? _currModel;
        private string? _modelPath;
        private float[]? src;
        private long[]? shape;
        private bool resized = false;


        private int frameCnt = 0;
        private Stopwatch totalRuntime = new Stopwatch();
        string? fpsDisplay;

        private bool _ardConnected;
        private SerialPort? _serialPort;
        private int[] _baudRates = { 9600, 19200, 38400, 57600, 115200 };
        private int _selectedBaud;
        private string? _selectedPort;


        TurrState _currState;


        ///////////////////////////setup and teardown///////////////////////////
        public Form1()
        {
            InitializeComponent();
            // Load += Form1_Load;
            FormClosed += Form1_Closed;
            FormClosing += Form1_FormClosing;
        }

        private void Form1_Load(object? sender, EventArgs e)
        {
            _srcFrame = new Mat();
            SetCameraButtonActive();

            _running = false;
            _swapThread?.IsBackground = true;
            _captureThread?.IsBackground = true;


            deactivateTrackMode();
            //_modelPath = "..\\..\\..\\assets\\yolo26n.onnx"; //relative file path from the project executable. 
            _modelPath = Path.Combine(AppContext.BaseDirectory, "assets", "yolo26n.onnx"); //file pathing when asset folder exists at location of .exe output
            try
            {
                using var options = SessionOptions.MakeSessionOptionWithCudaProvider(0);
                _currModel = new InferenceSession(_modelPath, options);

            }   // use CUDA
            catch
            {
                Console.WriteLine("falling back to CPU");
                _currModel = new InferenceSession(_modelPath);
            }   // fallback and use CPU    


            PortDropDown.DataSource = SerialPort.GetPortNames();
            BaudDropDown.DataSource = _baudRates;
            try { PortDropDown.SelectedIndex = 0; } catch { PortDropDown.SelectedIndex = -1; }
            BaudDropDown.SelectedIndex = 0;
            _ardConnected = false;

        }

        private void Form1_Closed(object? sender, EventArgs e)
        {
            //once form is closed, release other resources
            if (_captures != null)
            {
                _captures?.Release();
                _captures?.Dispose();
            }
            if (_srcFrame != null)
            {
                _srcFrame?.Dispose();
                _srcFrame = null;
            }
            if (_currModel != null)
            {
                _currModel.Dispose();
            }
            if (_serialPort != null)
            {
                _serialPort.Close();
                _serialPort.Dispose();
            }

        }

        private void Form1_FormClosing(object? sender, FormClosingEventArgs e)
        //kill the worker as we are closing the form, otherwise it will keep running in the background and cause memory leaks
        {
            KillThreads();
        }

        private void KillThreads()
        {
            _running = false;
            _alive = false;
            if (_captureThread != null && _swapThread != null) { _captureThread?.Join(500); _swapThread?.Join(500); }
        }




    ///////////////////////////Threads///////////////////////////
        private void grabFrame()
        {
            while (_alive == true)
            {
                Thread.Sleep(25);
                if (_running == true)
                {
                    if (_srcFrame == null || _captures == null || !_captures.IsOpened() || !pictureBox1.IsHandleCreated) { continue; }
                    _captures?.Read(_srcFrame); //decode the next frame from the video stream and store it in _frame

                }

            }
            return;
        }


        private void Stream()
        {
            totalRuntime.Start();
            while (_alive == true)
            {
                Thread.Sleep(50);
                if (_running == true)
                {
                    if (_srcFrame == null || _srcFrame.Empty() || _captures == null || !_captures.IsOpened() || !pictureBox1.IsHandleCreated) { continue; }
                    _processedFrame = _srcFrame;

                    if (_trackingMode == true) 
                    { 
                        trackInFrame();
                        if(_ardConnected == true)
                        {
                            
                        }
                    }
                    if (pictureBox1.InvokeRequired == true && !pictureBox1.IsDisposed) //required as per https://www.visioforge.com/help/docs/dotnet/general/code-samples/draw-video-picturebox/
                    {
                        // marshals the frame swapping to the UI thread, ensuring thread safety when updating the PictureBox control
                        //we delegate the tasks within swapFrames to a UI thread instead of direct access via this worker thread.
                        pictureBox1.BeginInvoke(new Action(swapFrames)); //https://stackoverflow.com/questions/229554/whats-the-difference-between-invoke-and-begininvoke
                        frameCnt++;
                    }
                }

            }
            return;
        }

        private void swapFrames()
        {

            if (!_alive || _processedFrame == null || _processedFrame.Empty() || pictureBox1.IsDisposed) { return; } //bail out of the method if any of these are true.
            //swap the old frame with new one, display new frame, free memory of the old frame
            if (_displayFrame != null) { _oldFrame = _displayFrame; }

            double elapsedSeconds = totalRuntime.Elapsed.TotalSeconds;
            if (elapsedSeconds > 0)
            {
                double fpsVal = frameCnt / elapsedSeconds;
                fpsDisplay = $"FPS: {fpsVal:F1}";
                Cv2.PutText(_processedFrame, fpsDisplay, new OpenCvSharp.Point(10, 25), HersheyFonts.HersheySimplex, 0.5, Scalar.White);

            }

            _displayFrame = BitmapConverter.ToBitmap(_processedFrame);
            pictureBox1.Image = _displayFrame;
            _oldFrame?.Dispose();
            frameCnt++;
        }

        private void trackInFrame()
        {
            //capture the frame, process it within the InferenceSession, display the output
            ///////////////////////////preprocessing ////////////////////////////////
            if (_processedFrame == null) { return; }
            OpenCvSharp.Rect ROICrop = Preprocessing.GetRectOfOriginalFrame(_processedFrame);
            int origWidth = _processedFrame.Width;
            int origHeight = _processedFrame.Height;
            if (Preprocessing.ValidateImgDim(_processedFrame) == false)
            {
                int aUWidth = Preprocessing.AlignUp(_processedFrame.Width) * 32;
                int aUHeight = Preprocessing.AlignUp(_processedFrame.Height) * 32;
                Preprocessing.performResize(_processedFrame, aUWidth, aUHeight);
                Preprocessing.performPaddingVert(_processedFrame, aUHeight);
                resized = true;
            }
            src = Preprocessing.prepareSrc(_processedFrame);
            shape = Preprocessing.prepareShape(_processedFrame);


            if (_currModel != null)
            {

                IDisposableReadOnlyCollection<OrtValue> sampleOutput = Postprocessing.infer(src, shape, _currModel); //infer here
                ///////////////////////////Postprocessing ////////////////////////////////
                ImmutableList<Detection> detections = Postprocessing.parseOutputData(sampleOutput);
                if (_trackingSession != null)
                {
                    //prepare detections for BYTETrack
                    List<ByteTrackCSharp.Object> BTObjs = new List<ByteTrackCSharp.Object>();
                    foreach (Detection val in detections)
                    { BTObjs.Add(Postprocessing.DetToByteTrackObject(val)); }

                    //update BYTETrack and get new tracking results
                    List<STrack> outputTracks = _trackingSession.update(BTObjs);
                    detections = Postprocessing.ListSTrackToDet(outputTracks);
                }

                //draw our detections and resize image back to original size if necessary
                Postprocessing.plotDetections(detections, _processedFrame);
                if (resized == true)
                {
                    //resize image back to original framesize to fit pictureBox
                    _processedFrame = new Mat(_processedFrame, ROICrop);
                    Preprocessing.performResize(_processedFrame, origWidth, origHeight);
                    resized = false;
                }

                //update the state variable TODO:









                sampleOutput.Dispose();
            }
            
        }



        ///////////////////////////buttons///////////////////////////

        private void PortDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            _selectedPort = PortDropDown.SelectedText;
        }

        private void PortRefresh_Click(object sender, EventArgs e)
        {
            PortDropDown.DataSource = null;
            PortDropDown.DataSource = SerialPort.GetPortNames();
        }

        private void BaudDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (BaudDropDown.SelectedItem != null){ _selectedBaud = (int)BaudDropDown.SelectedItem; }
        }

        private void ConnectArduino_Click(object sender, EventArgs e)
        { SerialPortConnect();}

        private void SerialPortConnect()
        {
            
            _selectedPort = PortDropDown.SelectedText;
            if (BaudDropDown.SelectedItem is int)
            {
                _selectedBaud = (int)BaudDropDown.SelectedItem;
            }
            try {             
                _serialPort = new SerialPort(_selectedPort);
                _serialPort.BaudRate = _selectedBaud;
                _serialPort.Open();
                }
            catch
            {
                MessageBox.Show("Failed connected to Arduino! Please check your settings and connection.");
                _ardConnected = false;
                return;
            }
            _ardConnected = true;
            return;
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
            _srcFrame = new Mat();
            if (_captures.IsOpened() == false)
            {
                MessageBox.Show("Could not connect to camera. Please connect a camera and try again.");
                return;
            }
            _captureThread = new Thread(new ThreadStart(grabFrame));
            _swapThread = new Thread(new ThreadStart(Stream));
            _alive = true;
            _captureThread.Start();
            _swapThread.Start();
            _trackingSession = new BYTETracker(Postprocessing.ObjToSTrack, 16, 150, (float)0.5, (float)0.5, (float)0.6);

            SetDisconnectButtonActive();
            return;
        }

        private void DisconnectCamera_Click(object sender, EventArgs e)
        {
            KillThreads();

            _captures?.Release();
            _captures?.Dispose();
            _captures = null;
            _srcFrame?.Release();
            _srcFrame?.Dispose();
            _srcFrame = null;

            pictureBox1.Hide();
            SetCameraButtonActive();
            return;
        }

        private void SetDisconnectButtonActive()
        {
            ConnectCamera.Enabled = false;
            DisconnectCamera.Enabled = true;
            StartStream.Enabled = true;
        }

        private void SetCameraButtonActive()
        {
            ConnectCamera.Enabled = true;
            DisconnectCamera.Enabled = false;
            StartStream.Enabled = false;
        }

        //private void pictureBox1_Click(object sender, EventArgs e)
        //{

        //}

        private void TrackEnable_Click(object sender, EventArgs e)
        { activateTrackMode();}

        private void activateTrackMode()
        {
            _trackingMode = true;
            TrackEnable.Enabled = false;
            TrackEnable.Visible = false;
            DisableTrack.Enabled = true;
            DisableTrack.Visible = true;
        }

        private void DisableTrack_Click(object sender, EventArgs e)
        { deactivateTrackMode();}

        private void deactivateTrackMode()
        {
            _trackingMode = false;
            TrackEnable.Enabled = true;
            TrackEnable.Visible = true;
            DisableTrack.Enabled = false;
            DisableTrack.Visible = false;
        }
    }
}
