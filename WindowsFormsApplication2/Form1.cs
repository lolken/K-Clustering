using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

namespace WindowsFormsApplication2
{
    public partial class Form1 : Form
    {
        #region Control Variables

        public int ClusterRadius = 100;
        public int parentCount = 50;
        public int interval = (int)(1000.0 / 60.0);
        public int clusterCount = 300;
        public int satCountOriginal = 50;
        public int Scale = 1;
        double OriginalArea = -1;

        //Dictionary<int, Color> ClusterColors = new Dictionary<int, Color>();

        #endregion Control Variables

        #region Members
        List<Kluster> parents = new List<Kluster>();
        List<Kluster> parentsPrev = new List<Kluster>();
        List<List<Point>> sats = new List<List<Point>>();
        Random rand = new Random();
        Timer FrameRater = new Timer();
        #endregion Members
        Icon icon;
        public Form1()
        {
            InitializeComponent();

            t.Interval = interval;

            t.Tick += new EventHandler(t_Tick);

            FrameRater.Tick += FrameRater_Tick;
            FrameRater.Interval = 1000;
            this.DoubleBuffered = true;
            OriginalArea = this.Size.Width * this.Size.Height;
            //makeboard();

            Bitmap bmp = new Bitmap(kMeans.Properties.Resources.target);
            icon = Icon.FromHandle(bmp.GetHicon());
            cc = new ClusterController(this);
            cc.Show(this);
            
        }

        void FrameRater_Tick(object sender, EventArgs e)
        {
            cc.fpslabel.Text = frames.ToString();
            frames = 0;
        }

        ClusterController cc;
        int frames = 0;

        public void updateClock(int scale)
        {
            double val = 1;
            switch (scale)
            {
                case 1:
                    val = 15;
                    break;
                case 2:
                    val = 2;
                    break;
                case 3:
                    val = 15;
                    break;

            }
            bool old = t.Enabled;

            if (old)
                t.Stop();
            t.Interval = (int)(1000.0 / val);
            if (old)
                t.Start();
        }
        void t_Tick(object sender, EventArgs e)
        {
            frames += 1;
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
                for (int j = 0; j < parents.Count; j++)
                {
                    distance = getDistance(parents[j].Location, tempsat[i]);
                    if (distance < min)
                    {
                        min = distance;
                        index = j;
                    }
                }
                sats[index].Add(new Point(tempsat[i].X, tempsat[i].Y));
                //this.Refresh();
            }

            for (int i = 0; i < parents.Count; i++)
            {
                Point p = getAvgPoint(sats[i]);
                if (p.X != -1000 && p.Y != -1000)
                    if (i != parentIndex)
                        parents[i].Location = p;
            }



            bool same = true;
            if (parentsPrev.Count > 0)
            {
                for (int i = 0; i < parents.Count; i++)
                    same &= parents[i].Location == parentsPrev[i].Location;

            }
            else
                same = false;

            if (same)
            {
                //t.Enabled = false;
                cc.startBut.Text = t.Enabled ? "Stop" : "Start";
                Converged = true;
            }
            else
                Converged = false;

            parentsPrev = new List<Kluster>(parents);

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
            parentsPrev.Clear();
            ColorsExample.Clear();
            for (int i = 0; i < clusterCount; i++)
                sats.Add(new List<Point>());

            Size size = this.Size;
            
            Point random = new Point(-1000,-1000);
            for (int i = 0; i < parentCount; i++)
            {
                random = new Point(-1000, -1000);
                while (random.X > size.Width - 10 || random.X < 0 || random.Y > size.Height - 10 || random.Y < 0)
                    random = new Point((int)(GetRandomPercentage() * Size.Width), (int)(GetRandomPercentage() * Size.Height));
                parents.Add(new Kluster(random,String.Format("Kluster{0}",i)));
            }

            random = new Point(-1000,-1000);
            Point RandomClusterPoint = new Point(-1000,-1000);
            double density = Math.Sqrt(((double)(size.Width * size.Height)/ OriginalArea));
            density *= satCountOriginal;
            int satCount = (int)density;
            cc.satcount.Text = satCount.ToString();
            //ClusterRadius = (size.Width * size.Height) / 9000;
            for (int i = 0; i < sats.Count; i++)
            {
                random = new Point(-1000, -1000);

                while (random.X > size.Width - 10 || random.X < 0 || random.Y > size.Height - 10 || random.Y < 0)
                    random = new Point((int)(GetRandomPercentage() * Size.Width), (int)(GetRandomPercentage() * Size.Height));
                for (int j = 0; j < satCount; j++)
                {
                    RandomClusterPoint = new Point(-1000, -1000);
                    while (RandomClusterPoint.X > size.Width - 10 || RandomClusterPoint.X < 0 || RandomClusterPoint.Y > size.Height - 10 || RandomClusterPoint.Y < 0)
                        RandomClusterPoint = new Point((int)(random.X + Math.Cos(2 * Math.PI * GetRandomPercentage()) * GetRandomPercentage() * ClusterRadius), (int)(random.Y + Math.Sin(2 * Math.PI * GetRandomPercentage()) * GetRandomPercentage() * ClusterRadius));
                    sats[i].Add(RandomClusterPoint);
                }
            }

            //clusterCount = parentCount;
        }

        //List<Color> colors = new List<Color>() { Color.Red, Color.Blue, Color.Green, Color.Black, Color.Pink };

        double GetRandomPercentage()
        {
            return rand.Next(0, 100000) / 100000.0;
        }

        List<Color> ColorsExample = new List<Color>();

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            System.Drawing.Graphics graphicsObj;

            graphicsObj = e.Graphics;
            graphicsObj.Clear(Color.FromArgb(25, 25, 25));

            Pen myPen = new Pen(System.Drawing.Color.Green, 5);

            if (sats.Count > 0)
            {
                for (int i = 0; i < sats.Count; i++)
                {
                    if (ColorsExample.Count <= i)
                        ColorsExample.Add(Color.FromArgb((int)(GetRandomPercentage() * 255), (int)(GetRandomPercentage() * 255), (int)(GetRandomPercentage() * 255)));
                    myPen = new Pen(Color.FromArgb(255, ColorsExample[i]), 5);

                    if (sats[i].Count > 0)
                    {
                        foreach (Point pnt in sats[i])
                            graphicsObj.DrawEllipse(myPen, new Rectangle(pnt.X, pnt.Y, 2 * Scale, 2 * Scale));
                    }

                }
            }

            for (int i = 0; i < parentCount; i++)
            {
                if (ColorsExample.Count <= i)
                    ColorsExample.Add(Color.FromArgb((int)(GetRandomPercentage() * 255), (int)(GetRandomPercentage() * 255), (int)(GetRandomPercentage() * 255)));
                if (i != parentIndex)
                    graphicsObj.DrawIcon(icon, new Rectangle(parents[i].X - 25, parents[i].Y - 25, 50, 50));
                else
                    graphicsObj.DrawIcon(icon, new Rectangle(parents[i].X - 40, parents[i].Y - 40, 80, 80));
            }

            if (!Quality)
                graphicsObj.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;
            else
                graphicsObj.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
        }

        public void button1_Click(object sender, EventArgs e)
        {
            t.Enabled = false;
            Converged = false;
            cc.startBut.Text = t.Enabled ? "Stop" : "Start";
            makeboard();
            Refresh();
        }

        public Timer t = new Timer();

        public void startBut_Click(object sender, EventArgs e)
        {
            if (!t.Enabled)
            {
                if (Converged)
                    makeboard();
                FrameRater.Start();
                t.Start();
            }
            else
            {
                //FrameRater.Stop();
                //t.Stop();
            }
        }
        bool quality = true;
        public bool Quality { get { return quality; } set { quality = value; } }
        bool converged = false;
        public bool Converged { get { return converged; } set { converged = value; } }

        bool moused = false;
        Point ClusterParent;
        Point MoveTo = new Point(0, 0);
        Point Prev = new Point(0, 0);
        int parentIndex = -1;
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (!t.Enabled)
                return;
            //Graphics graphObj = this.CreateGraphics();
            MoveTo = new Point(e.X, e.Y);

            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                int min = int.MaxValue;
                double distance = 0;
                parents.ForEach(pnt =>
                {
                    distance = getDistance(pnt.Location, MoveTo);
                    if (distance < min)
                    {
                        min = (int)distance;
                        ClusterParent = new Point(pnt.X, pnt.Y);
                    }
                });

                parentIndex = parents.FindIndex(pnt => { return pnt.Location == ClusterParent; });

                if (ModifierKeys == Keys.Control)
                {
                    if (min < 50)
                    {
                        parents.RemoveAt(parentIndex);
                        parentsPrev.RemoveAt(parentIndex);
                        parentCount -= 1;
                    }
                    else
                    {
                        parents.Add(new Kluster(new Point(MoveTo.X, MoveTo.Y), String.Format("Kluster{0}", parentIndex)));
                        parentsPrev.Add(new Kluster(new Point(MoveTo.X, MoveTo.Y), String.Format("Kluster{0}", parentIndex)));
                        parentCount += 1;
                    }
                    cc.parentCount.Text = parentCount.ToString();
                }
                else
                {
                    if (min < 400)
                    {
                        moused = true;
                        Prev = MoveTo;
                    }
                }
            }
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (moused)
            {
                MoveTo = new Point(e.X, e.Y);
                for (int i = 0; i < parents.Count; i++)
                {
                    if (parentIndex == i)
                    {
                        parents[i].Location = new Point(parents[i].X + (MoveTo.X - Prev.X), parents[i].Y + (MoveTo.Y - Prev.Y));
                        //parents[i] = ;
                        //parents[i] = new Point(MoveTo.X, MoveTo.Y);
                        break;
                    }
                }
                Prev = MoveTo;
                //this.Refresh();
            }
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            moused = false;
            parentIndex = -1;
            //Prev

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

    }

    public class Kluster
    {
        string lbl = "";
        Point location = new Point(0, 0);
        //private Kluster pnt;
        public Kluster(Point pnt, string label)
        {
            location = new Point(pnt.X, pnt.Y);
            lbl = label;
        }

        public Kluster(Kluster pnt)
        {
            location = new Point(pnt.X, pnt.Y);
            ID = pnt.ID;
        }

        public string ID { get { return lbl; } set { lbl = value; } }
        public Point Location { get { return location; } set { location = new Point(value.X, value.Y); } }
        public int X { get { return location.X; } set { location.X = value; } }
        public int Y { get { return location.Y; } set { location.Y = value; } }
    }
    

}
