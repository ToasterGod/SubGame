using Microsoft.Xna.Framework;

namespace SubGame.Extensions
{
    public static class Vector2Extensions
    {
        public static bool NearByHorizontal(this Vector2 source, Vector2 target, float distance)
        {
            if (source.X < target.X - distance || source.X > target.X + distance)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool NearByVertical(this Vector2 source, Vector2 target, float distance)
        {
            if (source.Y < target.Y - distance || source.Y > target.Y + distance)
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
