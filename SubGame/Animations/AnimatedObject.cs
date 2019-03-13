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
        //The texture object used when drawing the sprite, is loaded from the contentmanager in LoadContent
        private Texture2D mySpriteTexture;

        // The absolute position of mySpriteTexture on the screen, used for collisiondetection
        public Rectangle AccessCollisionBox { get; set; }
        public TimeSpan AccessCollisionTime { get; set; } = TimeSpan.MinValue;
        //The current position of the Sprite
        public Vector2 AccessPosition { get; set; } = new Vector2(0, 0);
        // Defines speed for movement
        public Vector2 AccessSpeed { get; set; }
        // The size of the Sprite, initialized in LoadContent
        public Rectangle AccessSize { get; set; }
        // Used to size the Sprite up or down from the original image
        public float AccessScale { get; set; } = 1.0f;

        public void LoadContent(ContentManager aContentManager, string anAssetName)
        {
            //Load the image
            mySpriteTexture = aContentManager.Load<Texture2D>(anAssetName);
            //Calculate the size
            AccessSize = new Rectangle(0, 0, (int)(mySpriteTexture.Width * AccessScale), (int)(mySpriteTexture.Height * AccessScale));
            AccessCollisionBox = new Rectangle(int.MaxValue, int.MaxValue, int.MaxValue, int.MaxValue);
        }

        public void Draw(SpriteBatch aSpriteBatch)
        {
            aSpriteBatch.Draw(mySpriteTexture, AccessPosition,
                new Rectangle(0, 0, mySpriteTexture.Width, mySpriteTexture.Height), Color.White,
                0.0f, Vector2.Zero, AccessScale, SpriteEffects.None, 0);

            // Calculate left and top position of collision box (BoundingBox)
            int x1 = AccessPosition.ToPoint().X;
            int y1 = AccessPosition.ToPoint().Y;

            // Calculate right and bottom position of collision box (BoundingBox)
            int x2 = AccessPosition.ToPoint().X + mySpriteTexture.Width;
            int y2 = AccessPosition.ToPoint().Y + mySpriteTexture.Height;

            // Set new values to BoundingBox to track where the mySpriteTexture is located on the screen
            AccessCollisionBox = new Rectangle(new Point(x1,y1), new Point(x2, y2));
        }
    }
}
