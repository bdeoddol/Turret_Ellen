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
using System.Diagnostics.Eventing.Reader;
using System.IO.Ports;
using System.Net;
using System.Numerics.Tensors;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.Marshalling;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Windows.Forms;




namespace Tracking
{
    public partial class Form1 : Form
    {
        //imgframe storage
        private VideoCapture? _captures;
        private Mat? _srcFrame;
        private Mat _processedFrame = new();
        private Bitmap? _displayFrame;
        private Bitmap? _oldFrame;

        //thread variables
        private bool _running;
        private bool _alive;
        // private  int UIFlag;
        private Thread? _streamThread;
        private Thread? _captureThread;

        //inferencing variables
        private bool _trackingMode;
        private BYTETracker? _trackingSession;
        private InferenceSession? _currModel;
        private string? _modelPath;
        private float[]? src;
        private long[]? shape;
        private bool resized = false;
        private readonly List<Detection> _detections = new();
        private readonly List<ByteTrackCSharp.Object> _BTObjs = new();

        //FPS variables
        private bool _fpsEnable = true;
        private Stopwatch _totalRuntime = new Stopwatch();
        private int _frameCnt = 0;
        private double _elapsedSeconds => _totalRuntime.Elapsed.Seconds;
        private double _fpsVal => _frameCnt / _elapsedSeconds;
        string? _fpsDisplay;

        //Serial Port Variables
        private bool _ardConnected;
        private SerialPort? _serialPort;
        private int[] _baudRates = { 9600, 19200, 38400, 57600, 115200 };
        private int _selectedBaud;
        private string? _selectedPort;

        //State Machine Variables
        private volatile TurrState _currState;
        private StateVar _stateVar = new();
        private Thread? _stateThread;
        private bool _stateOperate = false;
        
        //Remote Control Variables
        private bool _remoteControl;

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
            _streamThread?.IsBackground = true;
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
            ArduinoButtonDeactivated();
    
            _stateThread = new Thread(new ThreadStart(stateMachine));
            _stateOperate = true;
            _stateThread.Start();
        }

        private void Form1_Closed(object? sender, EventArgs e)
        {
            //once form is closed, release other resources
            if (_captures != null)
            {
                _captures.Release();
                _captures.Dispose();
            }
            if (_srcFrame != null)
            {
                _srcFrame.Dispose();
            }
            if (_currModel != null)
            {
                _currModel.Dispose();
            }
            if (_serialPort != null && _ardConnected == true)
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
            _stateOperate = false;
            if(_stateThread != null){_stateThread.Join(500);}
            if (_captureThread != null) {_captureThread?.Join(500);}
            if(_streamThread != null){_streamThread?.Join(500);}
        }

        //function to be called upon every camera connection
        private bool calibrateCamera()
        {
            if (_captures == null || _srcFrame == null) { return false; }
            _captures.Set(VideoCaptureProperties.FrameWidth, 960);
            _captures.Set(VideoCaptureProperties.FrameHeight, 540);
            _captures.Read(_srcFrame);
            Console.WriteLine("Connected Camera set to " + _srcFrame.Width + "x" + _srcFrame.Height + " resolution");

            if (_stateVar == null || _stateVar.cameraCalibration == null) { return false; }
            _stateVar.cameraCalibration.imgFrameH = _srcFrame.Height;
            _stateVar.cameraCalibration.imgFrameW = _srcFrame.Width;
            _stateVar.cameraCalibration.VertFOV = 33.836;
            _stateVar.cameraCalibration.HoriFOV = 56.068;


            return true;
        }




        ///////////////////////////Threads///////////////////////////
        /// 
        //Continutally captures and overwrites the frames from the webcam feed into a placeholder var called srcFrame
        private void grabFrame()
        {
            while (_alive == true)
            {
                Thread.Sleep(25);
                if (_running == true)
                {
                    if (_srcFrame == null || _captures == null || !_captures.IsOpened() || !pictureBox1.IsHandleCreated) { continue; }
                    _captures?.Read(_srcFrame); //decode the next frame from the video stream and store it in _srcframe


                }

            }
            return;
        }


        //The main streaming thread that calls the swap frame func. 
        // Grabs the latest captured frame overwritten into _srcframe and stores it into a var  called _processedframe
        //swaps the newly grabbed frame with the frame displayed in pictureBox1 
        //If allowed by _trackingMode, processedFrame may be modified and processed through inferencing before being swapped
        //  Also pastes the FPS counter onto the img before displaying it in picturebox1 if desired

        private void Stream()
        {
            _totalRuntime.Start();
            _frameCnt = 0;
            while (_alive == true)
            {
                Thread.Sleep(50);
                if (_running == true)
                {
                    if (_srcFrame == null || _srcFrame.Empty() || _captures == null || !_captures.IsOpened() || !pictureBox1.IsHandleCreated) { continue; }
                    _srcFrame.CopyTo(_processedFrame);

                    
                    if (_trackingMode == true)
                    { 
                        trackInFrame();
                        _stateVar.ActiveTargets = _detections.ToImmutableList(); //update the state variable
                    }
                    else
                    {
                        _stateVar.ActiveTargets = _stateVar.ActiveTargets.Clear();
                    }
                    if (pictureBox1.InvokeRequired == true && !pictureBox1.IsDisposed) //required as per https://www.visioforge.com/help/docs/dotnet/general/code-samples/draw-video-picturebox/
                    {
                        if (_fpsEnable == true)
                        {
                            if (_elapsedSeconds > 0)
                            {
                                // double fpsVal = _frameCnt / _elapsedSeconds;
                                _fpsDisplay = $"FPS: {_fpsVal:F1}";
                                Cv2.PutText(_processedFrame, _fpsDisplay, new OpenCvSharp.Point(10, 25), HersheyFonts.HersheySimplex, 1, Scalar.White, 2);

                            }
                        }
                        // marshals the frame swapping to the UI thread, ensuring thread safety when updating the PictureBox control
                        //we delegate the tasks within swapFrames to a UI thread instead of direct access via this worker thread.
                        pictureBox1.BeginInvoke(new Action(swapFrames)); //https://stackoverflow.com/questions/229554/whats-the-difference-between-invoke-and-begininvoke
                    }
                }

            }
            return;
        }

        // the primary function that performs the swap.
        private void swapFrames()
        {

            if (!_alive || _processedFrame == null || _processedFrame.Empty() || pictureBox1.IsDisposed) { return; } //bail out of the method if any of these are true.
            //swap the old frame with new one, display new frame, free memory of the old frame
            if (_displayFrame != null) { _oldFrame = _displayFrame; }
            _displayFrame = BitmapConverter.ToBitmap(_processedFrame);
            pictureBox1.Image = _displayFrame;
            _oldFrame?.Dispose();
            _frameCnt++;
            return;
        }

        //primary inferencing function
        //Takes the newly grabbed frame called _processedFrame performs preprocessing, inferencing, and postprocessing.
        //Draws the plotted detections onto the frame and prepares it to be ready and displayed witin Picturebox1
        private void trackInFrame()
        {

            _detections.Clear();
            _BTObjs.Clear();
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
                using IDisposableReadOnlyCollection<OrtValue> sampleOutput = Postprocessing.infer(src, shape, _currModel); //infer here
                ///////////////////////////Postprocessing ////////////////////////////////
                _detections.AddRange(Postprocessing.parseOutputData(sampleOutput));
                if (_trackingSession != null)
                {
                    //prepare detections for BYTETrack
                    foreach (Detection val in _detections)
                    { _BTObjs.Add(Postprocessing.DetToByteTrackObject(val)); }

                    //update BYTETrack and get new tracking results
                    List<STrack> outputTracks = _trackingSession.update(_BTObjs);
                    _detections.Clear();
                    _detections.AddRange(Postprocessing.ListSTrackToDet(outputTracks));
                }

                //draw our detections and resize image back to original size if necessary
                Postprocessing.plotDetections(_detections, _processedFrame);
                if (resized == true)
                {
                    //resize image back to original framesize to fit pictureBox
                    _processedFrame = new Mat(_processedFrame, ROICrop);
                    Preprocessing.performResize(_processedFrame, origWidth, origHeight);
                    resized = false;
                }
            }

            return;
        }

        private void stateMachine()
        {
            SerialCommand serialData;
            while (_stateOperate == true)
            {
                Thread.Sleep(100);
                //state swap
                if (_currState == TurrState.Inactive)
                {
                    Console.WriteLine("state : inactive");
                    if (_ardConnected == true) { _currState = TurrState.Idle; }
                    else { _currState = TurrState.Inactive; }
                }
                else if (_currState == TurrState.Idle)
                {
                    Console.WriteLine("state : idle");
                    if (_ardConnected == false) { _currState = TurrState.Inactive; }
                    else if (_remoteControl == true) { _currState = TurrState.Remote; }
                    else if (_stateVar.ActiveTargets.IsEmpty == false) { 
                        _stateVar.trackCycle = StateProcessing.RebuildTrackCycle(_stateVar.ActiveTargets.ToList()); //rebuild trackcycle
                        _stateVar.timer.Reset(); 
                        _stateVar.timer.Start(); 
                        _currState = TurrState.Track;
                    }
                    //else { _currState = TurrState.Idle; }
                }
                else if (_currState == TurrState.Track)
                {
                    Console.WriteLine("state : track");
                    if (_ardConnected == false) { _currState = TurrState.Inactive; }
                    else if (_remoteControl == true) { _currState = TurrState.Remote; }
                    else if (_stateVar.targetLost == true) { _stateVar.timer.Reset(); _stateVar.timer.Start(); _currState = TurrState.Search; }
                    else if (_stateVar.ActiveTargets.IsEmpty == true) { _currState = TurrState.Idle; }
                    //else { _currState = TurrState.Track; }
                }
                else if (_currState == TurrState.Search)
                {
                    Console.WriteLine("state : search");
                    if (_ardConnected == false) { _currState = TurrState.Inactive; }
                    else if (_remoteControl == true) { _currState = TurrState.Remote; }
                    // if (timerExceed4seconds)
                    // {
                    //     if(targetLostPlaceholder == false ){_currState = TurrState.Track;}
                    //     else if(_stateVar.ActiveTargets.IsEmpty == false || _stateVar.targetLost == false){_currState = TurrState.Idle;}    
                    // }
                    //else if (_remoteControl == true) { _currState = TurrState.Remote; }
                    //else if (_ardConnected == false) { _currState = TurrState.Inactive; }

                }
                else if (_currState == TurrState.Remote)
                {
                    Console.WriteLine("state : remote");
                    if (_ardConnected == false) { _currState = TurrState.Inactive; }
                    else if (_remoteControl == true) { _currState = TurrState.Remote; }
                    //else if (_stateVar.ActiveTargets.IsEmpty == true) { _currState = TurrState.Idle; }
                    //else if (_stateVar.ActiveTargets.IsEmpty == false) { _currState = TurrState.Track; }


                }

                ///////////////////stateevents//////////////////////


                if(_currState == TurrState.Track)
                { 
                    if(_stateVar.timer.Elapsed.Seconds > 4)
                    {
                       Console.WriteLine("tracking subject expired, swapping");
                       _stateVar.cycleCurrIdx++;
                        _stateVar.timer.Reset();
                        _stateVar.timer.Start();
                    }
                    if((_stateVar.cycleCurrIdx + 1) > _stateVar.trackCycle.Count)
                    {
                        Console.WriteLine("trackcycle exhausted");
                        _stateVar.cycleCurrIdx = 0;
                        _stateVar.trackCycle = StateProcessing.RebuildTrackCycle(_stateVar.ActiveTargets.ToList()); //rebuild trackcycle
                        _stateVar.timer.Reset();
                        _stateVar.timer.Start();
                    }
                    else if(_stateVar.trackCycle.Count == 0)
                    {
                        Console.WriteLine("empty trackcycle, rebuilding");
                        _stateVar.cycleCurrIdx = 0;
                        _stateVar.trackCycle = StateProcessing.RebuildTrackCycle(_stateVar.ActiveTargets.ToList()); //rebuild trackcycle
                        _stateVar.timer.Reset();
                        _stateVar.timer.Start();
                    }

                    else if(_stateVar.ActiveTargets.Exists(x => x.detID == _stateVar.currDetId) == false)
                    {
                        Console.WriteLine("target lost");
                        _stateVar.targetLost = true;
                        _stateVar.trackCycle = StateProcessing.RebuildTrackCycle(_stateVar.ActiveTargets.ToList()); //rebuild trackcycle
                    }
                }
                // else if(_currState == TurrState.Search)
                // {
                //     if(_stateVar.ActiveTargets.Exists(x => x.detID == _stateVar.currDetId) == true)
                //     {
                //         _currState = TurrState.Track;   
                //     }
                //     else if(_stateVar.timer.Elapsed.Seconds > 4)
                //     {

                //         _currState = TurrState.Track;
                //     }
                // }

            }


        }





    }
}
