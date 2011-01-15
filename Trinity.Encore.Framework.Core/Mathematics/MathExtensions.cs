namespace Trinity.Encore.Framework.Core.Mathematics
{
    public static class MathExtensions
    {
        public static float Round(this float value, float roundValue = FastMath.RoundValue)
        {
            return (int)(value + roundValue);
        }

        public static double Round(this double value, double roundValue = FastMath.RoundValue)
        {
            return (int)(value + roundValue);
        }

        public static decimal Round(this decimal value, decimal roundValue = (decimal)FastMath.RoundValue)
        {
            return (int)(value + roundValue);
        }
    }
}
