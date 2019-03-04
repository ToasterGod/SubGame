using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SubGame.Animations
{
    //Source: http://www.xnadevelopment.com/tutorials/scrollinga2dbackground/ScrollingA2DBackground.shtml
    class AnimatedBackground
    {
        //The current position of the Sprite
        public Vector2 Position = new Vector2(0, 0);

        //The texture object used when drawing the sprite, is loaded from the contentmanager in LoadContent
        private Texture2D mSpriteTexture;

        //The size of the Sprite, initialized in LoadContent
        public Rectangle Size;

        //Used to size the Sprite up or down from the original image
        public float Scale = 1.0f;

        //Load the texture for the sprite using the Content Pipeline
        public void LoadContent(ContentManager theContentManager, string theAssetName)
        {
            //Load the image
            mSpriteTexture = theContentManager.Load<Texture2D>(theAssetName);
            //Calculate the size
            Size = new Rectangle(0, 0, (int)(mSpriteTexture.Width * Scale), (int)(mSpriteTexture.Height * Scale));
        }

        //Draw the sprite to the screen, each instance of this class is called from the game class Draw
        public void Draw(SpriteBatch theSpriteBatch)
        {
            theSpriteBatch.Draw(mSpriteTexture, Position, 
                new Rectangle(0, 0, mSpriteTexture.Width, mSpriteTexture.Height), Color.White, 
                0.0f, Vector2.Zero, Scale, SpriteEffects.None, 0);
        }
    }
}
