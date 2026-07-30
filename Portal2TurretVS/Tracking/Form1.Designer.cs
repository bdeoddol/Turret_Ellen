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
            frameDisplay = new PictureBox();
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
            RmNableBut = new Button();
            RmDsblBut = new Button();
            remoteField = new Panel();
            numericUpDown1 = new NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)frameDisplay).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).BeginInit();
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
            // frameDisplay
            // 
            frameDisplay.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            frameDisplay.BackColor = SystemColors.Control;
            frameDisplay.Cursor = Cursors.Cross;
            frameDisplay.InitialImage = null;
            frameDisplay.Location = new Point(209, 12);
            frameDisplay.Name = "frameDisplay";
            frameDisplay.Size = new Size(851, 641);
            frameDisplay.SizeMode = PictureBoxSizeMode.Zoom;
            frameDisplay.TabIndex = 1;
            frameDisplay.TabStop = false;
            frameDisplay.Click += frameDisplay_Click;
            frameDisplay.MouseMove += frameDisplay_MouseMove;
            frameDisplay.Resize += frameDisplay_Resize;
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
            PortDropDown.Location = new Point(12, 422);
            PortDropDown.Name = "PortDropDown";
            PortDropDown.Size = new Size(121, 23);
            PortDropDown.TabIndex = 4;
            PortDropDown.Text = "COM Port";
            PortDropDown.SelectedIndexChanged += PortDropDown_SelectedIndexChanged;
            // 
            // PortRefresh
            // 
            PortRefresh.Location = new Point(12, 498);
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
            BaudDropDown.Location = new Point(12, 469);
            BaudDropDown.Name = "BaudDropDown";
            BaudDropDown.Size = new Size(121, 23);
            BaudDropDown.TabIndex = 6;
            BaudDropDown.Text = "Baud Rate";
            BaudDropDown.SelectedIndexChanged += BaudDropDown_SelectedIndexChanged;
            // 
            // PortNameLabel
            // 
            PortNameLabel.AutoSize = true;
            PortNameLabel.Location = new Point(12, 404);
            PortNameLabel.Name = "PortNameLabel";
            PortNameLabel.Size = new Size(64, 15);
            PortNameLabel.TabIndex = 7;
            PortNameLabel.Text = "Port Name";
            // 
            // BaudeRateLabel
            // 
            BaudeRateLabel.AutoSize = true;
            BaudeRateLabel.Location = new Point(12, 451);
            BaudeRateLabel.Name = "BaudeRateLabel";
            BaudeRateLabel.Size = new Size(66, 15);
            BaudeRateLabel.TabIndex = 8;
            BaudeRateLabel.Text = "Baude Rate";
            // 
            // ConnectArduino
            // 
            ConnectArduino.Location = new Point(12, 527);
            ConnectArduino.Name = "ConnectArduino";
            ConnectArduino.Size = new Size(121, 24);
            ConnectArduino.TabIndex = 9;
            ConnectArduino.Text = "Connect Arduino";
            ConnectArduino.UseVisualStyleBackColor = true;
            ConnectArduino.Click += ConnectArduino_Click;
            // 
            // TrackEnable
            // 
            TrackEnable.Location = new Point(12, 594);
            TrackEnable.Name = "TrackEnable";
            TrackEnable.Size = new Size(121, 23);
            TrackEnable.TabIndex = 10;
            TrackEnable.Text = "Enable Tracking";
            TrackEnable.UseVisualStyleBackColor = true;
            TrackEnable.Click += TrackEnable_Click;
            // 
            // DisableTrack
            // 
            DisableTrack.Location = new Point(12, 594);
            DisableTrack.Name = "DisableTrack";
            DisableTrack.Size = new Size(121, 23);
            DisableTrack.TabIndex = 11;
            DisableTrack.Text = "Disable Tracking";
            DisableTrack.UseVisualStyleBackColor = true;
            DisableTrack.Click += DisableTrack_Click;
            // 
            // DisconnectArduino
            // 
            DisconnectArduino.Location = new Point(12, 527);
            DisconnectArduino.Name = "DisconnectArduino";
            DisconnectArduino.Size = new Size(121, 23);
            DisconnectArduino.TabIndex = 12;
            DisconnectArduino.Text = "Disconnect Arduino";
            DisconnectArduino.UseVisualStyleBackColor = true;
            DisconnectArduino.Click += DisconnectArduino_Click;
            // 
            // RmNableBut
            // 
            RmNableBut.Location = new Point(12, 623);
            RmNableBut.Name = "RmNableBut";
            RmNableBut.Size = new Size(121, 23);
            RmNableBut.TabIndex = 0;
            RmNableBut.Text = "Enable Remote";
            RmNableBut.UseVisualStyleBackColor = true;
            RmNableBut.Click += RmNableBut_Click;
            // 
            // RmDsblBut
            // 
            RmDsblBut.Location = new Point(12, 623);
            RmDsblBut.Name = "RmDsblBut";
            RmDsblBut.Size = new Size(121, 23);
            RmDsblBut.TabIndex = 14;
            RmDsblBut.Text = "Disable Remote";
            RmDsblBut.UseVisualStyleBackColor = true;
            RmDsblBut.Click += RmDsblBut_Click;
            // 
            // remoteField
            // 
            remoteField.Anchor = AnchorStyles.None;
            remoteField.BackColor = SystemColors.MenuHighlight;
            remoteField.Cursor = Cursors.Cross;
            remoteField.Location = new Point(294, 76);
            remoteField.Name = "remoteField";
            remoteField.Size = new Size(681, 513);
            remoteField.TabIndex = 13;
            remoteField.Click += remoteField_Click;
            remoteField.MouseMove += remoteField_MouseMove;
            // 
            // numericUpDown1
            // 
            numericUpDown1.Location = new Point(139, 623);
            numericUpDown1.Maximum = new decimal(new int[] { 5, 0, 0, 0 });
            numericUpDown1.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numericUpDown1.Name = "numericUpDown1";
            numericUpDown1.Size = new Size(37, 23);
            numericUpDown1.TabIndex = 15;
            numericUpDown1.Value = new decimal(new int[] { 1, 0, 0, 0 });
            numericUpDown1.ValueChanged += numericUpDown1_ValueChanged;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1072, 665);
            Controls.Add(numericUpDown1);
            Controls.Add(RmDsblBut);
            Controls.Add(RmNableBut);
            Controls.Add(remoteField);
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
            Controls.Add(StartStream);
            Controls.Add(DisableTrack);
            Controls.Add(frameDisplay);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            KeyPreview = true;
            MinimumSize = new Size(904, 568);
            Name = "Form1";
            Text = "Form1";
            TopMost = true;
            Load += Form1_Load;
            KeyDown += Form1_KeyDown;
            ((System.ComponentModel.ISupportInitialize)frameDisplay).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button StartStream;
        private PictureBox frameDisplay;
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
        private Button RmNableBut;
        private Button RmDsblBut;
        private Panel remoteField;
        private NumericUpDown numericUpDown1;
    }
}
