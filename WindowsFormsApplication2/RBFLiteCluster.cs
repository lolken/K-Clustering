using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.LinearAlgebra.Generic.Factorization;
using MathNet.Numerics.LinearAlgebra.Generic;

namespace kMeans
{
    class RBFLiteCluster
    {
        List<double> weights = new List<double>();
        List<double[]> centers = new List<double[]>();
        List<double[]> cluster = new List<double[]>();
        public double gamma = 0.05;

        //allpoints, clusters, gamma, clusterCount
        public RBFLiteCluster(List<double[]> inputs, List<double[]> output, double g, List<double> clusterCount, bool RBFType)
        {
            gamma = g;
            if (RBFType)
            {
                centers = new List<double[]>(inputs);
                cluster = new List<double[]>(output);
                List<double> distances = new List<double>(inputs.Count);
                double[,] phiMat = new double[centers.Count, cluster.Count];
                double val = 0;

                double distance = 0;
                for (int i = 0; i < centers.Count; i++)
                {
                    for (int j = 0; j < cluster.Count; j++)
                    {
                        distance = getDistance(centers[i], cluster[j]);
                        val = Math.Pow(distance, 2) * Math.Log(distance);
                        phiMat[i, j] = double.IsNaN(val) ? 0 : val;
                    }
                }
                //weights = solve(phiMat, output.ToArray());
                double min = double.MinValue;
                //double distance = 0;
                int indexOfCluster = -1;

                for (int i = 0; i < centers.Count; i++)
                {
                    //distance = 0;
                    min = double.MaxValue;
                    for (int j = 0; j < cluster.Count; j++)
                    {
                        distance = getDistance(cluster[j], centers[i]);
                        if (distance < min)
                        {
                            min = distance;
                            indexOfCluster = j;
                        }
                    }
                    distances.Add(min);
                }

                double max = distances.Max();
                min = distances.Min();
                List<double> tmp = new List<double>();
                distances.ForEach(pnt => { tmp.Add(pnt / (max - min)); });

                //inputs.ForEach(val => { distances.Add(1000); });

                weights = solveCluster(phiMat, tmp.ToArray());
            }
            else
            {
                centers = new List<double[]>(output);
                cluster = new List<double[]>(output);
                List<double> distances = new List<double>(inputs.Count);
                double[,] phiMat = new double[centers.Count, cluster.Count];
                for (int i = 0; i < centers.Count; i++)
                {
                    for (int j = 0; j < cluster.Count; j++)
                    {
                        phiMat[i, j] = Math.Exp(-gamma * getDistance(cluster[j], centers[i]));
                    }
                }

                weights = solveCluster(phiMat, clusterCount.ToArray());
            }
        }

        public double getDistance(double[] parent, double[] sat)
        {
            return Math.Sqrt(Math.Pow(sat[0] - parent[0], 2) + Math.Pow(sat[1] - parent[1], 2));
        }

        List<double> solveCluster(double[,] phi, double[] z)
        {
            //( (phi(T)*phi)^-1 )*phi(T)*y
            var matrixA = new DenseMatrix(phi);
            var matTrans = matrixA.Transpose();

            var vectorB = new DenseVector(z);

            Vector<double> resultX = (matTrans * matrixA).Inverse() * matTrans * vectorB;
            List<double> w2 = new List<double>(resultX.ToArray());
            return w2;
        }

        public double Eval(double[] p)
        {
            double ret = 0;
            for (int i = 0; i < cluster.Count; i++)
                ret += weights[i] * Math.Exp(-gamma * getDistance(p, cluster[i]));

            return ret;
        }
    }
}
