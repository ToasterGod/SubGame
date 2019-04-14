using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;

namespace SubGame.Elements
{
    internal class PlayerElement : MovingElement
    {
        private bool myReleasePressed;
        private int myLeftEdge;
        private int myRightEdge;
        private List<SinkBombElement> mySinkBombList;
        private string myWeaponAsset;
        private ContentManager myContentManager;

        public Rectangle AccessCollisionBox { get; internal set; }
        public SinkBombReleasedDelegate AccessSinkBombReleased { get; set; }
        public WhereIsTheBoatDelegate AccessWhereIsTheBoat { get; set; }

        public PlayerElement(float aScale, float aDirection, float aRotation, float aSpeed, Vector2 aPosition, GraphicsDeviceManager aManager)
            : base(aScale, aDirection, aRotation, aSpeed, aPosition, aManager) 
            => GenerateNewWeapons();

        private void GenerateNewWeapons()
        {
            mySinkBombList = new List<SinkBombElement>();
            for (int i = 0; i < 4; i++)
            {
                mySinkBombList.Add(new SinkBombElement(1.0f, AccessDirection, AccessRotation, 1.0f, AccessPosition, myManager));
            }
        }

        public void LoadContent(ContentManager aContentManager, string anAsset, string aWeaponAsset)
        {
            myContentManager = aContentManager;
            myWeaponAsset = aWeaponAsset;
            LoadContent(aContentManager, anAsset);
            foreach (SinkBombElement sinkBomb in mySinkBombList)
            {
                sinkBomb.LoadContent(aContentManager, aWeaponAsset);
                sinkBomb.AccessPosition = new Vector2(AccessPosition.X + AccessSize.Width / 2, AccessPosition.Y + AccessSize.Height / 2);
            }
            //Skapa och Ladda vapen
            //textureSinkbomb = contentManager.Load<Texture2D>(weaponAsset);
        }

        public override void LoadContent(ContentManager aContentManager, string anAsset)
        {
            base.LoadContent(aContentManager, anAsset);
            //Level boat at sealevel and set left and right endpoints
            myLeftEdge = 60;
            myRightEdge = myManager.PreferredBackBufferWidth - 60 - AccessSize.Width;
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

            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                //The else statement below will make this a trigger, it's only valid once, no repetition
                if (myReleasePressed == false)
                {
                    myReleasePressed = true;
                    SinkBombElement sinkBomb = mySinkBombList.FirstOrDefault();
                    if (sinkBomb != null)
                    {
                        AccessSinkBombReleased?.Invoke(sinkBomb);
                        mySinkBombList.Remove(sinkBomb);
                    }
                }
            }
            else
            {
                //Force to unpress space before it is allowed to drop next sink bomb
                myReleasePressed = false;
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
            AccessWhereIsTheBoat?.Invoke(new Rectangle(AccessPosition.ToPoint(), AccessSize.Size), aGameTime);

            foreach (SinkBombElement sinkBomb in mySinkBombList)
            {
                sinkBomb.Update(aGameTime);
                sinkBomb.AccessPosition = new Vector2(AccessPosition.X + AccessSize.Width / 2, AccessPosition.Y + AccessSize.Height / 2);
            }
        }

        public override void Draw(SpriteBatch aSpriteBatch)
        {
            base.Draw(aSpriteBatch);
            foreach (SinkBombElement sinkBomb in mySinkBombList)
            {
                sinkBomb.Draw(aSpriteBatch);
            }
        }
    }
}
