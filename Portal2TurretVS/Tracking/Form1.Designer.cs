namespace Tracking
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
            PortDropDown = new ComboBox();
            PortRefresh = new Button();
            BaudDropDown = new ComboBox();
            PortNameLabel = new Label();
            BaudeRateLabel = new Label();
            ConnectArduino = new Button();
            TrackEnable = new Button();
            DisableTrack = new Button();
            DisconnectArduino = new Button();
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
            pictureBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pictureBox1.BackColor = SystemColors.Control;
            pictureBox1.InitialImage = null;
            pictureBox1.Location = new Point(209, 12);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(851, 641);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
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
            // PortDropDown
            // 
            PortDropDown.FormattingEnabled = true;
            PortDropDown.Location = new Point(12, 486);
            PortDropDown.Name = "PortDropDown";
            PortDropDown.Size = new Size(121, 23);
            PortDropDown.TabIndex = 4;
            PortDropDown.Text = "COM Port";
            PortDropDown.SelectedIndexChanged += PortDropDown_SelectedIndexChanged;
            // 
            // PortRefresh
            // 
            PortRefresh.Location = new Point(12, 562);
            PortRefresh.Name = "PortRefresh";
            PortRefresh.Size = new Size(75, 23);
            PortRefresh.TabIndex = 5;
            PortRefresh.Text = "Refresh";
            PortRefresh.UseVisualStyleBackColor = true;
            PortRefresh.Click += PortRefresh_Click;
            // 
            // BaudDropDown
            // 
            BaudDropDown.FormattingEnabled = true;
            BaudDropDown.Location = new Point(12, 533);
            BaudDropDown.Name = "BaudDropDown";
            BaudDropDown.Size = new Size(121, 23);
            BaudDropDown.TabIndex = 6;
            BaudDropDown.Text = "Baud Rate";
            BaudDropDown.SelectedIndexChanged += BaudDropDown_SelectedIndexChanged;
            // 
            // PortNameLabel
            // 
            PortNameLabel.AutoSize = true;
            PortNameLabel.Location = new Point(12, 468);
            PortNameLabel.Name = "PortNameLabel";
            PortNameLabel.Size = new Size(64, 15);
            PortNameLabel.TabIndex = 7;
            PortNameLabel.Text = "Port Name";
            // 
            // BaudeRateLabel
            // 
            BaudeRateLabel.AutoSize = true;
            BaudeRateLabel.Location = new Point(12, 515);
            BaudeRateLabel.Name = "BaudeRateLabel";
            BaudeRateLabel.Size = new Size(66, 15);
            BaudeRateLabel.TabIndex = 8;
            BaudeRateLabel.Text = "Baude Rate";
            // 
            // ConnectArduino
            // 
            ConnectArduino.Location = new Point(12, 629);
            ConnectArduino.Name = "ConnectArduino";
            ConnectArduino.Size = new Size(121, 24);
            ConnectArduino.TabIndex = 9;
            ConnectArduino.Text = "Connect Arduino";
            ConnectArduino.UseVisualStyleBackColor = true;
            ConnectArduino.Click += ConnectArduino_Click;
            // 
            // TrackEnable
            // 
            TrackEnable.Location = new Point(12, 429);
            TrackEnable.Name = "TrackEnable";
            TrackEnable.Size = new Size(102, 23);
            TrackEnable.TabIndex = 10;
            TrackEnable.Text = "Enable Tracking";
            TrackEnable.UseVisualStyleBackColor = true;
            TrackEnable.Click += TrackEnable_Click;
            // 
            // DisableTrack
            // 
            DisableTrack.Location = new Point(12, 429);
            DisableTrack.Name = "DisableTrack";
            DisableTrack.Size = new Size(102, 23);
            DisableTrack.TabIndex = 11;
            DisableTrack.Text = "Disable Tracking";
            DisableTrack.UseVisualStyleBackColor = true;
            DisableTrack.Click += DisableTrack_Click;
            // 
            // DisconnectArduino
            // 
            DisconnectArduino.Location = new Point(12, 629);
            DisconnectArduino.Name = "DisconnectArduino";
            DisconnectArduino.Size = new Size(121, 23);
            DisconnectArduino.TabIndex = 12;
            DisconnectArduino.Text = "Disconnect Arduino";
            DisconnectArduino.UseVisualStyleBackColor = true;
            DisconnectArduino.Click += DisconnectArduino_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1072, 665);
            Controls.Add(DisconnectArduino);
            Controls.Add(TrackEnable);
            Controls.Add(ConnectArduino);
            Controls.Add(BaudeRateLabel);
            Controls.Add(PortNameLabel);
            Controls.Add(BaudDropDown);
            Controls.Add(PortRefresh);
            Controls.Add(PortDropDown);
            Controls.Add(DisconnectCamera);
            Controls.Add(ConnectCamera);
            Controls.Add(pictureBox1);
            Controls.Add(StartStream);
            Controls.Add(DisableTrack);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MinimumSize = new Size(904, 568);
            Name = "Form1";
            Text = "Form1";
            TopMost = true;
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button StartStream;
        private PictureBox pictureBox1;
        private SaveFileDialog saveFileDialog1;
        private Button ConnectCamera;
        private Button DisconnectCamera;
        private ComboBox PortDropDown;
        private Button PortRefresh;
        private ComboBox BaudDropDown;
        private Label PortNameLabel;
        private Label BaudeRateLabel;
        private Button ConnectArduino;
        private Button TrackEnable;
        private Button DisableTrack;
        private Button DisconnectArduino;
    }
}
