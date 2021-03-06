﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.LinearAlgebra.Generic.Factorization;
using MathNet.Numerics.LinearAlgebra.Generic;

namespace WindowsFormsApplication2
{
    public partial class MultRBF : Form
    {
        BackgroundWorker worker = new BackgroundWorker();

        public MultRBF(Form1 main)
        {
            Main = main;
            InitializeComponent();
            this.DoubleBuffered = true;
            initializeWorker();
            Bitmap bmp = new Bitmap(kMeans.Properties.Resources.target);
            target = Icon.FromHandle(bmp.GetHicon());
            surfaceForm.Show();
        }
        Form1 Main;
        void initializeWorker()
        {
            worker.WorkerReportsProgress = true;
            worker.WorkerSupportsCancellation = true;
            worker.DoWork += worker_DoWork;
            worker.ProgressChanged += worker_ProgressChanged;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
        }

        kMeans._3dForm surfaceForm = new kMeans._3dForm();
        List<List<ColoredPoint>> pntCollection = new List<List<ColoredPoint>>();
        double[] gridRes = new double[2];

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //coloredPoints = new List<ColoredPoint>(e.Result as List<ColoredPoint>);
            Invalidate();
            surfaceForm.UpdateSurfacePointMesh(pntCollection, 370 / 4);
        }

        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }
        public double gamma = 0.01;
        int PixelDensity = 2;

        public bool Type1 = false;
        //List<double> vals = new List<double>();
        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            object[] arguments = e.Argument as object[];
            List<List<Point>> inputs = new List<List<Point>>(arguments[0] as List<List<Point>>);
            List<Kluster> output = new List<Kluster>(arguments[1] as List<Kluster>);

            List<double[]> allpoints = new List<double[]>();
            List<double[]> ClusterCenters = new List<double[]>();
            List<double> clusterCounts = new List<double>(0);
            lock (inputs)
            {
                //int count = 0;
                inputs.ForEach(satList =>
                {
                    lock (satList)
                    {
                        satList.ForEach(sat =>
                        {
                            //if(count%5==0)
                            lock (allpoints)
                            {
                                allpoints.Add(new double[] { sat.X, sat.Y });
                            }
                            //count++;
                        });
                        clusterCounts.Add(satList.Count);
                    }
                });
            }
            lock (output)
            {
                output.ForEach(cluster =>
                {
                    ClusterCenters.Add(new double[] { cluster.X, cluster.Y });
                });
            }
            if (allpoints.Count == 0 || ClusterCenters.Count == 0)
                return;
            if (e.Cancel) 
                return;

            List<kMeans.RBFLite> rbfs = new List<kMeans.RBFLite>();

            if (!Type1)
                RBF = new kMeans.RBFLiteCluster(allpoints, ClusterCenters, gamma, clusterCounts, checkBox1.Checked);
            else
            {
                for (int i = 0; i < inputs.Count; i++)
                    rbfs.Add(new kMeans.RBFLite(inputs[i].ConvertAll(new Converter<Point, double[]>((pnt) => { return new double[] { pnt.X, pnt.Y }; })), new double[] { output[i].Location.X, output[i].Location.Y }, clusterCounts[i], gamma));
            }

            double width = this.Width;
            double height = this.Height;

            double scaleScreenX = 0.1;
            double scaleScreenY = 0.1;

            double gridResolutionX = width / PixelDensity;
            double gridResolutionY = height / PixelDensity;

            gridRes[0] = gridResolutionX;
            gridRes[1] = gridResolutionY;
            //double gridResolutionX = width / (1 * 10.0 - 4 * (1 - scaleScreenX));
            //double gridResolutionY = height / (1 * 10.0 - 4 * (1 - scaleScreenY));
            //double x = 0; double y = 0;

            double xstep = width / gridResolutionX;
            double ystep = height / gridResolutionY;
            List<double> heights = new List<double>();
            double X = 0;
            double Y = 0;
            double Val = 0;
            for (double i = 0; i < gridResolutionX; i++)
            {
                for (double j = 0; j < gridResolutionY; j++)
                {
                    if (e.Cancel)
                        return;
                    Val = 0;
                    X = xstep * i;
                    Y = ystep * j;
                    if(Type1)
                        rbfs.ForEach(rbf => Val += rbf.Eval(new double[] { X, Y }));
                    else
                        Val = RBF.Eval(new double[] { X, Y });
                    heights.Add(Val);
                }
            }

            double max, min;
            max = heights.Max();
            min = heights.Min();

            pntCollection.Clear();

            List<ColoredPoint> pnts1 = new List<ColoredPoint>();
            List<ColoredPoint> pnts2 = new List<ColoredPoint>();
            List<ColoredPoint> pnts3 = new List<ColoredPoint>();
            List<ColoredPoint> pnts4 = new List<ColoredPoint>();
            List<ColoredPoint> pnts5 = new List<ColoredPoint>();
            List<ColoredPoint> pnts6 = new List<ColoredPoint>();
            List<ColoredPoint> pnts7 = new List<ColoredPoint>();
            List<ColoredPoint> pnts8 = new List<ColoredPoint>();

            List<double> vals1 = new List<double>();
            List<double> vals2 = new List<double>();
            List<double> vals3 = new List<double>();
            List<double> vals4 = new List<double>();
            List<double> vals5 = new List<double>();
            List<double> vals6 = new List<double>();
            List<double> vals7 = new List<double>();
            List<double> vals8 = new List<double>();

            #region Parallel Reads


            Parallel.Invoke(
                () =>
                {
                    double x = 0; double y = 0; double val = 0;
                    for (double i = 0; i < gridResolutionX / 4; i++)
                    {
                        for (double j = 0; j < gridResolutionY / 2; j++)
                        {
                            val = 0;
                            x = xstep * i;
                            y = ystep * j;
                            if(Type1)
                                rbfs.ForEach(rbf => val += rbf.Eval(new double[] { x, y }));
                            else
                                val = RBF.Eval(new double[] { x, y });
                            if (!double.IsNaN(val) && !double.IsInfinity(val))
                                pnts1.Add(new ColoredPoint(new Point((int)x, (int)y), GetColor(max, min, val), val));
                            
                        }
                    }
                },
                () =>
                {
                    double x = 0; double y = 0; double val = 0;
                    for (double i = 0; i < gridResolutionX / 4; i++)
                    {
                        for (double j = gridResolutionY / 2; j < gridResolutionY; j++)
                        {
                            val = 0;
                            x = xstep * i;
                            y = ystep * j;
                            if (Type1)
                                rbfs.ForEach(rbf => val += rbf.Eval(new double[] { x, y }));
                            else
                                val = RBF.Eval(new double[] { x, y });
                            if (!double.IsNaN(val) && !double.IsInfinity(val))
                                pnts2.Add(new ColoredPoint(new Point((int)x, (int)y), GetColor(max, min, val), val));
                            
                        }
                    }
                },
                () =>
                {
                    double x = 0; double y = 0; double val = 0;
                    for (double i = gridResolutionX / 4; i < gridResolutionX / 2; i++)
                    {
                        for (double j = 0; j < gridResolutionY / 2; j++)
                        {
                            val = 0;
                            x = xstep * i;
                            y = ystep * j;
                            if (Type1)
                                rbfs.ForEach(rbf => val += rbf.Eval(new double[] { x, y }));
                            else
                                val = RBF.Eval(new double[] { x, y });
                            if (!double.IsNaN(val) && !double.IsInfinity(val))
                                pnts3.Add(new ColoredPoint(new Point((int)x, (int)y), GetColor(max, min, val), val));
                        }
                    }
                },
                () =>
                {
                    double x = 0; double y = 0; double val = 0;
                    for (double i = gridResolutionX / 4; i < gridResolutionX / 2; i++)
                    {
                        for (double j = gridResolutionY / 2; j < gridResolutionY; j++)
                        {
                            val = 0;
                            x = xstep * i;
                            y = ystep * j;
                            if (Type1)
                                rbfs.ForEach(rbf => val += rbf.Eval(new double[] { x, y }));
                            else
                                val = RBF.Eval(new double[] { x, y });
                            if (!double.IsNaN(val) && !double.IsInfinity(val))
                                pnts4.Add(new ColoredPoint(new Point((int)x, (int)y), GetColor(max, min, val), val));
                        }
                    }
                },
                () =>
                {
                    double x = 0; double y = 0; double val = 0;
                    for (double i = gridResolutionX / 2; i < 3 * gridResolutionX / 4; i++)
                    {
                        for (double j = 0; j < gridResolutionY / 2; j++)
                        {
                            val = 0;
                            x = xstep * i;
                            y = ystep * j;
                            if (Type1)
                                rbfs.ForEach(rbf => val += rbf.Eval(new double[] { x, y }));
                            else
                                val = RBF.Eval(new double[] { x, y });
                            if (!double.IsNaN(val) && !double.IsInfinity(val))
                                pnts5.Add(new ColoredPoint(new Point((int)x, (int)y), GetColor(max, min, val), val));             
                        }
                    }
                },
                () =>
                {
                    double x = 0; double y = 0; double val = 0;
                    for (double i = gridResolutionX / 2; i < 3 * gridResolutionX / 4; i++)
                    {
                        for (double j = gridResolutionY / 2; j < gridResolutionY; j++)
                        {
                            val = 0;
                            x = xstep * i;
                            y = ystep * j;
                            if (Type1)
                                rbfs.ForEach(rbf => val += rbf.Eval(new double[] { x, y }));
                            else
                                val = RBF.Eval(new double[] { x, y });
                            if (!double.IsNaN(val) && !double.IsInfinity(val))
                                pnts6.Add(new ColoredPoint(new Point((int)x, (int)y), GetColor(max, min, val), val));       
                        }
                    }
                },
                () =>
                {
                    double x = 0; double y = 0; double val = 0;
                    for (double i = 3 * gridResolutionX / 4; i < gridResolutionX; i++)
                    {
                        for (double j = 0; j < gridResolutionY / 2; j++)
                        {
                            val = 0;
                            x = xstep * i;
                            y = ystep * j;
                            if (Type1)
                                rbfs.ForEach(rbf => val += rbf.Eval(new double[] { x, y }));
                            else
                                val = RBF.Eval(new double[] { x, y });
                            if (!double.IsNaN(val) && !double.IsInfinity(val))
                                pnts7.Add(new ColoredPoint(new Point((int)x, (int)y), GetColor(max, min, val), val));
                        }
                    }
                },
                () =>
                {
                    double x = 0; double y = 0; double val = 0;
                    for (double i = 3 * gridResolutionX / 4; i < gridResolutionX; i++)
                    {
                        for (double j = gridResolutionY / 2; j < gridResolutionY; j++)
                        {
                            val = 0;
                            x = xstep * i;
                            y = ystep * j;
                            if (Type1)
                                rbfs.ForEach(rbf => val += rbf.Eval(new double[] { x, y }));
                            else
                                val = RBF.Eval(new double[] { x, y });
                            if (!double.IsNaN(val) && !double.IsInfinity(val))
                                pnts8.Add(new ColoredPoint(new Point((int)x, (int)y), GetColor(max, min, val), val));   
                        }
                    }
                }
                );

            pntCollection.Add(new List<ColoredPoint>(pnts1));
            pntCollection.Add(pnts2);
            pntCollection.Add(pnts3);
            pntCollection.Add(pnts4);
            pntCollection.Add(pnts5);
            pntCollection.Add(pnts6);
            pntCollection.Add(pnts7);
            pntCollection.Add(pnts8);

            coloredPoints.Clear();
            coloredPoints.AddRange(pnts1);
            coloredPoints.AddRange(pnts2);
            coloredPoints.AddRange(pnts3);
            coloredPoints.AddRange(pnts4);
            coloredPoints.AddRange(pnts5);
            coloredPoints.AddRange(pnts6);
            coloredPoints.AddRange(pnts7);
            coloredPoints.AddRange(pnts8);

            #endregion Parallel Reads
            

            //double x = 0; double y = 0;
            //for (double i = 0; i < gridResolutionX; i++)
            //{
            //    for (double j = 0; j < gridResolutionY; j++)
            //    {
            //        if (e.Cancel)
            //            return;
            //        val = 0;
            //        x = xstep * i;
            //        y = ystep * j;
            //      //  val = rbf.Eval(new double[] { x, y });
            //        rbfs.ForEach(rbf => val += rbf.Eval(new double[] { x, y }));
            //        if (!double.IsNaN(val) && !double.IsInfinity(val))
            //        {
            //            Color c = GetColor(max, min, val);
            //            coloredPoints.Add(new ColoredPoint(new Point((int)x, (int)y), c));
                        
            //        }
            //    }
            //}
            //pntCollection.Add(coloredPoints);
        }

        int HiddenNodeCount = 3;
        kMeans.RBFLiteCluster RBF;
        List<ColoredPoint> coloredPoints = new List<ColoredPoint>();

        public System.Drawing.Color InterpolateBetweenColors(double max, double min, double val, System.Drawing.Color StartColor, System.Drawing.Color EndColor)
        {
            System.Drawing.Color color1 = StartColor;
            System.Drawing.Color color2 = EndColor;
            double fraction = val / (max - min);
            System.Drawing.Color color3 = System.Drawing.Color.FromArgb(
                (int)(color1.R * fraction + color2.R * (1 - fraction)),
                (int)(color1.G * fraction + color2.G * (1 - fraction)),
                (int)(color1.B * fraction + color2.B * (1 - fraction)));

            return color3;
        }
        public System.Drawing.Color GetColor(double max, double min, double val)
        {
            double del = (max - min) / 7;
            double[] nodes = new double[8];
            for (int i = 0; i < nodes.Length; i++)
                nodes[i] = ((7 - i) * min + i * max) / 7;

            int r, g, b;
            if (val >= nodes[7])
            {
                r = 255;
                g = 255;
                b = 255;
            }
            else if (val >= nodes[6])
            {
                r = 255;
                g = (int)(255 * (val - nodes[6]) / del);
                b = 255;
            }
            else if (val >= nodes[5])
            {
                r = 255;
                g = 0;
                b = (int)(255 * (val - nodes[5]) / del);
            }
            else if (val >= nodes[4])
            {
                r = 255;
                g = 255 - (int)(255 * (val - nodes[4]) / del);
                b = 0;
            }
            else if (val >= nodes[3])
            {
                r = (int)(255 * (val - nodes[3]) / del);
                g = 255;
                b = 0;
            }
            else if (val >= nodes[2])
            {
                r = 0;
                g = 255;
                b = 255 - (int)(255 * (val - nodes[2]) / del);
            }
            else if (val >= nodes[1])
            {
                r = 0;
                g = (int)(255 * (val - nodes[1]) / del);
                b = 255;
            }
            else if (val >= nodes[0])
            {
                r = 0;
                g = 0;
                b = (int)(255 * (val - nodes[0]) / del);
            }
            else
            {
                r = 0;
                g = 0;
                b = 0;
            }

            // clamps color values at 0-255
            Clamp<int>(ref r, 255, 0);
            Clamp<int>(ref g, 255, 0);
            Clamp<int>(ref b, 255, 0);

            return System.Drawing.Color.FromArgb(r, g, b);
        }

        public T Clamp<T>(ref T value, T max, T min)
         where T : System.IComparable<T>
        {
            T result = value;
            if (value.CompareTo(max) > 0)
                result = max;
            if (value.CompareTo(min) < 0)
                result = min;
            value = result;
            return result;
        }

        public void SetNetworkVariables(List<List<Point>> inputs, List<Kluster> output, double g)
        {
            gamma = g;
            if (inputs.Count == 0)
                return;

            if (worker.IsBusy)
            {
                worker.CancelAsync();
                Application.DoEvents();
                return;
            }

            worker.RunWorkerAsync(new object[] { inputs, output });

        }
        //void makeboard()
        //{


        //}
        Point MoveTo;
        private void MultRBF_MouseDown(object sender, MouseEventArgs e)
        {
            //MoveTo = new System.Drawing.Point(e.X, e.Y);
            //double val = 0;
            //rbfs.ForEach(rbf => { val += rbf.Eval(MoveTo); });
            //if (!double.IsNaN(val) && !double.IsInfinity(val))
            //{
            //    Color c = InterpolateBetweenColors(20000, 0, val, Color.White, Color.Black);
            //    coloredPoints.Add(new ColoredPoint(MoveTo, c));
            //    Invalidate();
            //}

        }

        private void MultRBF_MouseUp(object sender, MouseEventArgs e)
        {
            //MoveTo = new Point(-1000, -1000);
        }
        Icon target;
        private void MultRBF_Paint_1(object sender, PaintEventArgs e)
        {
            if (coloredPoints.Count == 0)
                return;

            Graphics graphicsObj = e.Graphics;
            graphicsObj.Clear(Color.White);

            coloredPoints.ForEach(pnt => { graphicsObj.FillRectangle(new SolidBrush(pnt.color), new Rectangle(pnt.X, pnt.Y, PixelDensity, PixelDensity)); });

            int parentCount = Main.parents.Count;
            for (int i = 0; i < parentCount; i++)
            {
                graphicsObj.DrawIcon(target, new Rectangle(Main.parents[i].X - 10, Main.parents[i].Y - 10, 20, 20));
            }
        }
        //bool RBFSwitch = false;
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            //RBFSwitch = checkBox1.Checked;
        }

        private void MultRBF_MouseDown_1(object sender, MouseEventArgs e)
        {
            Main.Form1_MouseDown(sender, e);
        }

        private void MultRBF_MouseMove(object sender, MouseEventArgs e)
        {
            Main.Form1_MouseMove(sender, e);
        }

        private void MultRBF_MouseUp_1(object sender, MouseEventArgs e)
        {
            Main.Form1_MouseUp(sender, e);
        }

        private void MultRBF_SizeChanged(object sender, EventArgs e)
        {
            surfaceForm.Size = new Size(this.Size.Width, this.Size.Height);
        }
    }

    public class ColoredPoint
    {
        public Color color = Color.White;
        Point p = new Point();
        public double val = 0;

        public int X
        {
            get { return p.X; }
            set { p.X = value; }
        }

        public int Y
        {
            get { return p.Y; }
            set { p.Y = value; }
        }

        public ColoredPoint(Point pnt, Color c)
        {
            p = new Point(pnt.X, pnt.Y);
            color = c;
        }
        public ColoredPoint(Point pnt, Color c, double value)
        {
            val = value;
            p = new Point(pnt.X, pnt.Y);
            color = c;
        }
    }

    //class RBFLiteCluster
    //{
    //    List<double> weights = new List<double>();
    //    List<double[]> centers = new List<double[]>();
    //    List<double[]> cluster = new List<double[]>();
    //    public double gamma = 0.01;

    //    public RBFLiteCluster(List<double[]> inputs, List<double[]> output)
    //    {
    //        centers = new List<double[]>(inputs);
    //        cluster = new List<double[]>(output);
    //        double[,] phiMat = new double[centers.Count, output.Count];
    //        for (int i = 0; i < output.Count; i++)
    //            for (int j = 0; j < centers.Count; j++)
    //                phiMat[j, i] = Math.Exp(-gamma * getDistance(output[i], centers[j]));

    //        List<double> distances = new List<double>(inputs.Count);

    //        inputs.ForEach(val => { distances.Add(100); });

    //        weights = solveCluster(phiMat, distances.ToArray());
    //    }

    //    public double getDistance(double[] parent, double[] sat)
    //    {
    //        return Math.Sqrt(Math.Pow(sat[0] - parent[0], 2) + Math.Pow(sat[1] - parent[1], 2));
    //    }

    //    List<double> solveCluster(double[,] phi, double[] z)
    //    {
    //        //( (phi(T)*phi)^-1 )*phi(T)*y
    //        var matrixA = new DenseMatrix(phi);
    //        var matTrans = matrixA.Transpose();

    //        var vectorB = new DenseVector(z);

    //        Vector<double> resultX = (matTrans * matrixA).Inverse() * matTrans * vectorB;
    //        List<double> w2 = new List<double>(resultX.ToArray());
    //        return w2;
    //    }

    //    public double Eval(double[] p)
    //    {
    //        double ret = 0;
    //        for (int i = 0; i < cluster.Count; i++)
    //            ret += weights[i] * Math.Exp(-gamma * getDistance(p, cluster[i]));

    //        return ret;
    //    }
    //}
}
