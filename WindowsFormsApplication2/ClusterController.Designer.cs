namespace WindowsFormsApplication2
{
    partial class ClusterController
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ClusterController));
            this.startBut = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.clusterCount = new System.Windows.Forms.TextBox();
            this.clusterRadius = new System.Windows.Forms.TextBox();
            this.parentCount = new System.Windows.Forms.TextBox();
            this.satcount = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.label5 = new System.Windows.Forms.Label();
            this.fpslabel = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.checkBox4 = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.SuspendLayout();
            // 
            // startBut
            // 
            this.startBut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.startBut.Location = new System.Drawing.Point(24, 391);
            this.startBut.Name = "startBut";
            this.startBut.Size = new System.Drawing.Size(75, 23);
            this.startBut.TabIndex = 2;
            this.startBut.Text = "Start";
            this.startBut.UseVisualStyleBackColor = true;
            this.startBut.Click += new System.EventHandler(this.startBut_Click);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.BackColor = System.Drawing.Color.Red;
            this.button1.Location = new System.Drawing.Point(24, 420);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "Reset";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // clusterCount
            // 
            this.clusterCount.Location = new System.Drawing.Point(8, 25);
            this.clusterCount.Name = "clusterCount";
            this.clusterCount.Size = new System.Drawing.Size(100, 20);
            this.clusterCount.TabIndex = 4;
            this.clusterCount.Text = "20";
            this.clusterCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // clusterRadius
            // 
            this.clusterRadius.Location = new System.Drawing.Point(8, 97);
            this.clusterRadius.Name = "clusterRadius";
            this.clusterRadius.Size = new System.Drawing.Size(100, 20);
            this.clusterRadius.TabIndex = 5;
            this.clusterRadius.Text = "55";
            this.clusterRadius.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // parentCount
            // 
            this.parentCount.Location = new System.Drawing.Point(8, 61);
            this.parentCount.Name = "parentCount";
            this.parentCount.Size = new System.Drawing.Size(100, 20);
            this.parentCount.TabIndex = 6;
            this.parentCount.Text = "2";
            this.parentCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // satcount
            // 
            this.satcount.Location = new System.Drawing.Point(8, 136);
            this.satcount.Name = "satcount";
            this.satcount.Size = new System.Drawing.Size(100, 20);
            this.satcount.TabIndex = 7;
            this.satcount.Text = "50";
            this.satcount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Cluster Count";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Parent Count";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(17, 84);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "Cluster Radius";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 120);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "Count Per Cluster";
            // 
            // trackBar1
            // 
            this.trackBar1.LargeChange = 1;
            this.trackBar1.Location = new System.Drawing.Point(8, 230);
            this.trackBar1.Maximum = 3;
            this.trackBar1.Minimum = 1;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(104, 45);
            this.trackBar1.TabIndex = 13;
            this.trackBar1.Value = 1;
            this.trackBar1.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(45, 214);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(34, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "Scale";
            // 
            // fpslabel
            // 
            this.fpslabel.AutoSize = true;
            this.fpslabel.Location = new System.Drawing.Point(37, 446);
            this.fpslabel.Name = "fpslabel";
            this.fpslabel.Size = new System.Drawing.Size(22, 13);
            this.fpslabel.TabIndex = 17;
            this.fpslabel.Text = "0.0";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(65, 446);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(21, 13);
            this.label7.TabIndex = 18;
            this.label7.Text = "fps";
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Checked = true;
            this.radioButton2.Enabled = false;
            this.radioButton2.Location = new System.Drawing.Point(24, 162);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(81, 17);
            this.radioButton2.TabIndex = 16;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "High Speed";
            this.radioButton2.UseVisualStyleBackColor = true;
            this.radioButton2.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Enabled = false;
            this.radioButton1.Location = new System.Drawing.Point(24, 185);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(82, 17);
            this.radioButton1.TabIndex = 15;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "High Quality";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(24, 281);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(58, 17);
            this.checkBox1.TabIndex = 19;
            this.checkBox1.Text = "Freeze";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(24, 300);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(54, 17);
            this.checkBox2.TabIndex = 20;
            this.checkBox2.Text = "Demo";
            this.checkBox2.UseVisualStyleBackColor = true;
            this.checkBox2.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
            // 
            // checkBox3
            // 
            this.checkBox3.AutoSize = true;
            this.checkBox3.Location = new System.Drawing.Point(24, 323);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(75, 17);
            this.checkBox3.TabIndex = 21;
            this.checkBox3.Text = "Show Grid";
            this.checkBox3.UseVisualStyleBackColor = true;
            this.checkBox3.CheckedChanged += new System.EventHandler(this.checkBox3_CheckedChanged);
            // 
            // checkBox4
            // 
            this.checkBox4.AutoSize = true;
            this.checkBox4.Location = new System.Drawing.Point(24, 346);
            this.checkBox4.Name = "checkBox4";
            this.checkBox4.Size = new System.Drawing.Size(92, 17);
            this.checkBox4.TabIndex = 22;
            this.checkBox4.Text = "Show Centers";
            this.checkBox4.UseVisualStyleBackColor = true;
            this.checkBox4.CheckedChanged += new System.EventHandler(this.checkBox4_CheckedChanged);
            // 
            // ClusterController
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(128, 481);
            this.Controls.Add(this.checkBox4);
            this.Controls.Add(this.checkBox3);
            this.Controls.Add(this.checkBox2);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.fpslabel);
            this.Controls.Add(this.radioButton2);
            this.Controls.Add(this.radioButton1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.satcount);
            this.Controls.Add(this.parentCount);
            this.Controls.Add(this.clusterRadius);
            this.Controls.Add(this.clusterCount);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.startBut);
            this.Controls.Add(this.trackBar1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ClusterController";
            this.Text = "ClusterController";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ClusterController_FormClosing);
            this.Load += new System.EventHandler(this.ClusterController_Load);
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox clusterCount;
        private System.Windows.Forms.TextBox clusterRadius;
        public System.Windows.Forms.TextBox parentCount;
        public System.Windows.Forms.TextBox satcount;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        public System.Windows.Forms.Button startBut;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.Label label5;
        public System.Windows.Forms.Label fpslabel;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.CheckBox checkBox3;
        private System.Windows.Forms.CheckBox checkBox4;
    }
}