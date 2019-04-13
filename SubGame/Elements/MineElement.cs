
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SubGame.Types;

namespace SubGame.Elements
{
    public class MineElement : MovingElement
    {
        private readonly int mySurfaceLevel;
        private int mySurfaceTime;
        public bool AccessTimeout { get; set; }

        public MineElement(int aSurfaceLevel, float aScale, float aDirection, float aRotation, float aSpeed, Vector2 aPosition, GraphicsDeviceManager aManager)
            : base(aScale, aDirection, aRotation, aSpeed, aPosition, aManager)
        {
            mySurfaceLevel = aSurfaceLevel;
            //Released = false;
        }

        public override void LoadContent(ContentManager aContentManager, string anAsset)
            => base.LoadContent(aContentManager, anAsset);

        public override void Update(GameTime aGameTime)
        {
            base.Update(aGameTime);

            if (AccessPosition.Y > mySurfaceLevel)
            {
                CalcVerticalMovement(AccessSpeed);
            }
            //else
            //{
            //    if (mySurfaceTime == 0)
            //    {
            //        mySurfaceTime = aGameTime.TotalGameTime.Seconds;
            //    }
            //    else if (aGameTime.TotalGameTime.Seconds > mySurfaceTime + 3)
            //    {
            //        AccessTimeout = true;
            //    }
            //}

        }

        public override void Draw(SpriteBatch aSpriteBatch)
        {
            Rectangle mySourceRectangle = new Rectangle(0, 0, myTexture.Width, myTexture.Height);
            Vector2 myOrigin = new Vector2(0, 0);
            SpriteEffects myEffects = SpriteEffects.None;
            if (AccessDirection < 0.0f)
                myEffects = SpriteEffects.FlipHorizontally;
            aSpriteBatch.Draw(myTexture, AccessPosition, mySourceRectangle, Color.White, AccessRotation, myOrigin, AccessScale, myEffects, 1);

            //base.Draw(spriteBatch);
        }
    }
}
