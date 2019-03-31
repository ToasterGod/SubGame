using Microsoft.Xna.Framework;

namespace SubGame.Elements
{
    internal class Element
    {
        public Vector2 Position { get; set; } = new Vector2(0, 0);
        public Rectangle Size { get; set; }


        public Element(Vector2 position) => this.Position = position;
    }
}
