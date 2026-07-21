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
            SerialPortConnect();
            if(_ardConnected == true) { ArduinoButtonActivated(); }
            return;
        }
        private void DisconnectArduino_Click(object sender, EventArgs e)
        {
            _serialPort?.Close();
            _ardConnected = false;
            ArduinoButtonDeactivated();

        }
        private void ArduinoButtonActivated()
        {
            ConnectArduino.Enabled = false;
            ConnectArduino.Visible = false;
            DisconnectArduino.Enabled = true;
            DisconnectArduino.Visible = true;
        }
        private void ArduinoButtonDeactivated()
        {
            ConnectArduino.Enabled = true;
            ConnectArduino.Visible = true;
            DisconnectArduino.Enabled = false;
            DisconnectArduino.Visible = false;
        }
        private void SerialPortConnect()
        {

            _selectedPort = PortDropDown.SelectedItem as string;
            if (BaudDropDown.SelectedItem is int){_selectedBaud = (int)BaudDropDown.SelectedItem;}
            try
            {
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
            if (_running == true) { _totalRuntime.Stop(); _running = false; }
            else
            {
                pictureBox1.Show();
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

            //after testing, aavg fps was  reported as 7
            //desired max time lost is decided to be 5 seconds, calculated ast maxtimelost(fps) = (fps/30)*track buffer
            //track buffer here is calculated to be 150
            _trackingSession = new BYTETracker(Postprocessing.ObjToSTrack, 7, 150, (float)0.5, (float)0.5, (float)0.6);
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
        { activateTrackMode(); }

        private void activateTrackMode()
        {
            _trackingMode = true;
            TrackEnable.Enabled = false;
            TrackEnable.Visible = false;
            DisableTrack.Enabled = true;
            DisableTrack.Visible = true;
        }

        private void DisableTrack_Click(object sender, EventArgs e)
        { 
            deactivateTrackMode();

        }

        private void deactivateTrackMode()
        {
            _trackingMode = false;
            _stateVar.ActiveTargets.Clear();
            TrackEnable.Enabled = true;
            TrackEnable.Visible = true;
            DisableTrack.Enabled = false;
            DisableTrack.Visible = false;
        }




    }
}
