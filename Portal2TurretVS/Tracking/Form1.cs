using ByteTrackCSharp;
using Microsoft.ML.OnnxRuntime;
using OpenCvSharp;
using OpenCvSharp.Dnn;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Immutable;
using System.Collections.Specialized;
using System.Diagnostics;
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
        //via https://elbruno.com/2020/11/16/dotnet-display-the-%F0%9F%8E%A6-camera-feed-in-a-winform-using-opencv-and-net5/

        private VideoCapture? _captures;
        private Mat? _srcFrame;
        private Mat? _processedFrame;
        private Bitmap? _displayFrame;
        private Bitmap? _oldFrame;

        private bool _running;
        private bool _alive;
        private Thread? _swapThread;
        private Thread? _captureThread;

        private InferenceSession? _currModel;
        private string? _modelPath;
        private float[]? src;
        private long[]? shape;
        private bool resized = false;
        private OpenCvSharp.Rect ROICrop;
        private int origWidth, origHeight;


        private int frameCnt = 0;
        private Stopwatch totalRuntime = new Stopwatch();
        string fpsDisplay = "Calculating...";


        private BYTETracker? _trackingSession;


        public Form1()
        {
            InitializeComponent();
            Load += Form1_Load;
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

            //_modelPath = "..\\..\\..\\assets\\yolo26n.onnx"; //relative file path from the project executable. Need to be adjusted when publishing TODO
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

            _trackingSession = new BYTETracker(Postprocessing.ObjToSTrack, 14, 30, (float)0.4, (float)0.5, (float)0.7);

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
            if(_currModel != null)
            {
                _currModel.Dispose();
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

        private void grabFrame()
        {
            while (_alive == true)
            {
                Thread.Sleep(50);
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
                Thread.Sleep(100);
                if (_running == true)
                {
                    if (_srcFrame == null || _srcFrame.Empty() || _captures == null || !_captures.IsOpened() ||  !pictureBox1.IsHandleCreated) { continue; }

                    //capture the frame, process it within the InferenceSession, display the output
                    ///////////////////////////preprocessing ////////////////////////////////
                    _processedFrame = _srcFrame;
                    if(Preprocessing.ValidateImgDim(_processedFrame) == false)
                    {
                        ROICrop = Preprocessing.GetRectOfOriginalFrame(_processedFrame);
                        origWidth = _processedFrame.Width;
                        origHeight = _processedFrame.Height;
                        int aUWidth = Preprocessing.AlignUp(_processedFrame.Width)*32;
                        int aUHeight = Preprocessing.AlignUp(_processedFrame.Height)*32;
                        Preprocessing.performResize(_processedFrame, aUWidth, aUHeight);
                        Preprocessing.performPaddingVert(_processedFrame, aUHeight);
                        resized = true;
                    }
                    src = Preprocessing.prepareSrc(_processedFrame);
                    shape = Preprocessing.prepareShape(_processedFrame);

                    
                    IDisposableReadOnlyCollection<OrtValue> sampleOutput;
                    if(_currModel != null)
                    {
                        
                        sampleOutput = Postprocessing.infer(src, shape, _currModel); //infer here

                        ///////////////////////////Postprocessing ////////////////////////////////
                        ImmutableList<Detection> detections = Postprocessing.parseOutputData(sampleOutput);

                        //prepare detections for BYTETrack
                        if(_trackingSession != null)
                        {
                            List<ByteTrackCSharp.Object> BTObjs = new List<ByteTrackCSharp.Object>();
                            foreach(Detection val in detections)
                            {BTObjs.Add(Postprocessing.DetToByteTrackObject(val));} 


                            List<STrack> outputTracks = _trackingSession.update(BTObjs);
                            detections = Postprocessing.ListSTrackToDet(outputTracks);   
                        }

                        Postprocessing.plotDetections(detections,  _processedFrame);
                        if(resized == true)
                        {
                            //resize image back to original framesize to fit pictureBox
                            _processedFrame = new Mat(_processedFrame, ROICrop);
                            Preprocessing.performResize(_processedFrame, origWidth, origHeight);
                            resized = false;
                        }
                        sampleOutput.Dispose();
                    }

        

                    if (pictureBox1.InvokeRequired == true && !pictureBox1.IsDisposed) //required as per https://www.visioforge.com/help/docs/dotnet/general/code-samples/draw-video-picturebox/
                    {
                        //invoke marshals the frame swapping to the UI thread, ensuring thread safety when updating the PictureBox control
                        //we basically delegate the tasks within swapFrames to a UI thread instead of direct access via this worker thread.
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
                double fpsVal = frameCnt/elapsedSeconds;
                fpsDisplay = $"FPS: {fpsVal:F1}";            
            }
            Cv2.PutText(_processedFrame, fpsDisplay, new OpenCvSharp.Point(10, 20), HersheyFonts.HersheyComplexSmall, 1, Scalar.White);

            _displayFrame = BitmapConverter.ToBitmap(_processedFrame);
            pictureBox1.Image = _displayFrame;
            _oldFrame?.Dispose();
            frameCnt++;
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
