using Microsoft.Xna.Framework;

namespace Trinity.Encore.Framework.Game.Mathematics
{
    public static class MathExtensions
    {
        public static Vector2 ToVector2(this Vector3 vector)
        {
            return new Vector2(vector.X, vector.Y);
        }

        public static Vector3 ToVector3(this Vector2 vector, bool max = true)
        {
            return new Vector3(vector.X, vector.Y, max ? float.MaxValue : float.MinValue);
        }
    }
}
