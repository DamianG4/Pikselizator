namespace Pikselizator
{
    partial class Form1
    {
        /// <summary>
        /// Wymagana zmienna projektanta.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Wyczyść wszystkie używane zasoby.
        /// </summary>
        /// <param name="disposing">prawda, jeżeli zarządzane zasoby powinny zostać zlikwidowane; Fałsz w przeciwnym wypadku.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Kod generowany przez Projektanta formularzy systemu Windows

        /// <summary>
        /// Metoda wymagana do obsługi projektanta — nie należy modyfikować
        /// jej zawartości w edytorze kodu.
        /// </summary>
        private void InitializeComponent()
        {
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rbAsm = new System.Windows.Forms.RadioButton();
            this.rbCSharp = new System.Windows.Forms.RadioButton();
            this.btnProcess = new System.Windows.Forms.Button();
            this.lblTime = new System.Windows.Forms.Label();
            this.trackBarThreads = new System.Windows.Forms.TrackBar();
            this.lblThreads = new System.Windows.Forms.Label();
            this.trackPowerSize = new System.Windows.Forms.TrackBar();
            this.lblPower = new System.Windows.Forms.Label();
            this.trackBarRadius = new System.Windows.Forms.TrackBar();
            this.lblRadius = new System.Windows.Forms.Label();
            this.btnLoad = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarThreads)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackPowerSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarRadius)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(32, 15);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(664, 459);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.btnProcess);
            this.groupBox1.Controls.Add(this.lblTime);
            this.groupBox1.Controls.Add(this.trackBarThreads);
            this.groupBox1.Controls.Add(this.lblThreads);
            this.groupBox1.Controls.Add(this.trackPowerSize);
            this.groupBox1.Controls.Add(this.lblPower);
            this.groupBox1.Controls.Add(this.trackBarRadius);
            this.groupBox1.Controls.Add(this.lblRadius);
            this.groupBox1.Controls.Add(this.btnLoad);
            this.groupBox1.Location = new System.Drawing.Point(783, 15);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(268, 459);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Panel";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rbAsm);
            this.groupBox2.Controls.Add(this.rbCSharp);
            this.groupBox2.Location = new System.Drawing.Point(8, 245);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox2.Size = new System.Drawing.Size(252, 82);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Wybor biblioteki";
            // 
            // rbAsm
            // 
            this.rbAsm.AutoSize = true;
            this.rbAsm.Location = new System.Drawing.Point(8, 52);
            this.rbAsm.Margin = new System.Windows.Forms.Padding(4);
            this.rbAsm.Name = "rbAsm";
            this.rbAsm.Size = new System.Drawing.Size(80, 20);
            this.rbAsm.TabIndex = 1;
            this.rbAsm.TabStop = true;
            this.rbAsm.Text = "ASM x64";
            this.rbAsm.UseVisualStyleBackColor = true;
            // 
            // rbCSharp
            // 
            this.rbCSharp.AutoSize = true;
            this.rbCSharp.Checked = true;
            this.rbCSharp.Location = new System.Drawing.Point(8, 23);
            this.rbCSharp.Margin = new System.Windows.Forms.Padding(4);
            this.rbCSharp.Name = "rbCSharp";
            this.rbCSharp.Size = new System.Drawing.Size(44, 20);
            this.rbCSharp.TabIndex = 0;
            this.rbCSharp.TabStop = true;
            this.rbCSharp.Text = "C#";
            this.rbCSharp.UseVisualStyleBackColor = true;
            // 
            // btnProcess
            // 
            this.btnProcess.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.btnProcess.Location = new System.Drawing.Point(31, 335);
            this.btnProcess.Margin = new System.Windows.Forms.Padding(4);
            this.btnProcess.Name = "btnProcess";
            this.btnProcess.Size = new System.Drawing.Size(208, 73);
            this.btnProcess.TabIndex = 9;
            this.btnProcess.Text = "PIKSELIZUJ";
            this.btnProcess.UseVisualStyleBackColor = true;
            this.btnProcess.Click += new System.EventHandler(this.btnProcess_Click);
            // 
            // lblTime
            // 
            this.lblTime.AutoSize = true;
            this.lblTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F);
            this.lblTime.Location = new System.Drawing.Point(26, 412);
            this.lblTime.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(128, 25);
            this.lblTime.TabIndex = 8;
            this.lblTime.Text = "Czas: --- ms";
            // 
            // trackBarThreads
            // 
            this.trackBarThreads.Location = new System.Drawing.Point(5, 202);
            this.trackBarThreads.Margin = new System.Windows.Forms.Padding(4);
            this.trackBarThreads.Maximum = 64;
            this.trackBarThreads.Minimum = 1;
            this.trackBarThreads.Name = "trackBarThreads";
            this.trackBarThreads.Size = new System.Drawing.Size(257, 56);
            this.trackBarThreads.TabIndex = 6;
            this.trackBarThreads.Value = 1;
            // 
            // lblThreads
            // 
            this.lblThreads.AutoSize = true;
            this.lblThreads.Location = new System.Drawing.Point(77, 175);
            this.lblThreads.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblThreads.Name = "lblThreads";
            this.lblThreads.Size = new System.Drawing.Size(106, 16);
            this.lblThreads.TabIndex = 5;
            this.lblThreads.Text = "Liczba wątków: 1";
            // 
            // trackPowerSize
            // 
            this.trackPowerSize.Location = new System.Drawing.Point(3, 135);
            this.trackPowerSize.Margin = new System.Windows.Forms.Padding(4);
            this.trackPowerSize.Maximum = 100;
            this.trackPowerSize.Minimum = 2;
            this.trackPowerSize.Name = "trackPowerSize";
            this.trackPowerSize.Size = new System.Drawing.Size(257, 56);
            this.trackPowerSize.TabIndex = 4;
            this.trackPowerSize.Value = 2;
            // 
            // lblPower
            // 
            this.lblPower.AutoSize = true;
            this.lblPower.Location = new System.Drawing.Point(107, 116);
            this.lblPower.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPower.Name = "lblPower";
            this.lblPower.Size = new System.Drawing.Size(46, 16);
            this.lblPower.TabIndex = 3;
            this.lblPower.Text = "Moc: 0";
            // 
            // trackBarRadius
            // 
            this.trackBarRadius.Location = new System.Drawing.Point(8, 76);
            this.trackBarRadius.Margin = new System.Windows.Forms.Padding(4);
            this.trackBarRadius.Maximum = 2000;
            this.trackBarRadius.Minimum = 1;
            this.trackBarRadius.Name = "trackBarRadius";
            this.trackBarRadius.Size = new System.Drawing.Size(257, 56);
            this.trackBarRadius.TabIndex = 2;
            this.trackBarRadius.Value = 1;
            // 
            // lblRadius
            // 
            this.lblRadius.AutoSize = true;
            this.lblRadius.Location = new System.Drawing.Point(83, 55);
            this.lblRadius.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblRadius.Name = "lblRadius";
            this.lblRadius.Size = new System.Drawing.Size(70, 16);
            this.lblRadius.TabIndex = 1;
            this.lblRadius.Text = "Promien: 0";
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(8, 23);
            this.btnLoad.Margin = new System.Windows.Forms.Padding(4);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(252, 28);
            this.btnLoad.TabIndex = 0;
            this.btnLoad.Text = "Wczytaj Obraz";
            this.btnLoad.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1067, 554);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.pictureBox1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarThreads)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackPowerSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarRadius)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Label lblRadius;
        private System.Windows.Forms.Label lblPower;
        private System.Windows.Forms.TrackBar trackBarRadius;
        private System.Windows.Forms.TrackBar trackBarThreads;
        private System.Windows.Forms.Label lblThreads;
        private System.Windows.Forms.TrackBar trackPowerSize;
        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.Button btnProcess;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rbCSharp;
        private System.Windows.Forms.RadioButton rbAsm;
    }
}