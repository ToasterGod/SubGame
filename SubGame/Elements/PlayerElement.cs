using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SubGame.Elements
{
    public delegate void WhereIsTheBoatDelegate(Rectangle location);

    internal class PlayerElement : MovingElement
    {
        private int myLeftEdge;
        private int myRightEdge;
        private Texture2D mySinkbombTexture;

        public Rectangle AccessCollisionBox { get; internal set; }
        public WhereIsTheBoatDelegate AccessWhereIsTheBoat { get; set; }

        public PlayerElement(float aScale, float aDirection, float aRotation, float aSpeed, Vector2 aPosition, GraphicsDeviceManager aManager)
            : base(aScale, aDirection, aRotation, aSpeed, aPosition, aManager)
        {
        }

        public void LoadContent(ContentManager aContentManager, string anAsset, string aWeaponAsset)
        {
            LoadContent(aContentManager, anAsset);
            //Skapa och Ladda vapen
            //textureSinkbomb = contentManager.Load<Texture2D>(weaponAsset);
        }

        public override void LoadContent(ContentManager aContentManager, string anAsset)
        {
            base.LoadContent(aContentManager, anAsset);
            //Level boat at sealevel and set left and right endpoints
            myLeftEdge = 60;
            myRightEdge = manager.PreferredBackBufferWidth - 60 - AccessSize.Width;
            AccessPosition = new Vector2(myLeftEdge, AccessPosition.Y - AccessSize.Height * 0.7f);
        }

        public override void Update(GameTime aGameTime)
        {
            AccessDirection = 0.0f;

            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                AccessDirection = -1.0f;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                AccessDirection = 1.0f;
            }

            //Keep the boat within screens left and right edge
            if (AccessPosition.X <= myLeftEdge && AccessDirection < 0.0f)
            {
                AccessDirection = 0.0f;
            }
            else if (AccessPosition.X > (myRightEdge) && AccessDirection > 0.0f)
            {
                AccessDirection = 0.0f;
            }

            CalcHorizontalMovement(AccessSpeed);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            AccessWhereIsTheBoat?.Invoke(new Rectangle(AccessPosition.ToPoint(), AccessSize.Size));

            base.Draw(spriteBatch);
        }
    }
}
