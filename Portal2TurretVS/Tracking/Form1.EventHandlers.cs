using ByteTrackCSharp;
using OpenCvSharp;
using System;
using System.Collections.Generic;
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

            _selectedPort = PortDropDown.SelectedText;
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
            _trackingSession = new BYTETracker(Postprocessing.ObjToSTrack, (int)_fpsVal, (int)(_fpsVal * 10), (float)0.5, (float)0.5, (float)0.6);
            _alive = true;
            _captureThread.Start();
            _streamThread.Start();

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
        { deactivateTrackMode(); }

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
