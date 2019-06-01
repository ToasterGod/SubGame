using ConfigModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SubGame.Elements
{
    public class SinkBombElement : ArmedElement
    {
        public SinkBombElement(float aScale, float aDirection, float aRotation, float aSpeed, Vector2 aPosition, GraphicsDeviceManager aManager)
            : base(aScale, aDirection, aRotation, aSpeed, aPosition, aManager, LevelDifficulty.Normal, LevelDifficulty.Normal)
        { }

        public override void LoadContent(ContentManager aContentManager, string anAsset)
            => base.LoadContent(aContentManager, anAsset);

        public override void Update(GameTime aGameTime)
        {
            base.Update(aGameTime);
            CalcVerticalMovement(AccessSpeed);
        }

        public override void Draw(SpriteBatch aSpriteBatch)
        {
            Rectangle mySourceRectangle = new Rectangle(0, 0, myTexture.Width, myTexture.Height);
            Vector2 myOrigin = new Vector2(0, 0);
            SpriteEffects myEffects = SpriteEffects.None;
            if (AccessDirection < 0.0f)
            {
                myEffects = SpriteEffects.FlipHorizontally;
            }
            aSpriteBatch.Draw(myTexture, AccessPosition, mySourceRectangle, Color.White, AccessRotation, myOrigin, AccessScale, myEffects, 1);
            //base.Draw(aSpriteBatch);
        }
    }
}