using System;
using System.Collections.Generic;
using System.Linq;
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
        private List<SinkBombElement> mySinkBombList;
        private string myWeaponAsset;
        private ContentManager myContentManager;

        public Rectangle AccessCollisionBox { get; internal set; }
        public WhereIsTheBoatDelegate AccessWhereIsTheBoat { get; set; }

        public PlayerElement(float aScale, float aDirection, float aRotation, float aSpeed, Vector2 aPosition, GraphicsDeviceManager aManager)
            : base(aScale, aDirection, aRotation, aSpeed, aPosition, aManager)
        {
            GenerateNewWeapons();
        }

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
            foreach (var sinkBomb in mySinkBombList)
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

            if ((aGameTime.TotalGameTime.Milliseconds % 99) == 0)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Space))
                {
                    var sinkBomb = mySinkBombList.FirstOrDefault(s => s.AccessReleased == false);
                    if (sinkBomb != null)
                    {
                        sinkBomb.AccessReleased = true;
                    }
                }
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

            foreach (var sinkBomb in mySinkBombList.Where(s => s.AccessReleased == false))
            {
                sinkBomb.Update(aGameTime);
                sinkBomb.AccessPosition = new Vector2(AccessPosition.X + AccessSize.Width / 2, AccessPosition.Y + AccessSize.Height / 2);
            }

            foreach (var sinkBomb in mySinkBombList.Where(s => s.AccessReleased == true))
            {
                sinkBomb.Update(aGameTime);
            }
        }

        public override void Draw(SpriteBatch aSpriteBatch)
        {
            AccessWhereIsTheBoat?.Invoke(new Rectangle(AccessPosition.ToPoint(), AccessSize.Size));

            base.Draw(aSpriteBatch);
            foreach (var sinkBomb in mySinkBombList)
            {
                sinkBomb.Draw(aSpriteBatch);
            }
        }
    }
}
