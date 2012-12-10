namespace kMeans
{
    partial class _3dForm
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
            this.viewportProfessional1 = new devDept.Eyeshot.ViewportProfessional();
            this.SuspendLayout();
            // 
            // viewportProfessional1
            // 
            this.viewportProfessional1.AmbientLight = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.viewportProfessional1.Background = new devDept.Eyeshot.Background(devDept.Eyeshot.backgroundStyleType.Gradient, System.Drawing.Color.Black, System.Drawing.Color.Black, null);
            this.viewportProfessional1.Cursor = System.Windows.Forms.Cursors.Default;
            this.viewportProfessional1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.viewportProfessional1.Grid = new devDept.Eyeshot.Grid(new devDept.Eyeshot.Point2D(-50D, -50D), new devDept.Eyeshot.Point2D(100D, 100D), 10D, System.Drawing.Color.Gray, System.Drawing.Color.Black, false, false);
            this.viewportProfessional1.Location = new System.Drawing.Point(0, 0);
            this.viewportProfessional1.Name = "viewportProfessional1";
            this.viewportProfessional1.OriginSymbol = new devDept.Eyeshot.OriginSymbol(20, devDept.Eyeshot.originSymbolStyleType.Ball, new System.Drawing.Font("Tahoma", 8.25F), System.Drawing.Color.Black, System.Drawing.Color.Red, System.Drawing.Color.Green, System.Drawing.Color.Blue, "X", "X", "Y", "Z", false, false);
            this.viewportProfessional1.ShowLabels = false;
            this.viewportProfessional1.Size = new System.Drawing.Size(284, 262);
            this.viewportProfessional1.TabIndex = 0;
            // 
            // _3dForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.viewportProfessional1);
            this.Name = "_3dForm";
            this.Text = "_3dForm";
            this.Load += new System.EventHandler(this._3dForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private devDept.Eyeshot.ViewportProfessional viewportProfessional1;
    }
}