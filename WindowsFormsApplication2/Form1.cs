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
        public bool DEMO_MODE = false;

        #region Control Variables

        public int ClusterRadius = 100;
        public int parentCount = 50;
        public int interval = (int)(1000.0 / 60.0);
        public int clusterCount = 300;
        public int satCount = 50;
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

        public Timer t = new Timer();

        private bool freeze = false;

        public bool Frozen
        {
            get { return freeze; }
            set
            {
                parents.ForEach(k => { k.Frozen = value; });
                freeze = value;
            }
        }

        ClusterController cc;
        int frames = 0;
        Dictionary<int, Color> ColorLookup = new Dictionary<int, Color>();

        bool moused = false;
        Kluster ClusterParent;
        Point MoveTo = new Point(0, 0);
        Point Prev = new Point(0, 0);
        int parentGUID = -1;
        int eatenGUID = -1;

        Icon icon;
        Icon Dead;
        Icon target;

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

            Bitmap bmp = new Bitmap(kMeans.Properties.Resources.eye2);
            icon = Icon.FromHandle(bmp.GetHicon());
            bmp = new Bitmap(kMeans.Properties.Resources.eye1);
            Dead = Icon.FromHandle(bmp.GetHicon());
            bmp = new Bitmap(kMeans.Properties.Resources.target);
            target = Icon.FromHandle(bmp.GetHicon());

            cc = new ClusterController(this);
            cc.Show(this);
            satCountOriginal = satCount;

        }

        void FrameRater_Tick(object sender, EventArgs e)
        {
            cc.fpslabel.Text = frames.ToString();
            frames = 0;
        }

        List<Point> Negras = new List<Point>();

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

            Negras.Clear();

            List<Convex.Point> perimeterPoints = new List<Convex.Point>();
            List<Convex.Point> xys = new List<Convex.Point>();
            sats.ForEach(sat =>
                {
                    xys.Clear();
                    sat.ForEach(s => { xys.Add(new Convex.Point(s.X, s.Y)); });
                    perimeterPoints = Convex.Convexhull.convexhull(xys.ToArray()).ToList();
                    perimeterPoints.ForEach(pnt => { Negras.Add(new Point((int)pnt.x, (int)pnt.y)); });
                });

            for (int i = 0; i < parents.Count; i++)
            {
                Point p = getAvgPoint(sats[i]);
                if (p.X != -1000 && p.Y != -1000)
                    if (parents[i].ID != parentGUID)
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

        bool ParentsContainsGUID(int key)
        {
            Kluster k = parents.Find(p => { return p.ID == key; });
            return k != null;
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
            ColorLookup.Clear();
            //ColorsExample.Clear();
            for (int i = 0; i < clusterCount; i++)
                sats.Add(new List<Point>());

            Size size = this.Size;

            Point random = new Point(-1000, -1000);

            for (int i = 0; i < parentCount; i++)
            {
                random = new Point(-1000, -1000);

                while (random.X > size.Width - 10 || random.X < 0 || random.Y > size.Height - 10 || random.Y < 0)
                    random = new Point((int)(GetRandomPercentage() * Size.Width), (int)(GetRandomPercentage() * Size.Height));

                parents.Add(new Kluster(random, GetAvailableGUID()));
            }



            random = new Point(-1000, -1000);
            Point RandomClusterPoint = new Point(-1000, -1000);
            double density = Math.Sqrt(((double)(size.Width * size.Height) / OriginalArea));
            density *= satCountOriginal;
            satCount = (int)density;
            cc.satcount.Text = satCount.ToString();
            //ClusterRadius = (size.Width * size.Height) / 9000;
            if (DEMO_MODE)
            {
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
            }
            else
            {
                double rangeX = 1616.0;
                double rangeY = 916.0;

                double width = this.Width;
                double height = this.Height;

                //double DensityXFull = width / 10.0;
               // double DensityYFull = height / 10.0;

                double scaleScreenX = width / rangeX;
                double scaleScreenY = height / rangeY;

                double gridResolutionX = width / (10.0 - 5 * (1-scaleScreenX));
                double gridResolutionY = height / (10.0 - 5 * (1-scaleScreenY));
                double x = 0; double y = 0;

                double xstep = width / gridResolutionX;
                double ystep = height / gridResolutionY;
                for (double i = 0; i < gridResolutionX; i++)
                {
                    for (double j = 0; j < gridResolutionY; j++)
                    {
                        x = xstep * i;
                        y = ystep * j;
                        sats[0].Add(new Point((int)x, (int)y));

                    }
                }
            }
        }


        double GetRandomPercentage()
        {
            return rand.Next(0, 100000) / 100000.0;
        }


        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            System.Drawing.Graphics graphicsObj;

            graphicsObj = e.Graphics;
            graphicsObj.Clear(Color.FromArgb(25, 25, 25));

            Pen myPen = new Pen(System.Drawing.Color.Green, 5);
            Pen tmp = new Pen(Color.Black, 5);
            if (sats.Count > 0)
            {
                for (int i = 0; i < sats.Count; i++)
                {
                    if (i < parents.Count)
                    {
                        if (!ColorLookup.ContainsKey(parents[i].ID))
                            ColorLookup.Add(parents[i].ID, GetRandomColor());
                        myPen = new Pen(Color.FromArgb(255, ColorLookup[parents[i].ID]), 5);
                    }
                    else
                        myPen = new Pen(GetRandomColor(), 5);
                    
                    if (sats[i].Count > 0)
                    {
                        foreach (Point pnt in sats[i])
                        {
                            if(!Negras.Contains(pnt))
                            graphicsObj.DrawEllipse(myPen, new Rectangle(pnt.X, pnt.Y, 2 * Scale, 2 * Scale));
                            else
                                graphicsObj.DrawEllipse(tmp, new Rectangle(pnt.X, pnt.Y, 2 * Scale, 2 * Scale));
                        }
                    }

                }
            }
            bool show = true;
            if (show)
            {
                for (int i = 0; i < parentCount; i++)
                {
                    if (!ColorLookup.ContainsKey(parents[i].ID))
                        ColorLookup.Add(parents[i].ID, GetRandomColor());
                    if (parents[i].ID == parentGUID && !DEMO_MODE)
                        graphicsObj.DrawIcon(DEMO_MODE ? target : icon, new Rectangle(parents[i].X - 50, parents[i].Y - 50, 100, 100));
                    else if (parents[i].ID == eatenGUID && !parents[i].Marked && !DEMO_MODE)
                        graphicsObj.DrawIcon(DEMO_MODE ? target : icon, new Rectangle(parents[i].X - 40, parents[i].Y - 40, 80, 80));

                    if (parents[i].Marked)
                        graphicsObj.DrawIcon(DEMO_MODE ? target : Dead, new Rectangle(parents[i].X - 25, parents[i].Y - 25, 50, 50));
                    else
                        graphicsObj.DrawIcon(DEMO_MODE ? target : icon, new Rectangle(parents[i].X - 25, parents[i].Y - 25, 50, 50));

                }
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
        private bool keepPlaying;
        public bool Converged { get { return converged; } set { converged = value; } }

        int GetAvailableGUID()
        {

            int GUID = rand.Next(0, 100);
            while (ParentsContainsGUID(GUID))
                GUID = GetAvailableGUID();

            return GUID;
        }

        Color GetRandomColor() { return Color.FromArgb((int)(GetRandomPercentage() * 255), (int)(GetRandomPercentage() * 255), (int)(GetRandomPercentage() * 255)); }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (!t.Enabled)
                return;

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
                        ClusterParent = pnt;
                    }
                });
                ClusterParent.IsMaster = true;
                int parentindex = parents.FindIndex(pnt => { return pnt.Location == ClusterParent.Location; });
                parentGUID = parents[parentindex].ID;

                if (ModifierKeys == Keys.Control)
                {
                    if (min < 25)
                        RemoveParent(parentGUID);
                    else
                        AddParent();

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
        void RemoveParent(int GUID)
        {
            if (parents.Count > 1)
            {
                ColorLookup.Remove(GUID);
                parents.Remove(parents.Find(k => { return k.ID == GUID; }));
                parentsPrev.Remove(parentsPrev.Find(k => { return k.ID == GUID; }));
                parentCount -= 1;

            }
        }
        void AddParent()
        {
            parents.Add(new Kluster(new Point(MoveTo.X, MoveTo.Y), GetAvailableGUID(), Frozen));
            parentsPrev.Add(new Kluster(new Point(MoveTo.X, MoveTo.Y), GetAvailableGUID(), Frozen));
            parentCount += 1;
            if (!ColorLookup.ContainsKey(parents[parents.Count - 1].ID))
                ColorLookup.Add(parents[parents.Count - 1].ID, GetRandomColor());
            else
                ColorLookup[parents[parents.Count - 1].ID] = GetRandomColor();
        }
        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (moused)
            {
                MoveTo = new Point(e.X, e.Y);

                int parentind = parents.FindIndex(pnt => { return pnt.ID == parentGUID; });
                parents[parentind].Location = new Point(parents[parentind].X + (MoveTo.X - Prev.X), parents[parentind].Y + (MoveTo.Y - Prev.Y));

                if (!DEMO_MODE)
                {
                    int min = int.MaxValue;
                    double distance = 0;
                    int tmpindex = 0;
                    Point eatme = new Point(-1000, -1000);
                    for (int i = 0; i < parents.Count; i++)
                    {
                        if (parents[i].ID != parentGUID)
                        {
                            distance = getDistance(parents[i].Location, parents[parentind].Location);
                            if (distance < min)
                            {
                                min = (int)distance;
                                eatenGUID = parents[i].ID;
                                tmpindex = i;
                            }
                        }
                        else
                            parentind = i;
                    }

                    //you have to be 2x bigger
                    if (sats[tmpindex].Count > 1.5 * sats[parentind].Count)
                    {
                        if (min < 100)
                        {
                            //RemoveParent(eatenGUID);

                            Kluster found = parents.Find(k => { return k.ID == eatenGUID; });
                            if (found != null)
                            {
                                found.Marked = true;
                                List<Kluster> o = parents.FindAll(k => { return !k.Marked; });
                                if (o == null)
                                    keepPlaying = true;
                                else if (o.Count > 1)
                                    keepPlaying = true;
                                else
                                    keepPlaying = false;
                                if (!keepPlaying)
                                {
                                    AddParent();
                                    parents.ForEach(p => { p.Marked = false; });
                                    Form1_MouseUp(sender, e);
                                }
                            }
                        }
                    }
                }

                Prev = MoveTo;
            }
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            if (ClusterParent != null)
                ClusterParent.IsMaster = false;
            moused = false;
            parentGUID = -1;
            eatenGUID = -1;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

    }

    public class Kluster
    {
        int guid = -1;
        Point location = new Point(0, 0);
        bool marked = false;
        bool freeze = false;
        public bool Marked
        {
            get { return marked; }
            set { marked = value; }
        }
        //private Kluster pnt;
        public Kluster(Point pnt, int GUID, bool frozen)
        {
            Frozen = frozen;
            location = new Point(pnt.X, pnt.Y);
            ID = GUID;
        }

        public Kluster(Point pnt, int GUID)
        {
            Frozen = false;
            location = new Point(pnt.X, pnt.Y);
            ID = GUID;
        }

        public Kluster(Kluster pnt)
        {
            location = new Point(pnt.X, pnt.Y);
            ID = pnt.ID;
        }

        public int ID { get { return guid; } set { guid = value; } }

        public bool Frozen
        {
            get { return freeze; }
            set { freeze = value; }
        }
        bool master = false;
        public bool IsMaster
        {
            get { return master; }
            set { master = value; }
        }
        public Point Location
        {
            get { return location; }
            set
            {
                if (IsMaster)
                    location = new Point(value.X, value.Y);
                if (Frozen)
                    return;
                //if (!this.Marked)
                location = new Point(value.X, value.Y);

            }
        }

        List<Point> minions = new List<Point>();
        public List<Point> Minions
        {
            get { return minions; }
            set { minions = value; }
        }
        public int X { get { return location.X; } set { location.X = value; } }
        public int Y { get { return location.Y; } set { location.Y = value; } }
    }

    #region ConvexHull Function and Helpers
    namespace Convex
    {
        class Convexhull
        {
            public static Point[] convexhull(Point[] pts)
            {
                // Sort points lexicographically by increasing (x, y)
                int N = pts.Length;
                Polysort.Quicksort<Point>(pts);
                Point left = pts[0], right = pts[N - 1];
                // Partition into lower hull and upper hull
                CDLL<Point> lower = new CDLL<Point>(left), upper = new CDLL<Point>(left);
                for (int i = 0; i < N; i++)
                {
                    double det = Point.Area2(left, right, pts[i]);
                    if (det > 0)
                        upper = upper.Append(new CDLL<Point>(pts[i]));
                    else if (det < 0)
                        lower = lower.Prepend(new CDLL<Point>(pts[i]));
                }
                lower = lower.Prepend(new CDLL<Point>(right));
                upper = upper.Append(new CDLL<Point>(right)).Next;
                // Eliminate points not on the hull
                eliminate(lower);
                eliminate(upper);
                // Eliminate duplicate endpoints
                if (lower.Prev.val.Equals(upper.val))
                    lower.Prev.Delete();
                if (upper.Prev.val.Equals(lower.val))
                    upper.Prev.Delete();
                // Join the lower and upper hull
                Point[] res = new Point[lower.Size() + upper.Size()];
                lower.CopyInto(res, 0);
                upper.CopyInto(res, lower.Size());
                return res;
            }

            // Graham's scan
            private static void eliminate(CDLL<Point> start)
            {
                CDLL<Point> v = start, w = start.Prev;
                bool fwd = false;
                while (v.Next != start || !fwd)
                {
                    if (v.Next == w)
                        fwd = true;
                    if (Point.Area2(v.val, v.Next.val, v.Next.Next.val) < 0) // right turn
                        v = v.Next;
                    else
                    {                                       // left turn or straight
                        v.Next.Delete();
                        v = v.Prev;
                    }
                }
            }
        }
        class Point : Ordered<Point>
        {
            private static readonly Random rnd = new Random();

            public double x, y;

            public Point(double x, double y)
            {
                this.x = x; this.y = y;
            }

            public override string ToString()
            {
                return "(" + x + ", " + y + ")";
            }

            public static Point Random(int w, int h)
            {
                return new Point(rnd.Next(w), rnd.Next(h));
            }

            public bool Equals(Point p2)
            {
                return x == p2.x && y == p2.y;
            }

            public override bool Less(Ordered<Point> o2)
            {
                Point p2 = (Point)o2;
                return x < p2.x || x == p2.x && y < p2.y;
            }

            // Twice the signed area of the triangle (p0, p1, p2)
            public static double Area2(Point p0, Point p1, Point p2)
            {
                return p0.x * (p1.y - p2.y) + p1.x * (p2.y - p0.y) + p2.x * (p0.y - p1.y);
            }
        }

        // ------------------------------------------------------------

        // Circular doubly linked lists of T

        class CDLL<T>
        {
            private CDLL<T> prev, next;     // not null, except in deleted elements
            public T val;

            // A new CDLL node is a one-element circular list
            public CDLL(T val)
            {
                this.val = val; next = prev = this;
            }

            public CDLL<T> Prev
            {
                get { return prev; }
            }

            public CDLL<T> Next
            {
                get { return next; }
            }

            // Delete: adjust the remaining elements, make this one point nowhere
            public void Delete()
            {
                next.prev = prev; prev.next = next;
                next = prev = null;
            }

            public CDLL<T> Prepend(CDLL<T> elt)
            {
                elt.next = this; elt.prev = prev; prev.next = elt; prev = elt;
                return elt;
            }

            public CDLL<T> Append(CDLL<T> elt)
            {
                elt.prev = this; elt.next = next; next.prev = elt; next = elt;
                return elt;
            }

            public int Size()
            {
                int count = 0;
                CDLL<T> node = this;
                do
                {
                    count++;
                    node = node.next;
                } while (node != this);
                return count;
            }

            public void PrintFwd()
            {
                CDLL<T> node = this;
                do
                {
                    Console.WriteLine(node.val);
                    node = node.next;
                } while (node != this);
                Console.WriteLine();
            }

            public void CopyInto(T[] vals, int i)
            {
                CDLL<T> node = this;
                do
                {
                    vals[i++] = node.val;	// still, implicit checkcasts at runtime 
                    node = node.next;
                } while (node != this);
            }
        }

        // ------------------------------------------------------------

        class Polysort
        {
            private static void swap<T>(T[] arr, int s, int t)
            {
                T tmp = arr[s]; arr[s] = arr[t]; arr[t] = tmp;
            }

            // Typed OO-style quicksort a la Hoare/Wirth

            private static void qsort<T>(Ordered<T>[] arr, int a, int b)
            {
                // sort arr[a..b]
                if (a < b)
                {
                    int i = a, j = b;
                    Ordered<T> x = arr[(i + j) / 2];
                    do
                    {
                        while (arr[i].Less(x)) i++;
                        while (x.Less(arr[j])) j--;
                        if (i <= j)
                        {
                            swap<Ordered<T>>(arr, i, j);
                            i++; j--;
                        }
                    } while (i <= j);
                    qsort<T>(arr, a, j);
                    qsort<T>(arr, i, b);
                }
            }

            public static void Quicksort<T>(Ordered<T>[] arr)
            {
                qsort<T>(arr, 0, arr.Length - 1);
            }
        }

        public abstract class Ordered<T>
        {
            public abstract bool Less(Ordered<T> that);
        }

    #endregion
    }
}
