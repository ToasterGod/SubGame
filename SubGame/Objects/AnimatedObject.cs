using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SubGame.Objects
{
    internal class AnimatedObject
    {
        //The texture object used when drawing the sprite, is loaded from the contentmanager in LoadContent
        private Texture2D mySpriteTexture;

        private Texture2D myBoom; //This should be for when sinkbomb hits the sub or when mine hits the ship, ship should be animatedobject too but without movement

        // The absolute position of mySpriteTexture on the screen, used for collisiondetection
        public Rectangle AccessCollisionBox { get; set; }

        //Bool to indicate collision detected
        public bool AccessCollisionDetected { get; set; }

        //Timer for collision
        public TimeSpan AccessCollisionTime { get; set; } = TimeSpan.MinValue;

        //The current position of the Sprite
        public Vector2 AccessPosition { get; set; } = new Vector2(0, 0);

        // Defines speed for movement
        public Vector2 AccessSpeed { get; set; }

        // Defines speed for movement
        public int AccessFireAt { get; set; }

        // The true size of the Sprite, initialized in LoadContent, mySpriteTexture.Bounds cannot be used since it is unscaled measures
        public Rectangle AccessSize { get; set; }

        // Used to size the Sprite up or down from the original image
        public float AccessScale { get; set; } = 1.0f;

        public AnimatedObject(float aScale, Vector2 aSpeed)
        {
            AccessScale = aScale;
            AccessSpeed = aSpeed;
        }

        public void LoadContent(ContentManager aContentManager, string anAssetName)
        {
            //Load the image
            mySpriteTexture = aContentManager.Load<Texture2D>(anAssetName);
            myBoom = aContentManager.Load<Texture2D>("Backgrounds/Boom");
            //Calculate the size
            AccessSize = new Rectangle(0, 0, (int)Math.Round(mySpriteTexture.Width * AccessScale), (int)Math.Round(mySpriteTexture.Height * AccessScale));
            AccessCollisionBox = new Rectangle(int.MaxValue, int.MaxValue, int.MaxValue, int.MaxValue);
        }

        public void Draw(SpriteBatch aSpriteBatch)
        {
            Rectangle tempBox = new Rectangle(0, 0, mySpriteTexture.Width, mySpriteTexture.Height);

            aSpriteBatch.Draw(mySpriteTexture, AccessPosition, tempBox, Color.White, 0.0f,
                Vector2.Zero, AccessScale, SpriteEffects.None, 0);

            //Bamalamadingdong-woopdidoopdicrashelicrash
            if (AccessCollisionDetected)
            {
                aSpriteBatch.Draw(myBoom, new Vector2(AccessPosition.X - (myBoom.Width / 2) + (AccessSize.Height / 2), AccessPosition.Y - (myBoom.Height / 2) + (AccessSize.Height / 2)), Color.White);
            }

            // Set new values to BoundingBox to track where the mySpriteTexture is located on the screen
            AccessCollisionBox = new Rectangle(AccessPosition.ToPoint(), new Point(AccessSize.Width, AccessSize.Height));

            //Generates output in Visual Studio Output - Debug window
            //Debug.WriteLine($"{AccessCollisionBox.X}x{AccessCollisionBox.Y}, {AccessCollisionBox.Width}x{AccessCollisionBox.Height}");
        }
    }
}