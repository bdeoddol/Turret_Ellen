using System;
using System.Windows.Forms;
using OpenCvSharp;
//ensure that 



namespace Portal2Turret
{
    public partial class SnapshotTest : Form
    {
        //via https://elbruno.com/2020/11/16/dotnet-display-the-%F0%9F%8E%A6-camera-feed-in-a-winform-using-opencv-and-net5/
        private VideoCapture _capture;
        private Mat _image;
        public SnapshotTest()
        {
            InitializeComponent();
            this.Load += Form1_Load; //load the form
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            _capture = new VideoCapture(0); //0 is the default camera index
            _image = new Mat(); //Mat is a matrix class in OpenCV that can hold images (container for pixels)
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _capture.Read(_image); //read the image of the curr frame from the camera
            if (_image.Empty()) return;
            pictureBox1.Image = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(_image); //convert the mat to a bit map and display in the pic box
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
