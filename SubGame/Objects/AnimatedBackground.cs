using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SubGame.Objects
{
    //Source: http://www.xnadevelopment.com/tutorials/scrollinga2dbackground/ScrollingA2DBackground.shtml
    internal class AnimatedBackground
    {
        //The texture object used when drawing the sprite, is loaded from the contentmanager in LoadContent
        private Texture2D mySpriteTexture;

        //The current position of the Sprite
        public Vector2 AccessPosition { get; set; } = new Vector2(0, 0);

        //The size of the Sprite, initialized in LoadContent
        public Rectangle AccessSize { get; set; }

        //Used to size the Sprite up or down from the original image
        public float AccessScale { get; set; } = 1.0f;

        public AnimatedBackground(float aScale)
        {
            AccessScale = aScale;
        }

        //Load the texture for the sprite using the Content Pipeline
        public void LoadContent(ContentManager aContentManager, string anAssetName)
        {
            //Load the image
            mySpriteTexture = aContentManager.Load<Texture2D>(anAssetName);
            //Calculate the size
            AccessSize = new Rectangle(0, 0, (int)(mySpriteTexture.Width * AccessScale), (int)(mySpriteTexture.Height * AccessScale));
        }

        //Draw the sprite to the screen, each instance of this class is called from the game class Draw
        public void Draw(SpriteBatch aSpriteBatch)
        {
            aSpriteBatch.Draw(mySpriteTexture, AccessPosition,
                new Rectangle(0, 0, mySpriteTexture.Width, mySpriteTexture.Height), Color.White,
                0.0f, Vector2.Zero, AccessScale, SpriteEffects.None, 0);
        }
    }
}