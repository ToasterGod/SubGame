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
    class AnimatedObject
    {
        //The current position of the Sprite
        public Vector2 aPosition = new Vector2(0, 0);

        public Vector2 aSpeed { get; set; }

        //The texture object used when drawing the sprite, is loaded from the contentmanager in LoadContent
        private Texture2D aSpriteTexture;

        //The size of the Sprite, initialized in LoadContent
        public Rectangle aSize;

        //Used to size the Sprite up or down from the original image
        public float aScale = 1.0f;

        public void LoadContent(ContentManager aContentManager, string anAssetName)
        {
            //Load the image
            aSpriteTexture = aContentManager.Load<Texture2D>(anAssetName);
            //Calculate the size
            aSize = new Rectangle(0, 0, (int)(aSpriteTexture.Width * aScale), (int)(aSpriteTexture.Height * aScale));
        }

        public void Draw(SpriteBatch theSpriteBatch)
        {
            theSpriteBatch.Draw(aSpriteTexture, aPosition,
                new Rectangle(0, 0, aSpriteTexture.Width, aSpriteTexture.Height), Color.White,
                0.0f, Vector2.Zero, aScale, SpriteEffects.None, 0);
        }
    }
}
