using ByteTrackCSharp;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO.Ports;
using System.Text;

namespace Tracking
{
    public partial class Form1
    {
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
            if (BaudDropDown.SelectedItem != null) { _selectedBaud = (int)BaudDropDown.SelectedItem; }
        }

        private void ConnectArduino_Click(object sender, EventArgs e)
        { 
            bool success = SerialPortConnect();
            UpdateArduinoStatus(success);
            UpdateRemoteStatus(false);
            return;
        }

        private void DisconnectArduino_Click(object sender, EventArgs e)
        {
            _serialPort?.Close();
            UpdateArduinoStatus(false);
            UpdateRemoteStatus(false);
        }
        private void UpdateArduinoStatus(bool status)
        {
            _ardConnected = status;
            ConnectArduino.Enabled = !status;
            ConnectArduino.Visible = !status;
            DisconnectArduino.Enabled = status;
            DisconnectArduino.Visible = status;
        }
        private bool SerialPortConnect()
        {

            _selectedPort = PortDropDown.SelectedItem as string;
            if (BaudDropDown.SelectedItem is int){_selectedBaud = (int)BaudDropDown.SelectedItem;}
            try
            {
                _serialPort = new SerialPort(_selectedPort);
                _serialPort.BaudRate = _selectedBaud;
                _serialPort.Open();
                Console.WriteLine("port connected @ " + _selectedPort + "x" + _selectedBaud );
            }
            catch
            {
                MessageBox.Show("Failed connected to Arduino! Please check your settings and connection.");
                return false;
            }
            return true;
        }

        private void StartStream_Click(object sender, EventArgs e) //toggle
        {
            if (_running == true) { _totalRuntime.Stop(); _running = false; }
            else
            {
                frameDisplay.Show();
                frameDisplay.Enabled = true;
                _totalRuntime.Start();
                _running = true;
            }
            return;
        }

        private void ConnectCamera_Click(object sender, EventArgs e)
        {
            //initialize new resources
            _captures = new VideoCapture(0);
            _srcFrame = new Mat();
            if (_captures.IsOpened() == false)
            {
                MessageBox.Show("Could not connect to camera. Please connect a camera and try again.");
                return;
            }

            //calibrate our camera upon every new camera connect
            calibrateCamera();

            _captureThread = new Thread(new ThreadStart(grabFrame));
            _streamThread = new Thread(new ThreadStart(Stream));

            //after testing, aavg fps was  reported as 5
            //desired max time lost is decided to be 5 seconds, calculated as  maxtimelost(fps) = (fps/30)*track buffer
            //track buffer here is calculated to be 150
            _trackingSession = new BYTETracker(Postprocessing.ObjToSTrack, 5, 150, (float)0.6, (float)0.7, (float)0.65);
            _totalRuntime.Reset();
            _frameCnt = 0;

            _alive = true;
            _captureThread.Start();
            _streamThread.Start();

            SetDisconnectButtonActive();
            return;
        }

        private void DisconnectCamera_Click(object sender, EventArgs e)
        {
            _running = false;
            _alive = false;
            _stateVar.ActiveTargets = _stateVar.ActiveTargets.Clear();            
            if (_captureThread != null) {_captureThread?.Join(500);}
            if(_streamThread != null){_streamThread?.Join(500);}

            _captures?.Release();
            _captures?.Dispose();
            _captures = null;
            _srcFrame?.Release();
            _srcFrame?.Dispose();
            _srcFrame = null;


            frameDisplay.Hide();
            frameDisplay.Enabled = false;
            UpdateRemoteStatus(false);
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
        { UpdateTrackStatus(true); }

        private void DisableTrack_Click(object sender, EventArgs e)
        { 
            UpdateTrackStatus(false);
            _stateVar.ClearActiveDet();
        }

        private void UpdateTrackStatus(bool status)
        {
            _trackingMode = status;
            TrackEnable.Enabled = !status;
            TrackEnable.Visible = !status;
            DisableTrack.Enabled = status;
            DisableTrack.Visible = status;
        }

        private void frameDisplay_Resize(object sender, EventArgs e)
        {
            //upon the frameDisplay resize, we need to realign the remote field

            BeginInvoke(new Action(()=>LayoutRemoteField(0.08)));
            // Schedule the layout update on the UI thread, occurs asynchronously after the resize event is processed to prevent blocking and incomplete 
            // layout  calculations during the resize event. This ensures that the layout is updated correctly after the control (frameDisplay) has been resized.
        }

        private void LayoutRemoteField(double sizeFactor)
        {
            //resize and recenter the remoteField


            System.Drawing.Size imgRect = GetDisplayedImageSize(frameDisplay);
            if (imgRect == System.Drawing.Size.Empty) return;

            // we want the remoteField to be 0.80% the size of the displayed imge and centered within it
            remoteField.Width = (int)(imgRect.Width * sizeFactor);
            remoteField.Height = (int)(imgRect.Height * sizeFactor);

            remoteField.Left = frameDisplay.Left + ((frameDisplay.ClientSize.Width-imgRect.Width)/2) + ((imgRect.Width - remoteField.Width) / 2);
            remoteField.Top = frameDisplay.Top + ((frameDisplay.ClientSize.Height - imgRect.Height)/2) + ((imgRect.Height - remoteField.Height) / 2);
            //https://stackoverflow.com/questions/23659647/how-to-get-displayed-image-dimensions-of-an-image-scaled-to-fit-a-picturebox#comment36342004_23659815

        }


        // Source - https://stackoverflow.com/a/39847866
        // Posted by Robert Rodriguez
        // Retrieved 2026-07-21, License - CC BY-SA 3.0

        private System.Drawing.Size GetDisplayedImageSize(PictureBox pictureBox)
        {
            System.Drawing.Size containerSize = pictureBox.ClientSize;
            float containerAspectRatio = (float)containerSize.Height / (float)containerSize.Width;

            if(pictureBox.Image == null) { return System.Drawing.Size.Empty; }
            System.Drawing.Size originalImageSize = pictureBox.Image.Size;
            float imageAspectRatio = (float)originalImageSize.Height / (float)originalImageSize.Width;

            System.Drawing.Size result = new System.Drawing.Size();
            if (containerAspectRatio > imageAspectRatio)
            {
                result.Width = containerSize.Width;
                result.Height = (int)(imageAspectRatio * (float)containerSize.Width);
            }
            else
            {
                result.Height = containerSize.Height;
                result.Width = (int)((1.0f / imageAspectRatio) * (float)containerSize.Height);
            }
            return result;
        }

        private void RmDsblBut_Click(object sender, EventArgs e)
        {
            UpdateRemoteStatus(false);
        }

        private void RmNableBut_Click(object sender, EventArgs e)
        {
            UpdateRemoteStatus(true);
        }

        private void UpdateRemoteStatus(bool status)
        {
            if(_ardConnected == false)
            {
                _remoteControl = false;
                RmNableBut.Enabled = false;
                RmNableBut.Visible = false;
                RmDsblBut.Enabled = false;
                RmDsblBut.Visible = false;
                _remoteFieldEngaged = false;
                remoteField.Enabled = false;
                remoteField.Visible = false;
                return;
            }
            _remoteControl = status;
            RmNableBut.Enabled = !status;
            RmNableBut.Visible = !status;
            RmDsblBut.Enabled = status;
            RmDsblBut.Visible = status;
            _remoteFieldEngaged = false;
            remoteField.Enabled = false; //for debug
            remoteField.Visible = false; //for debug
            return;
        }

        private void frameDisplay_Click(object sender, EventArgs e)
        {
            // System.Drawing.Point remoteFieldCenter = new System.Drawing.Point(remoteField.Location.X + (remoteField.Width / 2), remoteField.Location.Y + (remoteField.Height / 2)); 
            // Cursor.Position = PointToScreen(remoteFieldCenter);


            _remoteFieldEngaged = true;
            //this point is derived relative to the remotefield | remoteField.PointToScreen(new System.Drawing.Point(remoteField.Width / 2, remoteField.Height / 2))
            Cursor.Position = _remoteFieldCenter;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (_remoteFieldEngaged == false) { return; }
            else if (_remoteFieldEngaged == true && e.KeyCode == Keys.Escape)
            {
                _remoteFieldEngaged = false;
                return;
            }
        }

        private void frameDisplay_MouseMove(object sender, MouseEventArgs e)
        {
            if (_remoteFieldEngaged == true)
            {

                if (Cursor.Position == _remoteFieldCenter)
                {
                    //everytime we recenter, the OS fires a MouseMove event, we can avoid these events by checking if the cursor is at the center position again, then we'll know to ignore it
                    return;
                }
                //otherwise, process
                OpenCvSharp.Point remoteFieldCenterOPCV = new OpenCvSharp.Point(_remoteFieldCenter.X, _remoteFieldCenter.Y);
                OpenCvSharp.Point cursorPosition = new OpenCvSharp.Point(Cursor.Position.X, Cursor.Position.Y);

                // Console.WriteLine("Remote Field Center: " + remoteFieldCenterOPCV.X + "x" + remoteFieldCenterOPCV.Y);
                // Console.WriteLine("Cursor Position: " + cursorPosition.X + "x" + cursorPosition.Y);

                //update serial command
                _stateVar.serialPayload = CameraProcessing.calcCursorTravel(_stateVar.cameraCalibration, remoteFieldCenterOPCV, cursorPosition);
                //recenter
                Cursor.Position = _remoteFieldCenter;
            }

        }


        /* Why do we need a remoteThread to update our cursor and etc when polling?
         * 
         * The _streamThread delegates the swapFrame function to the UI thread (main thread) which ensures the task is thread safe,
         * a separate worker thread on it's own should not update the picturebox via swapFrame because it is considered unsafe behavior.
         * We call frameDisplay.BeginInvoke(new Action(swapFrames); which delegates the task to the UI thread. This is called asynchronously in a queue, so the UI thread will process it
         * when it is able to.
         * This is important because if we were to call swapFrames directly from the _streamThread, it would cause a cross-thread operation exception.
         * BUT, if I enter the remotefield_click polling loop on the UI thread, the thread is blocked until _remoteFieldEngaged == false is 
         * to move on executing the the queued swapFrame function asynchronously
         * Moreover, to disengage without a seperate _remoteThread, the ESC key must be pressed and detected,
         * however because the UI thread is stuck in the polling loop 
         * so it can never detect the disengage in
         * Form1_KeyDown.

         */
        private void remoteField_Click(object sender, EventArgs e)
        {   //this point is derived relative to the client
            // System.Drawing.Point remoteFieldCenter = new System.Drawing.Point(remoteField.Location.X + (remoteField.Width / 2), remoteField.Location.Y + (remoteField.Height / 2)); 
            // Cursor.Position = PointToScreen(remoteFieldCenter);


            _remoteFieldEngaged = true;
            //this point is derived relative to the remotefield | remoteField.PointToScreen(new System.Drawing.Point(remoteField.Width / 2, remoteField.Height / 2))
            Cursor.Position = _remoteFieldCenter;
        }

        private void remoteField_MouseMove(object sender, MouseEventArgs e)
        {
            if (_remoteFieldEngaged == true)
            {

                if (Cursor.Position == _remoteFieldCenter)
                {
                    //everytime we recenter, the OS fires a MouseMove event, we can avoid these events by checking if the cursor is at the center position again, then we'll know to ignore it
                    return;
                }
                //otherwise, process
                OpenCvSharp.Point remoteFieldCenterOPCV = new OpenCvSharp.Point(_remoteFieldCenter.X, _remoteFieldCenter.Y);
                OpenCvSharp.Point cursorPosition = new OpenCvSharp.Point(Cursor.Position.X, Cursor.Position.Y);

                // Console.WriteLine("Remote Field Center: " + remoteFieldCenterOPCV.X + "x" + remoteFieldCenterOPCV.Y);
                // Console.WriteLine("Cursor Position: " + cursorPosition.X + "x" + cursorPosition.Y);

                //update serial command
                _stateVar.serialPayload = CameraProcessing.calcCursorTravel(_stateVar.cameraCalibration, remoteFieldCenterOPCV, cursorPosition);
                //recenter
                Cursor.Position = _remoteFieldCenter;
            }

        }


    }


}

