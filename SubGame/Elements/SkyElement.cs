using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using SubGame.Types;

namespace SubGame.Elements
{
    internal class SkyElement : MovingElement
    {
        public bool AccessOutOfBounds { get; set; }

        public SkyElement(float aScale, float aDirection, float aRotation, float aSpeed, Vector2 aPosition, GraphicsDeviceManager aManager) 
            : base(aScale, aDirection, aRotation, aSpeed, aPosition, aManager) 
            => AccessDirection = -1.0f;
        
        public void LoadContent(ContentManager aContentManager, string[] someAssets)
        {
            int tempRandom = RandomNumber.Between(1, someAssets.Length) - 1;
            LoadContent(aContentManager, someAssets[tempRandom]);
        }

        public override void Update(GameTime aGameTime)
        {
            //// Reset the sky if it is outside the left or right edge
            if (AccessPosition.X + AccessSize.Width < 0 && AccessDirection < 0.0f)
            {
                //Sky going left outside of left edge
                AccessOutOfBounds = true;
            }
            else if (AccessPosition.X > myManager.PreferredBackBufferWidth && AccessDirection > 0.0f)
            {
                //Sky going right outside of right edge
                AccessOutOfBounds = true;
            }

            // Calculate the movement of all the clouds
            CalcHorizontalMovement(AccessSpeed);

            base.Update(aGameTime);
        }

        public override void Draw(SpriteBatch aSpriteBatch) 
            => base.Draw(aSpriteBatch);
    }
}
