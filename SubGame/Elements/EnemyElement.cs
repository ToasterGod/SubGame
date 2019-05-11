using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SubGame.Extensions;
using SubGame.Types;
using System.Collections.Generic;
using System.Linq;

namespace SubGame.Elements
{
    public class EnemyElement : MovingElement
    {
        public MineReleasedDelegate AccessMineReleased { get; set; }
        private readonly float[] myDirectionLevels = { -1.0f, 1.0f };
        private int myDropFrequency;
        private int mySubSpeed;

        public int GetDepth { get; private set; }

        private ContentManager myContentManager;
        private string[] myAssets;

        private int myBehindLeftEdge;
        private int myBehindRightEdge;

        //Mine stuff
        private List<MineElement> myMineList;
        private string myWeaponAsset;
        private double myLatestDroppedMine;
        private readonly int myMineCount;
        private readonly int mySurfaceLevel;

        public EnemyElement(int aSurfaceLevel, float aScale, float aDirection, float aRotation, float aSpeed, Vector2 aPosition, GraphicsDeviceManager aManager, int someMines)
            : base(aScale, aDirection, aRotation, aSpeed, aPosition, aManager)
        {
            mySurfaceLevel = aSurfaceLevel;
            AccessIsEnemy = true;
            myMineCount = someMines;
            GenerateNewEnemy();
        }

        private void GenerateNewEnemy()
        {
            mySubSpeed = RandomNumber.Between(40, 130);
            //Depth = depthLevels[RandomNumber.Between(1, 3) - 1];
            GetDepth = RandomNumber.Between(450, 850);
            AccessDirection = myDirectionLevels[RandomNumber.Between(1, 2) - 1];
            //Only initial position for Depth value, it will be final after LoadContent
            AccessPosition = new Vector2(myManager.PreferredBackBufferWidth, GetDepth);
            GenerateNewWeapons();
        }

        private void GenerateNewWeapons()
        {
            myMineList = new List<MineElement>();
            for (int i = 0; i < myMineCount; i++)
            {
                myMineList.Add(new MineElement(mySurfaceLevel, 1.0f, AccessDirection, AccessRotation, 1.0f, AccessPosition, myManager));
            }
        }

        public void LoadContent(ContentManager aContentManager, string[] anAssets, string aWeaponAsset)
        {
            myContentManager = aContentManager;
            myAssets = anAssets;
            myWeaponAsset = aWeaponAsset;
            if (mySubSpeed < 70)
            {
                //slow sub
                AccessSpeed = 0.5f;
                myDropFrequency = RandomNumber.Between(800, 1000);
                LoadContent(aContentManager, anAssets[0]);
            }
            else if (mySubSpeed < 100)
            {
                //fast sub
                AccessSpeed = 0.8f;
                myDropFrequency = RandomNumber.Between(700, 900);
                LoadContent(aContentManager, anAssets[1]);
            }
            else
            {
                // fastest sub
                AccessSpeed = 1.2f;
                myDropFrequency = RandomNumber.Between(400, 600);
                LoadContent(aContentManager, anAssets[2]);
            }
            foreach (MineElement mine in myMineList)
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
            myBehindRightEdge = myManager.PreferredBackBufferWidth;
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
                //Sub going left outside of left edge, create a new
                ResetSub();
            }
            else if (AccessPosition.X > myManager.PreferredBackBufferWidth && AccessDirection > 0.0f)
            {
                //Sub going right outside of right edge, create a new
                ResetSub();
            }

            // Calculate the movement
            CalcHorizontalMovement(AccessSpeed);

            foreach (MineElement mine in myMineList)
            {
                //Make all not dropped mines follow the sub
                mine.AccessPosition = new Vector2(AccessPosition.X + AccessSize.Center.X, AccessPosition.Y + AccessSize.Center.Y);
            }
        }

        public void ResetSub()
        {
            GenerateNewEnemy();
            LoadContent(myContentManager, myAssets, myWeaponAsset);
        }

        public override void Draw(SpriteBatch aSpriteBatch)
        {
            base.Draw(aSpriteBatch);

            //Mines belonging to the sub doesn't have to be drawn, so skip the foreach loop once we are done
            //Drawing mines that are dropped (no longer belongs to the sub) will be handled in the Level1 class
            foreach (MineElement mine in myMineList)
            {
                mine.Draw(aSpriteBatch);
            }
        }

        public void BoatIsFoundAt(Rectangle aLocation, GameTime gameTime)
        {
            //Check if we are within -50 - 0 - +50 from the boat
            if (AccessPosition.NearByHorizontal(aLocation.Center.ToVector2(), 50))
            {
                //Allow drop of mines every third second
                if (gameTime.TotalGameTime.TotalSeconds > myLatestDroppedMine + 3)
                {
                    myLatestDroppedMine = gameTime.TotalGameTime.TotalSeconds;
                    ReleaseMine();
                }
            }
        }

        public void ReleaseMine()
        {
            MineElement myMine = myMineList.FirstOrDefault();
            if (myMine != null)
            {
                myMine.AccessDirection = -1.0f;
                myMine.AccessSpeed = 1.0f;
                myMine.AccessPosition = AccessPosition;
                AccessMineReleased?.Invoke(myMine);
                myMineList.Remove(myMine);
            }
        }
    }
}


