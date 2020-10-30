namespace StereoCameraDemoApp
{
    partial class MainFormStCam
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.label_LCam = new System.Windows.Forms.Label();
            this.label_RCam = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.pictureBox_RCam = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox_LCam = new System.Windows.Forms.PictureBox();
            this.backgroundWorkerR = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorkerL = new System.ComponentModel.BackgroundWorker();
            this.button_SaveImage = new System.Windows.Forms.Button();
            this.button_StereoMatch = new System.Windows.Forms.Button();
            this.button_CalibCreate = new System.Windows.Forms.Button();
            this.button_PyStereoIMG = new System.Windows.Forms.Button();
            this.button_PyStereoMOV = new System.Windows.Forms.Button();
            this.button_stUnrealCV = new System.Windows.Forms.Button();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_RCam)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_LCam)).BeginInit();
            this.SuspendLayout();
            // 
            // label_LCam
            // 
            this.label_LCam.AutoSize = true;
            this.label_LCam.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label_LCam.Location = new System.Drawing.Point(608, -94);
            this.label_LCam.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_LCam.Name = "label_LCam";
            this.label_LCam.Size = new System.Drawing.Size(113, 24);
            this.label_LCam.TabIndex = 10;
            this.label_LCam.Text = "Camera(L)";
            // 
            // label_RCam
            // 
            this.label_RCam.AutoSize = true;
            this.label_RCam.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label_RCam.Location = new System.Drawing.Point(-499, -94);
            this.label_RCam.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_RCam.Name = "label_RCam";
            this.label_RCam.Size = new System.Drawing.Size(115, 24);
            this.label_RCam.TabIndex = 9;
            this.label_RCam.Text = "Camera(R)";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label1.Location = new System.Drawing.Point(1138, 54);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(113, 24);
            this.label1.TabIndex = 14;
            this.label1.Text = "Camera(L)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label2.Location = new System.Drawing.Point(31, 54);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(115, 24);
            this.label2.TabIndex = 13;
            this.label2.Text = "Camera(R)";
            // 
            // panel2
            // 
            this.panel2.AutoSize = true;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.pictureBox_RCam);
            this.panel2.Location = new System.Drawing.Point(26, 76);
            this.panel2.Margin = new System.Windows.Forms.Padding(4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(902, 706);
            this.panel2.TabIndex = 12;
            // 
            // pictureBox_RCam
            // 
            this.pictureBox_RCam.BackColor = System.Drawing.SystemColors.ControlDark;
            this.pictureBox_RCam.Location = new System.Drawing.Point(9, 16);
            this.pictureBox_RCam.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBox_RCam.Name = "pictureBox_RCam";
            this.pictureBox_RCam.Size = new System.Drawing.Size(800, 600);
            this.pictureBox_RCam.TabIndex = 2;
            this.pictureBox_RCam.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.AutoSize = true;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.pictureBox_LCam);
            this.panel1.Location = new System.Drawing.Point(1132, 76);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(902, 706);
            this.panel1.TabIndex = 11;
            // 
            // pictureBox_LCam
            // 
            this.pictureBox_LCam.BackColor = System.Drawing.SystemColors.ControlDark;
            this.pictureBox_LCam.Location = new System.Drawing.Point(9, 16);
            this.pictureBox_LCam.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBox_LCam.Name = "pictureBox_LCam";
            this.pictureBox_LCam.Size = new System.Drawing.Size(800, 600);
            this.pictureBox_LCam.TabIndex = 0;
            this.pictureBox_LCam.TabStop = false;
            // 
            // backgroundWorkerR
            // 
            this.backgroundWorkerR.WorkerReportsProgress = true;
            this.backgroundWorkerR.WorkerSupportsCancellation = true;
            this.backgroundWorkerR.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorkerR_DoWork);
            this.backgroundWorkerR.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorkerR_ProgressChanged);
            // 
            // backgroundWorkerL
            // 
            this.backgroundWorkerL.WorkerReportsProgress = true;
            this.backgroundWorkerL.WorkerSupportsCancellation = true;
            this.backgroundWorkerL.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorkerL_DoWork);
            this.backgroundWorkerL.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorkerL_ProgressChanged);
            // 
            // button_SaveImage
            // 
            this.button_SaveImage.Location = new System.Drawing.Point(15, 8);
            this.button_SaveImage.Margin = new System.Windows.Forms.Padding(4);
            this.button_SaveImage.Name = "button_SaveImage";
            this.button_SaveImage.Size = new System.Drawing.Size(115, 28);
            this.button_SaveImage.TabIndex = 15;
            this.button_SaveImage.Text = "SaveImg";
            this.button_SaveImage.UseVisualStyleBackColor = true;
            this.button_SaveImage.Click += new System.EventHandler(this.button_SaveImage_Click);
            // 
            // button_StereoMatch
            // 
            this.button_StereoMatch.Location = new System.Drawing.Point(388, 8);
            this.button_StereoMatch.Margin = new System.Windows.Forms.Padding(4);
            this.button_StereoMatch.Name = "button_StereoMatch";
            this.button_StereoMatch.Size = new System.Drawing.Size(138, 28);
            this.button_StereoMatch.TabIndex = 16;
            this.button_StereoMatch.Text = "C#Stereo";
            this.button_StereoMatch.UseVisualStyleBackColor = true;
            this.button_StereoMatch.Click += new System.EventHandler(this.button_StereoMatch_Click);
            // 
            // button_CalibCreate
            // 
            this.button_CalibCreate.Location = new System.Drawing.Point(195, 8);
            this.button_CalibCreate.Margin = new System.Windows.Forms.Padding(4);
            this.button_CalibCreate.Name = "button_CalibCreate";
            this.button_CalibCreate.Size = new System.Drawing.Size(138, 28);
            this.button_CalibCreate.TabIndex = 17;
            this.button_CalibCreate.Text = "CalibCreate";
            this.button_CalibCreate.UseVisualStyleBackColor = true;
            this.button_CalibCreate.Click += new System.EventHandler(this.button_CalibrateCreate_Click);
            // 
            // button_PyStereoIMG
            // 
            this.button_PyStereoIMG.Location = new System.Drawing.Point(579, 8);
            this.button_PyStereoIMG.Margin = new System.Windows.Forms.Padding(4);
            this.button_PyStereoIMG.Name = "button_PyStereoIMG";
            this.button_PyStereoIMG.Size = new System.Drawing.Size(138, 28);
            this.button_PyStereoIMG.TabIndex = 18;
            this.button_PyStereoIMG.Text = "PyStereoIMG";
            this.button_PyStereoIMG.UseVisualStyleBackColor = true;
            this.button_PyStereoIMG.Click += new System.EventHandler(this.button_PyStereo_Click);
            // 
            // button_PyStereoMOV
            // 
            this.button_PyStereoMOV.Location = new System.Drawing.Point(756, 8);
            this.button_PyStereoMOV.Margin = new System.Windows.Forms.Padding(4);
            this.button_PyStereoMOV.Name = "button_PyStereoMOV";
            this.button_PyStereoMOV.Size = new System.Drawing.Size(138, 28);
            this.button_PyStereoMOV.TabIndex = 19;
            this.button_PyStereoMOV.Text = "PyStereoMOV";
            this.button_PyStereoMOV.UseVisualStyleBackColor = true;
            this.button_PyStereoMOV.Click += new System.EventHandler(this.button_PyStereoMOV_Click);
            // 
            // button_stUnrealCV
            // 
            this.button_stUnrealCV.Location = new System.Drawing.Point(948, 8);
            this.button_stUnrealCV.Margin = new System.Windows.Forms.Padding(4);
            this.button_stUnrealCV.Name = "button_stUnrealCV";
            this.button_stUnrealCV.Size = new System.Drawing.Size(138, 28);
            this.button_stUnrealCV.TabIndex = 20;
            this.button_stUnrealCV.Text = "stUnrealCV";
            this.button_stUnrealCV.UseVisualStyleBackColor = true;
            this.button_stUnrealCV.Click += new System.EventHandler(this.button_stUnrealCV_Click);
            // 
            // MainFormStCam
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(2242, 930);
            this.Controls.Add(this.button_stUnrealCV);
            this.Controls.Add(this.button_PyStereoMOV);
            this.Controls.Add(this.button_PyStereoIMG);
            this.Controls.Add(this.button_CalibCreate);
            this.Controls.Add(this.button_StereoMatch);
            this.Controls.Add(this.button_SaveImage);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label_LCam);
            this.Controls.Add(this.label_RCam);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MainFormStCam";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "StereoCamera";
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_RCam)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_LCam)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label_LCam;
        private System.Windows.Forms.Label label_RCam;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.PictureBox pictureBox_RCam;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureBox_LCam;
        private System.ComponentModel.BackgroundWorker backgroundWorkerR;
        private System.ComponentModel.BackgroundWorker backgroundWorkerL;
        private System.Windows.Forms.Button button_SaveImage;
        private System.Windows.Forms.Button button_StereoMatch;
        private System.Windows.Forms.Button button_CalibCreate;
        private System.Windows.Forms.Button button_PyStereoIMG;
        private System.Windows.Forms.Button button_PyStereoMOV;
        private System.Windows.Forms.Button button_stUnrealCV;
    }
}

