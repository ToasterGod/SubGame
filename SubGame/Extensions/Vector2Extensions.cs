using Microsoft.Xna.Framework;

namespace SubGame.Extensions
{
    public static class Vector2Extensions
    {
        public static bool NearByHorizontal(this Vector2 aSource, Vector2 aTarget, float aDistance)
        {
            if (aSource.X < aTarget.X - aDistance || aSource.X > aTarget.X + aDistance)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool NearByVertical(this Vector2 aSource, Vector2 aTarget, float aDistance)
        {
            if (aSource.Y < aTarget.Y - aDistance || aSource.Y > aTarget.Y + aDistance)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
