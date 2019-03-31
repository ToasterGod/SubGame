using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SubGame.Elements
{
    internal class StaticElement : Element
    {
        private Texture2D texture;
        public float Angle { get; set; }
        public float Scale { get; private set; }

        public StaticElement(float scale, float angle, Vector2 position) : base(position)
        {
            Scale = scale;
            Angle = angle;
        }

        public virtual void LoadContent(ContentManager aContentManager, string anAssetName)
        {
            texture = aContentManager.Load<Texture2D>(anAssetName);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            Rectangle sourceRectangle = new Rectangle(0, 0, texture.Width, texture.Height);

            spriteBatch.Draw(texture, Position, sourceRectangle, Color.White, Angle, new Vector2(0, 0), Scale, SpriteEffects.None, 1);
        }
    }
}
