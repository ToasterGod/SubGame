using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SubGame.Elements
{
    internal class StaticElement : Element
    {
        private Texture2D myTexture;
        public float AccessAngle { get; set; }
        public float GetScale { get; private set; }

        public StaticElement(float aScale, float anAngle, Vector2 aPosition) : base(aPosition)
        {
            GetScale = aScale;
            AccessAngle = anAngle;
        }

        public virtual void LoadContent(ContentManager aContentManager, string anAssetName) 
            => myTexture = aContentManager.Load<Texture2D>(anAssetName);

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            Rectangle tempSourceRectangle = new Rectangle(0, 0, myTexture.Width, myTexture.Height);

            spriteBatch.Draw(myTexture, AccessPosition, tempSourceRectangle, Color.White, AccessAngle, new Vector2(0, 0), GetScale, SpriteEffects.None, 1);
        }
    }
}
