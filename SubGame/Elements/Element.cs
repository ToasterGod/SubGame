using Microsoft.Xna.Framework;

namespace SubGame.Elements
{
    public class Element
    {
        public Vector2 AccessPosition { get; set; } = new Vector2(0, 0);
        public virtual Rectangle AccessSize { get; set; }


        public Element(Vector2 aPosition) 
            => AccessPosition = aPosition;
    }
}
