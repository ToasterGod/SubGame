using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace SubGame.Elements
{
    public class StaticElement : Element
    {
        private Texture2D myTexture;
        public float AccessAngle { get; set; }
        public float AccessScale { get; private set; }
        public int AccessTimeToLive { get; }
        public override Rectangle AccessSize => new Rectangle(0, 0, myTexture.Width, myTexture.Height);
        public Rectangle MyHitBox => new Rectangle(AccessPosition.ToPoint(), new Point(Convert.ToInt32(AccessSize.Width*AccessScale), Convert.ToInt32(AccessSize.Height*AccessScale)));

        public StaticElement(float aScale, Vector2 aPosition)
            : base(aPosition)
        {
            AccessScale = aScale;
            AccessTimeToLive = -1;
        }

        public StaticElement(float aScale, Vector2 aPosition, int aTimeToLive)
            : base(aPosition)
        {
            AccessScale = aScale;
            AccessTimeToLive = aTimeToLive;
        }

        public virtual void LoadContent(ContentManager aContentManager, string anAssetName) => myTexture = aContentManager.Load<Texture2D>(anAssetName);

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            Rectangle tempSourceRectangle = new Rectangle(0, 0, myTexture.Width, myTexture.Height);

            spriteBatch.Draw(myTexture, AccessPosition, tempSourceRectangle, Color.White, AccessAngle, new Vector2(0, 0), AccessScale, SpriteEffects.None, 1);
        }
    }
}
