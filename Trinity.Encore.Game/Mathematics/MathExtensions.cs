using System.Diagnostics.Contracts;
using Mono.GameMath;

namespace Trinity.Encore.Game.Mathematics
{
    public static class MathExtensions
    {
        public static Vector2 ToVector2(this Vector3 vector)
        {
            return new Vector2(vector.X, vector.Y);
        }

        public static Vector3 ToVector3(this Vector2 vector, float z = 0.0f)
        {
            return new Vector3(vector.X, vector.Y, z);
        }
    }
}
