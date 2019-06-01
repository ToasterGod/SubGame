using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SubGame.Types;

namespace SubGame.Elements
{
    internal class HelicopterElement : MovingElement
    {
        public bool AccessActive { get; private set; }
        public bool AccessLifting { get; private set; }
        public bool AccessLowering { get; private set; }
        public bool AccessHovering { get; private set; }
        public int AccessHoveringTime { get; private set; }
        public bool AccessLoadedWeapons { get; set; }

        public void SetActive()
        {
            AccessActive = true;
            AccessLowering = true;
            AccessHovering = false;
            AccessLifting = false;
            AccessLoadedWeapons = false;
        }

        private void SetInactive()
        {
            AccessActive = false;
            AccessLowering = false;
            AccessHovering = false;
            AccessLifting = false;
            AccessHoveringTime = 0;
            AccessPosition = new Vector2(RandomNumber.Between(0, myManager.PreferredBackBufferWidth - AccessSize.Width), 0 - AccessSize.Height);
        }

        public HelicopterElement(float aScale, float aDirection, float aRotation, float aSpeed, Vector2 aPosition, GraphicsDeviceManager aManager)
            : base(aScale, aDirection, aRotation, aSpeed, aPosition, aManager)
        {
        }

        public override void LoadContent(ContentManager aContentManager, string anAsset)
        {
            base.LoadContent(aContentManager, anAsset);
            SetInactive();
        }

        public override void Update(GameTime aGameTime)
        {
            // Not really needed since base.Update does nothing
            base.Update(aGameTime);

            if (AccessActive)
            {
                if (AccessPosition.Y > 0.0f)
                {
                    AccessHovering = true;
                    AccessLifting = false;
                    AccessLowering = false;
                }

                if (!AccessHovering)
                {
                    if (AccessLifting)
                    {
                        AccessDirection = -1.0f;
                    }
                    if (AccessLowering)
                    {
                        AccessDirection = 1.0f;
                    }
                }
                else if (AccessHovering)
                {
                    AccessDirection = 0.0f;
                    if (AccessHoveringTime == 0)
                    {
                        AccessHoveringTime = aGameTime.TotalGameTime.Seconds;
                    }
                    if (AccessHovering && AccessHoveringTime + 2 <= aGameTime.TotalGameTime.Seconds)
                    {
                        AccessLifting = true;
                        AccessHovering = false;
                        AccessLowering = false;
                        AccessHoveringTime = 0;
                        AccessDirection = -1.0f;
                    }
                }

                CalcVerticalMovement(AccessSpeed);

                if (AccessPosition.Y <= 0 - AccessSize.Height)
                {
                    SetInactive();
                }
            }
        }

        protected override void CalcVerticalMovement(float aSpeed)
        {
            if (AccessDirection < 0.0f || AccessDirection > 0.0f)
            {
                Vector2 tempDirection = new Vector2(0, AccessDirection);
                //tempDirection.Normalize();
                AccessPosition += tempDirection * aSpeed;
            }
        }

        public override void Draw(SpriteBatch aSpriteBatch)
        {
            Rectangle mySourceRectangle = new Rectangle(0, 0, myTexture.Width, myTexture.Height);
            Vector2 myOrigin = new Vector2(0, 0);
            aSpriteBatch.Draw(myTexture, AccessPosition, mySourceRectangle, Color.White, AccessRotation, myOrigin, AccessScale, SpriteEffects.None, 1);
        }
    }
}
