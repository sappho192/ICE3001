using System;
using GibbsPhenomenonTest.Core.Delegate;

namespace GibbsPhenomenonTest
{
    public class ApproxTest
    {
        public ApproxTest(uint N, double delta_t)
        {
            if (N != 0)
            {
                this.N = N;
            }
            else
            {
                this.N = 1;
            }

            if (delta_t > 0)
            {
                Delta_t = delta_t;
            }
            else
            {
                Delta_t = 0.001;
            }
        }

        public uint N
        {
            get; set;
        }

        public double Delta_t
        {
            get; set;
        }

        public Function1Continuous<double, double> x_t = t =>
        {
            return Math.Sign(Math.Sin(Math.PI * t));
        };

        public Function1Discrete<double, double, uint> xN_t =
        (t, NMax) =>
        {
            double result = 0;
            for (int n = 1; n <= NMax; n += 2)
            {
                result += (1 / (double)n) * Math.Sin(n * Math.PI * t);
            }
            result *= (4 / Math.PI);

            return result;
        };

    }
}
