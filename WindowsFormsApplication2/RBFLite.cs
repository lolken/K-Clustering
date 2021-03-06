﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.LinearAlgebra.Generic.Factorization;
using MathNet.Numerics.LinearAlgebra.Generic;

namespace kMeans
{
    public class RBFLite
    {
        List<double> weights = new List<double>();
        List<double[]> centers = new List<double[]>();
        public double gamma = 0.5;

        public RBFLite(List<double[]> inputs, double[] centerXYZ, double density, double g)
        {
            gamma = g;
            centers = new List<double[]>(inputs);
            double[,] phiMat = new double[centers.Count, centers.Count];
            //double distance = 0;
            //for (int i = 0; i < centers.Count; i++)
            //{
            //    for (int j = 0; j < centers.Count; j++)
            //    {
            //        distance = getDistance(centers[i], centers[j]);
            //        phiMat[i, j] = Math.Pow(distance, 2) * Math.Log(distance);
            //    }
            //}
            //weights = solve(phiMat, output.ToArray());

            //gamma = CalculateGamma(inputs, centerXYZ);
            for (int i = 0; i < centers.Count; i++)
                for (int j = 0; j < centers.Count; j++)
                    phiMat[i, j] = Math.Exp(-gamma * getDistance(centers[i], centers[j]));

            List<double> targets = new List<double>();
            inputs.ForEach(val => { targets.Add(density); });
            weights = solve(phiMat, targets.ToArray());
        }

        public RBFLite(List<double[]> inputs, double[] centerXYZ, double g)
        {
            gamma = g;
            centers = new List<double[]>(inputs);
            double[,] phiMat = new double[centers.Count, centers.Count];
            //double distance = 0;
            //for (int i = 0; i < centers.Count; i++)
            //{
            //    for (int j = 0; j < centers.Count; j++)
            //    {
            //        distance = getDistance(centers[i], centers[j]);
            //        phiMat[i, j] = Math.Pow(distance, 2) * Math.Log(distance);
            //    }
            //}
            //weights = solve(phiMat, output.ToArray());

            //gamma = CalculateGamma(inputs, centerXYZ);
            for (int i = 0; i < centers.Count; i++)
                for (int j = 0; j < centers.Count; j++)
                    phiMat[i, j] = Math.Exp(-gamma * getDistance(centers[i], centers[j]));

            List<double> targets = new List<double>();
            inputs.ForEach(val => { targets.Add(100); });
            weights = solve(phiMat, targets.ToArray());
        }

        private double CalculateGamma(List<double[]> inputs, double[] centerXYZ)
        {
            double sum = 0;
            inputs.ForEach(val =>
            {
                sum += getDistance(centerXYZ, val);

            });
            return Math.Sqrt(sum / (1000.0 * inputs.Count));
        }

        public double getDistance(double[] parent, double[] sat)
        {
            return Math.Sqrt(Math.Pow(sat[0] - parent[0], 2) + Math.Pow(sat[1] - parent[1], 2));
        }

        List<double> solve(double[,] phi, double[] z)
        {
            var matrixA = new DenseMatrix(phi);
            var matAInverse = matrixA.Inverse();

            var vectorB = new DenseVector(z);

            Vector<double> resultX = matAInverse.LU().Solve(vectorB);
            List<double> w2 = new List<double>(resultX.ToArray());
            return w2;
        }

        public double Eval(double[] p)
        {
            double ret = 0;
            for (int i = 0; i < weights.Count; i++)
                ret += weights[i] * Math.Exp(-gamma * getDistance(p, centers[i]));

            return ret;
        }
    }

    public static class RBFStatic
    {
        delegate double distanceFighter(double[] p1, double[] p2);

        public static List<double> SolveRightNow(List<double[]> inputs, List<double> outputs, double gamma)
        {
            distanceFighter getDistance = ((p1, p2) =>
            {
                double ret = 0;
                for (int i = 0; i < p1.Rank; i++)
                    ret += Math.Pow(p1[i] - p2[i], 2);
                return Math.Sqrt(ret);
            });

            List<double> weights = new List<double>(inputs.Count);

            double[,] phiMat = new double[inputs.Count, inputs.Count];
            for (int i = 0; i < inputs.Count; i++)
                for (int j = 0; j < inputs.Count; j++)
                    phiMat[i, j] = Math.Exp(-gamma * getDistance(inputs[i], inputs[j]));

            var matrixA = new DenseMatrix(phiMat);
            var matAInverse = matrixA.Inverse();

            var vectorB = new DenseVector(outputs.ToArray());

            Vector<double> resultX = matAInverse.LU().Solve(vectorB);
            weights = new List<double>(resultX.ToArray());

            return weights;
        }
    }
}
