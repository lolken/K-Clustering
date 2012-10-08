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
    public partial class Form1 : Form
    {
        #region Members
        List<Point> parents = new List<Point>();
        List<Point> parentsPrev = new List<Point>();
        List<List<Point>> sats = new List<List<Point>>();
        Random rand = new Random();
        #region Control Variables

        public int ClusterRadius = 100;
        public int parentCount = 50;
        public int interval = (int)(1000.0 / 10.0);
        public int clusterCount = 300;
        public int satCount = 50;

        #endregion Control Variables

        #endregion Members

        public Form1()
        {
            InitializeComponent();

            t.Interval = interval;

            t.Tick += new EventHandler(t_Tick);

            makeboard();

            cc = new ClusterController(this);
            cc.Show();
        }
        ClusterController cc;
        void t_Tick(object sender, EventArgs e)
        {
            List<Point> tempsat = new List<Point>();

            for (int i = 0; i < sats.Count; i++)
                tempsat.AddRange(sats[i]);

            //int clusterCount = sats.Count;
            sats.Clear();
            for (int i = 0; i < parentCount; i++)
                sats.Add(new List<Point>());

            double min = double.MaxValue;
            int index = -1;
            double distance = 0;
            for (int i = 0; i < tempsat.Count; i++)
            {
                min = double.MaxValue;
                index = -1;
                for (int j = 0; j < parentCount; j++)
                {
                    distance = getDistance(parents[j], tempsat[i]);
                    if (distance < min)
                    {
                        min = distance;
                        index = j;
                    }
                }
                sats[index].Add(new Point(tempsat[i].X, tempsat[i].Y));
            }

            for (int i = 0; i < parentCount; i++)
            {
                Point p = getAvgPoint(sats[i]);
                if(p.X != -1000 && p.Y != -1000)
                    parents[i] = p;
            }
            

            bool same = true;
            if (parentsPrev.Count > 0)
            {
                for (int i = 0; i < parentCount; i++)
                    same &= parents[i] == parentsPrev[i];

            }
            else
                same = false;

            if (same)
            {
                t.Enabled = false;
                cc.startBut.Text = t.Enabled ? "Stop" : "Start";
            }
            else
                parentsPrev = new List<Point>(parents);

            Invalidate();
        }

        public Point getAvgPoint(List<Point> Points)
        {
            if (Points.Count == 0)
                return new Point(-1000, -1000);

            return new Point((int)Points.Average(pnt => pnt.X), (int)Points.Average(pnt => pnt.Y));
        }

        public double getDistance(Point parent, Point sat)
        {
            return Math.Sqrt(Math.Pow(sat.X - parent.X, 2) + Math.Pow(sat.Y - parent.Y, 2));
        }

        private void makeboard()
        {
            // assign parents some xy points
            // fill the satellites with xy points

            parents.Clear();
            sats.Clear();
            for (int i = 0; i < clusterCount; i++)
                sats.Add(new List<Point>());

            Size size = this.Size;
            for (int i = 0; i < parentCount; i++)
            {
                parents.Add(new Point((int)(GetRandomPercentage() * Size.Width), (int)(GetRandomPercentage() * Size.Height)));
            }

            Point randomPoint = new Point();
            //ClusterRadius = (size.Width * size.Height) / 9000;
            for (int i = 0; i < sats.Count; i++)
            {
                randomPoint = new Point((int)(GetRandomPercentage() * Size.Width), (int)(GetRandomPercentage() * Size.Height));
                for (int j = 0; j < satCount; j++)
                    sats[i].Add(new Point((int)(randomPoint.X + Math.Cos(2 * Math.PI * GetRandomPercentage()) * GetRandomPercentage() * ClusterRadius), (int)(randomPoint.Y + Math.Sin(2 * Math.PI * GetRandomPercentage()) * GetRandomPercentage() * ClusterRadius)));
            }

            //clusterCount = parentCount;
        }

        List<Color> colors = new List<Color>() { Color.Red, Color.Blue, Color.Green, Color.Black, Color.Pink };

        double GetRandomPercentage()
        {
            return rand.Next(0, 100000) / 100000.0;
        }

        List<Color> ColorsExample = new List<Color>();

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            System.Drawing.Graphics graphicsObj;

            graphicsObj = e.Graphics;

            graphicsObj.Clear(Color.Black);

            Pen myPen = new Pen(System.Drawing.Color.Green, 5);

            for (int i = 0; i < parentCount; i++)
            {
                if (ColorsExample.Count <= i)
                    ColorsExample.Add(Color.FromArgb((int)(GetRandomPercentage() * 255), (int)(GetRandomPercentage() * 255), (int)(GetRandomPercentage() * 255)));
                graphicsObj.DrawEllipse(new Pen(ColorsExample[i], 5), new Rectangle(parents[i].X - 10, parents[i].Y - 10, 20, 20));
            }

            if (sats.Count > 0)
            {
                for (int i = 0; i < sats.Count; i++)
                {
                    if (ColorsExample.Count <= i)
                        ColorsExample.Add(Color.FromArgb((int)(GetRandomPercentage() * 255), (int)(GetRandomPercentage() * 255), (int)(GetRandomPercentage() * 255)));
                    myPen = new Pen(ColorsExample[i], 5);

                    if (sats[i].Count > 0)
                    {
                        foreach (Point pnt in sats[i])
                            graphicsObj.DrawEllipse(myPen, new Rectangle(pnt.X, pnt.Y, 2, 2));
                    }

                }
            }
        }

        public void button1_Click(object sender, EventArgs e)
        {
            t.Enabled = false;
            cc.startBut.Text = t.Enabled ? "Stop" : "Start";
            makeboard();
            Refresh();
        }

        public Timer t = new Timer();

        public void startBut_Click(object sender, EventArgs e)
        {
            if (!t.Enabled)
                t.Start();
            else
                t.Stop();
        }
    }
}
