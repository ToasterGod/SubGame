using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using SubGame.Types;

namespace SubGame.Elements
{
    internal class EnemyElement : MovingElement
    {
        private readonly float[] myDirectionLevels = { -1.0f, 1.0f };
        private int mySubSpeed;

        public int GetDepth { get; private set; }

        private ContentManager myContentManager;
        private string[] myAssets;

        private int myBehindLeftEdge;
        private int myBehindRightEdge;

        //Mine stuff
        private List<MineElement> myMineList;
        private string myWeaponAsset;

        public EnemyElement(float aScale, float aDirection, float aRotation, float aSpeed, Vector2 aPosition, GraphicsDeviceManager aManager)
            : base(aScale, aDirection, aRotation, aSpeed, aPosition, aManager)
        {
            GenerateNewEnemy();
        }

        private void GenerateNewEnemy()
        {
            mySubSpeed = RandomNumber.Between(40, 130);
            //Depth = depthLevels[RandomNumber.Between(1, 3) - 1];
            GetDepth = RandomNumber.Between(450, 850);
            AccessDirection = myDirectionLevels[RandomNumber.Between(1, 2) - 1];
            //Only initial position for Depth value, it will be final after LoadContent
            AccessPosition = new Vector2(manager.PreferredBackBufferWidth, GetDepth);
            GenerateNewWeapons();
        }

        private void GenerateNewWeapons()
        {
            myMineList = new List<MineElement>();
            for (int i = 0; i < 4; i++)
            {
                myMineList.Add(new MineElement(1.0f, AccessDirection, AccessRotation, 1.0f, AccessPosition, manager));
            }
        }

        public void LoadContent(ContentManager aContentManager, string[] anAssets, string aWeaponAsset)
        {
            this.myContentManager = aContentManager;
            this.myAssets = anAssets;
            this.myWeaponAsset = aWeaponAsset;
            if (mySubSpeed < 70)
            {
                //slow sub
                AccessSpeed = 0.5f;
                LoadContent(aContentManager, anAssets[0]);
            }
            else if (mySubSpeed < 100)
            {
                //fast sub
                AccessSpeed = 0.8f;
                LoadContent(aContentManager, anAssets[1]);
            }
            else
            {
                // fastest sub
                AccessSpeed = 1.2f;
                LoadContent(aContentManager, anAssets[2]);
            }
            foreach (var mine in myMineList)
            {
                mine.LoadContent(aContentManager, aWeaponAsset);
                mine.AccessPosition = new Vector2(AccessPosition.X + AccessSize.Width / 2, AccessPosition.Y + AccessSize.Height / 2);
            }
        }

        public override void LoadContent(ContentManager aContentManager, string anAsset)
        {
            base.LoadContent(aContentManager, anAsset);

            //Set left and right startpoints
            myBehindLeftEdge = 0 - AccessSize.Width;
            myBehindRightEdge = manager.PreferredBackBufferWidth;
            if (AccessDirection > 0.0f)
            {
                AccessPosition = new Vector2(myBehindLeftEdge, AccessPosition.Y);
            }
            else if (AccessDirection < 0.0f)
            {
                AccessPosition = new Vector2(myBehindRightEdge, AccessPosition.Y);
            }
        }

        public override void Update(GameTime aGameTime)
        {
            //Add pre update stuff here:

            //Then call base update
            base.Update(aGameTime);

            //Add post update stuff here:
            // Reset the subs if any of them is outside the left edge
            if (AccessPosition.X + AccessSize.Width < 0 && AccessDirection < 0.0f)
            {
                //Sub going left outside of left edge
                GenerateNewEnemy();
                LoadContent(myContentManager, myAssets, myWeaponAsset);
            }
            else if (AccessPosition.X > manager.PreferredBackBufferWidth && AccessDirection > 0.0f)
            {
                //Sub going right outside of right edge
                GenerateNewEnemy();
                LoadContent(myContentManager, myAssets, myWeaponAsset);
            }

            // Calculate the movement
            CalcHorizontalMovement(AccessSpeed);

            foreach (var mine in myMineList.Where(m => m.AccessReleased == false))
            {
                mine.Update(aGameTime);
                mine.AccessPosition = new Vector2(AccessPosition.X + AccessSize.Width / 2, AccessPosition.Y + AccessSize.Height / 2);
            }

            foreach (var mine in myMineList.Where(m => m.AccessReleased == true))
            {
                mine.Update(aGameTime);
            }
        }

        public override void Draw(SpriteBatch aSpriteBatch)
        {
            //Mines are drawn before sub, that way they will be hidden behind the sub
            base.Draw(aSpriteBatch);
            foreach (var mine in myMineList)
            {
                mine.Draw(aSpriteBatch);
            }
        }

        public void BoatIsFoundAt(Rectangle aLocation)
        {
            //throw new NotImplementedException();
        }

        public void ReleaseMine()
        {
            var myMine = myMineList.FirstOrDefault(m => m.AccessReleased == false);
            if (myMine != null)
            {
                myMine.AccessReleased = true;
                myMine.AccessDirection = -1.0f;
                myMine.AccessSpeed = 10.0f;
                myMine.AccessPosition = AccessPosition;
            }
        }
    }
}
