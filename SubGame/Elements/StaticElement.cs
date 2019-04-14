using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SubGame.Elements
{
    internal class StaticElement : Element
    {
        private Texture2D texture;
        public float AccessAngle { get; set; }
        public float GetScale { get; private set; }

        public StaticElement(float aScale, float anAngle, Vector2 aPosition) : base(aPosition)
        {
            GetScale = aScale;
            AccessAngle = anAngle;
        }

        public virtual void LoadContent(ContentManager aContentManager, string anAssetName) 
            => texture = aContentManager.Load<Texture2D>(anAssetName);

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            Rectangle tempSourceRectangle = new Rectangle(0, 0, texture.Width, texture.Height);

            spriteBatch.Draw(texture, AccessPosition, tempSourceRectangle, Color.White, AccessAngle, new Vector2(0, 0), GetScale, SpriteEffects.None, 1);
        }
    }
}
