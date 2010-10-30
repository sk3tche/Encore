namespace Trinity.Encore.Framework.Core.Mathematics
{
    public static class MathExtensions
    {
        public static float Round(this float value, float round = FastMath.RoundValue)
        {
            return (int)(value + round);
        }

        public static double Round(this double value, double round = FastMath.RoundValue)
        {
            return (int)(value + round);
        }

        public static decimal Round(this decimal value, decimal round = (decimal)FastMath.RoundValue)
        {
            return (int)(value + round);
        }
    }
}
