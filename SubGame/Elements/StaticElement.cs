using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SubGame.Elements
{
    public class StaticElement : Element
    {
        private Texture2D texture;
        public float AccessAngle { get; set; }
        public float GetScale { get; private set; }
        public int AccessTimeToLive { get; }

        public StaticElement(float aScale, Vector2 aPosition) 
            : base(aPosition)
        {
            GetScale = aScale;
            AccessTimeToLive = -1;
        }

        public StaticElement(float aScale, Vector2 aPosition, int aTimeToLive)
            : base(aPosition)
        {
            GetScale = aScale;
            AccessTimeToLive = aTimeToLive;
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
