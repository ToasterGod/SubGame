using ConfigModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace SubGame.Elements
{
    public class MovingElement : Element
    {
        protected GraphicsDeviceManager myManager;
        protected Texture2D myTexture; //Full access allowed by inheritance

        private SpriteEffects myEffects = SpriteEffects.None;

        //public bool AccessBeenHit { get; set; }
        //public int AccessHitTime { get; set; }

        public float AccessDirection { get; set; }
        public float AccessRotation { get; set; }
        public float AccessSpeed { get; set; }
        public float AccessScale { get; set; }
        public Texture2D MyTexture => myTexture;  //Read-only access allowed by instantiation
        public Rectangle MyHitBox => new Rectangle(AccessPosition.ToPoint(), new Point(AccessSize.Width, AccessSize.Height));
        public bool AccessIsEnemy { get; set; }

        public MovingElement(float aScale, float aDirection, float aRotation, float aSpeed, Vector2 aPosition, GraphicsDeviceManager aManager)
            : base(aPosition)
        {
            AccessScale = aScale;
            AccessDirection = aDirection;
            AccessRotation = aRotation;
            AccessSpeed = aSpeed;
            myManager = aManager;
        }

        public virtual void LoadContent(ContentManager aContentManager, string anAsset)
        {
            myTexture = aContentManager.Load<Texture2D>(anAsset);
            AccessSize = new Rectangle(0, 0, (int)Math.Round(myTexture.Width * AccessScale), (int)Math.Round(myTexture.Height * AccessScale));
        }

        public virtual void Update(GameTime aGameTime)
        {
        }

        public virtual void Draw(SpriteBatch aSpriteBatch)
        {
            Rectangle mySourceRectangle = new Rectangle(0, 0, myTexture.Width, myTexture.Height);
            Vector2 myOrigin = new Vector2(0, 0);

            //Check direction of object so we always have the front towards moving direction
            //If it stops it should stay in last known direction
            if (AccessDirection < 0.0f)
            {
                //The object is moving from right to left, flip the image, default is always from right to left
                myEffects = SpriteEffects.FlipHorizontally;
            }
            else if (AccessDirection > 0.0f)
            {
                //The object is moving from left ro right, don't flip the image
                myEffects = SpriteEffects.None;
            }

            aSpriteBatch.Draw(myTexture, AccessPosition, mySourceRectangle, Color.White, AccessRotation, myOrigin, AccessScale, myEffects, 1);
        }

        protected void CalcHorizontalMovement(float aSpeed)
        {
            if (AccessDirection < 0.0f || AccessDirection > 0.0f)
            {
                Vector2 tempDirection = new Vector2(AccessDirection, 0);
                tempDirection.Normalize();
                AccessPosition += tempDirection * aSpeed;
            }
        }

        protected void CalcVerticalMovement(float aSpeed)
        {
            if (AccessDirection < 0.0f || AccessDirection > 0.0f)
            {
                Vector2 tempDirection = new Vector2(0, AccessDirection);
                tempDirection.Normalize();
                AccessPosition += tempDirection * aSpeed;
            }
        }
    }
}