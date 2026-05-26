namespace LiveTest
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            StartStream = new Button();
            pictureBox1 = new PictureBox();
            saveFileDialog1 = new SaveFileDialog();
            ConnectCamera = new Button();
            DisconnectCamera = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // StartStream
            // 
            StartStream.Location = new Point(12, 65);
            StartStream.Name = "StartStream";
            StartStream.Size = new Size(165, 46);
            StartStream.TabIndex = 0;
            StartStream.Text = "Start Stream";
            StartStream.UseVisualStyleBackColor = true;
            StartStream.Click += StartStream_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.Location = new Point(230, 12);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(558, 426);
            pictureBox1.TabIndex = 1;
            pictureBox1.TabStop = false;
            // 
            // ConnectCamera
            // 
            ConnectCamera.Location = new Point(12, 12);
            ConnectCamera.Name = "ConnectCamera";
            ConnectCamera.Size = new Size(79, 47);
            ConnectCamera.TabIndex = 2;
            ConnectCamera.Text = "Connect Camera";
            ConnectCamera.UseVisualStyleBackColor = true;
            ConnectCamera.Click += ConnectCamera_Click;
            // 
            // DisconnectCamera
            // 
            DisconnectCamera.Location = new Point(97, 12);
            DisconnectCamera.Name = "DisconnectCamera";
            DisconnectCamera.Size = new Size(79, 47);
            DisconnectCamera.TabIndex = 3;
            DisconnectCamera.Text = "Disconnect Camera";
            DisconnectCamera.UseVisualStyleBackColor = true;
            DisconnectCamera.Click += DisconnectCamera_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(DisconnectCamera);
            Controls.Add(ConnectCamera);
            Controls.Add(pictureBox1);
            Controls.Add(StartStream);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Button StartStream;
        private PictureBox pictureBox1;
        private SaveFileDialog saveFileDialog1;
        private Button ConnectCamera;
        private Button DisconnectCamera;
    }
}
