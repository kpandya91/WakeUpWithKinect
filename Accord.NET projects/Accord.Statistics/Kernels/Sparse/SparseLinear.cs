﻿// Accord Statistics Library
// The Accord.NET Framework
// http://accord-framework.net
//
// Copyright © César Souza, 2009-2015
// cesarsouza at gmail.com
//
//    This library is free software; you can redistribute it and/or
//    modify it under the terms of the GNU Lesser General Public
//    License as published by the Free Software Foundation; either
//    version 2.1 of the License, or (at your option) any later version.
//
//    This library is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
//    Lesser General Public License for more details.
//
//    You should have received a copy of the GNU Lesser General Public
//    License along with this library; if not, write to the Free Software
//    Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA
//

namespace Accord.Statistics.Kernels.Sparse
{
    using System;

    /// <summary>
    ///   Sparse Linear Kernel.
    /// </summary>
    /// 
    /// <remarks>
    ///   The Sparse Linear kernel accepts inputs in the libsvm sparse format.
    /// </remarks>
    /// 
    [Serializable]
    public sealed class SparseLinear : KernelBase, IKernel
    {
        private double constant;

        /// <summary>
        ///   Constructs a new Linear kernel.
        /// </summary>
        /// 
        /// <param name="constant">A constant intercept term. Default is 1.</param>
        /// 
        public SparseLinear(double constant)
        {
            this.constant = constant;
        }

        /// <summary>
        ///   Constructs a new Linear Kernel.
        /// </summary>
        /// 
        public SparseLinear()
            : this(1) { }

        /// <summary>
        ///   Gets or sets the kernel's intercept term.
        /// </summary>
        /// 
        public double Constant
        {
            get { return constant; }
            set { constant = value; }
        }

        /// <summary>
        ///   Sparse Linear kernel function.
        /// </summary>
        /// 
        /// <param name="x">Sparse vector <c>x</c> in input space.</param>
        /// <param name="y">Sparse vector <c>y</c> in input space.</param>
        /// <returns>Dot product in feature (kernel) space.</returns>
        /// 
        public override double Function(double[] x, double[] y)
        {
            if (x == y)
                return 1.0;

            return Product(x, y) + constant;
        }

       

        /// <summary>
        ///   Computes the squared distance in feature space
        ///   between two points given in input space.
        /// </summary>
        /// 
        /// <param name="x">Vector <c>x</c> in feature (kernel) space.</param>
        /// <param name="y">Vector <c>y</c> in feature (kernel) space.</param>
        /// <returns>Distance between <c>x</c> and <c>y</c> in input space.</returns>
        /// 
        public override double Distance(double[] x, double[] y)
        {
            if (x == y)
                return 0;

            return SquaredEuclidean(x,y);
        }




        /// <summary>
        ///   Computes the product of two vectors given in sparse representation.
        /// </summary>
        /// 
        /// <param name="x">The first vector <c>x</c>.</param>
        /// <param name="y">The second vector <c>y</c>.</param>
        /// 
        /// <returns>The inner product <c>x * y</c> between the given vectors.</returns>
        /// 
        public static double Product(double[] x, double[] y)
        {
            double sum = 0;

            int i = 0, j = 0;

            while (i < x.Length && j < y.Length)
            {
                double posx = x[i];
                double posy = y[j];

                if (posx == posy)
                {
                    sum += x[i + 1] * y[j + 1];

                    i += 2; j += 2;
                }
                else if (posx < posy)
                {
                    i += 2;
                }
                else if (posx > posy)
                {
                    j += 2;
                }
            }

            return sum;
        }

        /// <summary>
        ///   Computes the squared Euclidean distance of two vectors given in sparse representation.
        /// </summary>
        /// 
        /// <param name="x">The first vector <c>x</c>.</param>
        /// <param name="y">The second vector <c>y</c>.</param>
        /// 
        /// <returns>
        ///   The squared Euclidean distance <c>d² = |x - y|²</c> between the given vectors.
        /// </returns>
        /// 
        public static double SquaredEuclidean(double[] x, double[] y)
        {
            double sum = 0;

            int i = 0, j = 0;

            while (i < x.Length && j < y.Length)
            {
                double posx = x[i];
                double posy = y[j];

                if (posx == posy)
                {
                    double d = x[i + 1] - y[j + 1];

                    sum += d * d;

                    i += 2; j += 2;
                }
                else if (posx < posy)
                {
                    double d = x[j + 1];
                    sum += d * d;
                    i += 2;
                }
                else if (posx > posy)
                {
                    double d = y[j + 1];
                    sum += d * d;
                    j += 2;
                }
            }

            return sum;
        }

    }
}
