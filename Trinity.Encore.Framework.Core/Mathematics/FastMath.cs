using System;

namespace Trinity.Encore.Framework.Core.Mathematics
{
    /// <summary>
    /// Provides math methods that are generally faster than those
    /// of System.Math.
    /// </summary>
    public static class FastMath
    {
        /// <summary>
        /// Value used to round floating-point numbers.
        /// </summary>
        public const float RoundValue = 0.5f;

        /// <summary>
        /// Math.PI with less precision, but faster.
        /// </summary>
        public const float PI = (float)Math.PI;

        /// <summary>
        /// PI * 2.
        /// </summary>
        public const float PI2 = PI * 2;

        /// <summary>
        /// Math.E with less precision, but faster.
        /// </summary>
        public const float E = (float)Math.E;

        public static short MinMax(short value, short min, short max)
        {
            if (value > max)
                return max;

            return value < min ? min : value;
        }

        public static ushort MinMax(ushort value, ushort min, ushort max)
        {
            if (value > max)
                return max;

            return value < min ? min : value;
        }

        public static int MinMax(int value, int min, int max)
        {
            if (value > max)
                return max;

            return value < min ? min : value;
        }

        public static uint MinMax(uint value, uint min, uint max)
        {
            if (value > max)
                return max;

            return value < min ? min : value;
        }

        public static long MinMax(long value, long min, long max)
        {
            if (value > max)
                return max;

            return value < min ? min : value;
        }

        public static ulong MinMax(ulong value, ulong min, ulong max)
        {
            if (value > max)
                return max;

            return value < min ? min : value;
        }

        public static float MinMax(float value, float min, float max)
        {
            if (value > max)
                return max;

            return value < min ? min : value;
        }

        public static double MinMax(double value, double min, double max)
        {
            if (value > max)
                return max;

            return value < min ? min : value;
        }

        public static decimal MinMax(decimal value, decimal min, decimal max)
        {
            if (value > max)
                return max;

            return value < min ? min : value;
        }
    }
}
