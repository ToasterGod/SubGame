using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SubGame.Elements
{
    internal class MovingElement : Element
    {
        protected GraphicsDeviceManager myManager;
        protected Texture2D myTexture;

        public float AccessDirection { get; set; }
        public float AccessRotation { get; set; }
        public float AccessSpeed { get; set; }
        public float AccessScale { get; set; }
        public Texture2D MyTexture => myTexture;

        public MovingElement(float aScale, float aDirection, float aRotation, float aSpeed, Vector2 aPosition, GraphicsDeviceManager aManager)
            : base(aPosition)
        {
            AccessScale = aScale;
            AccessDirection = aDirection;
            AccessRotation = aRotation;
            AccessSpeed = aSpeed;
            this.myManager = aManager;
        }

        public virtual void LoadContent(ContentManager aContentManager, string anAsset)
        {
            myTexture = aContentManager.Load<Texture2D>(anAsset);
            AccessSize = new Rectangle(0, 0, (int)Math.Round(myTexture.Width * AccessScale), (int)Math.Round(myTexture.Height * AccessScale));
        }

        public virtual void Update(GameTime aGameTime)
        {}

        public virtual void Draw(SpriteBatch aSpriteBatch)
        {
            Rectangle mySourceRectangle = new Rectangle(0, 0, myTexture.Width, myTexture.Height);
            Vector2 myOrigin = new Vector2(0, 0);
            SpriteEffects myEffects = SpriteEffects.None;
            if (AccessDirection < 0.0f)
                myEffects = SpriteEffects.FlipHorizontally;
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