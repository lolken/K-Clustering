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
            this.SuspendLayout();
            // 
            // startBut
            // 
            this.startBut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.startBut.Location = new System.Drawing.Point(32, 232);
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
            this.button1.Location = new System.Drawing.Point(32, 261);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "Reset";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // clusterCount
            // 
            this.clusterCount.Location = new System.Drawing.Point(19, 34);
            this.clusterCount.Name = "clusterCount";
            this.clusterCount.Size = new System.Drawing.Size(100, 20);
            this.clusterCount.TabIndex = 4;
            this.clusterCount.Text = "100";
            this.clusterCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // clusterRadius
            // 
            this.clusterRadius.Location = new System.Drawing.Point(19, 129);
            this.clusterRadius.Name = "clusterRadius";
            this.clusterRadius.Size = new System.Drawing.Size(100, 20);
            this.clusterRadius.TabIndex = 5;
            this.clusterRadius.Text = "100";
            this.clusterRadius.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // parentCount
            // 
            this.parentCount.Location = new System.Drawing.Point(19, 80);
            this.parentCount.Name = "parentCount";
            this.parentCount.Size = new System.Drawing.Size(100, 20);
            this.parentCount.TabIndex = 6;
            this.parentCount.Text = "50";
            this.parentCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // satcount
            // 
            this.satcount.Location = new System.Drawing.Point(20, 181);
            this.satcount.Name = "satcount";
            this.satcount.Size = new System.Drawing.Size(100, 20);
            this.satcount.TabIndex = 7;
            this.satcount.Text = "30";
            this.satcount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Cluster Count";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Parent Count";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(17, 116);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "Cluster Radius";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 165);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "Count Per Cluster";
            // 
            // ClusterController
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(138, 296);
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
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ClusterController";
            this.Text = "ClusterController";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ClusterController_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox clusterCount;
        private System.Windows.Forms.TextBox clusterRadius;
        private System.Windows.Forms.TextBox parentCount;
        private System.Windows.Forms.TextBox satcount;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        public System.Windows.Forms.Button startBut;
    }
}