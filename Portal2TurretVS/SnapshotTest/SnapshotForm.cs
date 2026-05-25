using System;
using System.Windows.Forms;
using OpenCvSharp;
//ensure that 



namespace Portal2Turret
{
    public partial class SnapshotTest : Form
    {
        //via https://elbruno.com/2020/11/16/dotnet-display-the-%F0%9F%8E%A6-camera-feed-in-a-winform-using-opencv-and-net5/
        private VideoCapture _capture = new VideoCapture(0);
        private Mat _image = new Mat();

        public SnapshotTest()
        {
            InitializeComponent();
            this.Load += SnapshotTest_Load; //load the form
        }
        private void SnapshotTest_Load(object? sender, EventArgs? e) //lol apparently the question marks in c# syntax represent that this var COULD be null
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            _capture.Read(_image); //read the image of the curr frame from the camera
            if (_image.Empty()) { return; }
            pictureBox1.Image = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(_image); //convert the mat to a bit map and display in the pic box
            /* note that the frame of teh camera within the pictureBox 
             * will always display starting from the top leftmera corner,
             * similar to html when an image is too small for a container */
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void SnapshotTest_Load_1(object sender, EventArgs e)
        {

        }
    }
}
