using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{
    public partial class ClusterController : Form
    {
        Form1 main;
        public ClusterController(Form1 mainForm)
        {
            main = mainForm;
            InitializeComponent();
            trackBar1_Scroll(this, new EventArgs());
        }

        private void ClusterController_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.FormOwnerClosing)
                main.Close();
        }

        private void startBut_Click(object sender, EventArgs e)
        {
            main.startBut_Click(sender, e);
            startBut.Text = main.t.Enabled ? "Stop" : "Start";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            main.clusterCount = Convert.ToInt32(clusterCount.Text);
            main.ClusterRadius = Convert.ToInt32(clusterRadius.Text);
            main.parentCount = Convert.ToInt32(parentCount.Text);
            main.satCount = Convert.ToInt32(satcount.Text);
            //main.satCountOriginal = Convert.ToInt32(satcount.Text);
            main.button1_Click(sender, e);
            startBut_Click(sender, e);
        }

        private void ClusterController_Load(object sender, EventArgs e)
        {
            button1_Click(sender, e);
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            main.Scale = trackBar1.Value;
            main.button1_Click(sender, e);
            startBut_Click(sender, e);
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            main.Quality = radioButton2.Checked;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            main.Frozen = checkBox1.Checked;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            main.DEMO_MODE = checkBox2.Checked;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            main.toggleGrid = checkBox3.Checked;
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            main.ShowCenters = checkBox4.Checked;
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            main.activateField = checkBox5.Checked;
            checkBox4.Checked = true;
        }
    }
}
