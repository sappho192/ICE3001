using GibbsPhenomenonTest.Core.Delegate;

namespace GibbsPhenomenonTest.Core
{
    public static class Approx
    {
        public static double[] GetDiscreteT(int from, int to, double delta_t)
        {
            if (from > to)
            {
                return new double[0];
            }

            uint count = (uint)((to - from) / delta_t);
            double[] result = new double[count];

            for (int i = 0; i < count; i++)
            {
                result[i] = i * delta_t;
            }

            return result;
        }

        public static double[] SetOf(Function1Continuous<double, double> x_t, int from, int to, double delta_t)
        {
            if(from > to) { return new double[0]; }

            var dis_t = GetDiscreteT(from, to, delta_t);
            double[] f_t = new double[dis_t.Length];

            for (int i = 0; i < dis_t.Length; i++)
            {
                f_t[i] = x_t(dis_t[i]);
            }

            return f_t;
        }

        public static double[] SetOf(Function1Discrete<double, double, uint> xN_t, int from, int to, double delta_t, uint N)
        {
            if(from > to) { return new double[0]; }

            var dis_t = GetDiscreteT(from, to, delta_t);
            double[] f_t = new double[dis_t.Length];

            for (int i = 0; i < dis_t.Length; i++)
            {
                f_t[i] = xN_t(dis_t[i], N);
            }

            return f_t;
        }
    }
}
