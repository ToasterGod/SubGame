using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SubGame.Elements;
using SubGame.Types;
using System.Collections.Generic;
using System.Linq;

namespace SubGame
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Level1 : Game
    {
        private readonly GraphicsDeviceManager myGraphics;
        private SpriteBatch mySpriteBatch;

        // All the new
        private readonly int mySurfaceLevel = 280;
        private List<CloudElement> myClouds;
        private StaticElement myOcean;
        private PlayerElement myBoat;
        private List<EnemyElement> mySubs;
        private List<MineElement> myMines;
        private List<SinkBombElement> mySinkBombs;
        private StaticText myStatusPanelLeft;
        private StaticText myStatusPanelRight;

        private readonly int myBoatHits;
        private readonly int mySubHits;
        private int myLatestAddedCloud;

        public Level1()
        {
            myGraphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = 1920, // set this value to the desired width of your window, 1280 is better on lower resolution screen
                PreferredBackBufferHeight = 1080 // set this value to the desired height of your window, 1024 is better on lower resolution screen
            };
            myGraphics.ApplyChanges();
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            myClouds = new List<CloudElement>();
            GenerateInitialClouds();
            myOcean = new StaticElement(1.0f, 0.0f, new Vector2(0, mySurfaceLevel));
            myBoat = new PlayerElement(1.0f, 0.01f, 0.0f, 1.5f, new Vector2(0, mySurfaceLevel), myGraphics);
            myBoat.AccessSinkBombReleased += SinkBombReleased;
            mySubs = new List<EnemyElement>();
            myMines = new List<MineElement>();
            mySinkBombs = new List<SinkBombElement>();
            //Level1 = three subs at the time, each having one mine
            for (int i = 0; i < 3; i++)
            {
                EnemyElement sub = new EnemyElement(mySurfaceLevel, 0.6f, 0.0f, 0.0f, 1.0f, new Vector2(0, 0), myGraphics);
                sub.AccessMineReleased += MineReleased;
                myBoat.AccessWhereIsTheBoat += sub.BoatIsFoundAt;
                mySubs.Add(sub);
            }

            int staticTextTop = myGraphics.PreferredBackBufferHeight - 100;
            myStatusPanelLeft = new StaticText(new Vector2(20, staticTextTop), new Vector2(300, 80), myGraphics);
            myStatusPanelRight = new StaticText(new Vector2(myGraphics.PreferredBackBufferWidth - 320, staticTextTop), new Vector2(300, 80), myGraphics);

            base.Initialize();
        }

        private void SinkBombReleased(SinkBombElement aSinkBomb) 
            => mySinkBombs.Add(aSinkBomb);

        private void MineReleased(MineElement aMine) 
            => myMines.Add(aMine);

        protected override void LoadContent()
        {
            mySpriteBatch = new SpriteBatch(GraphicsDevice);

            myOcean.LoadContent(Content, "Backgrounds/SolidOcean");

            foreach (EnemyElement sub in mySubs)
            {
                sub.LoadContent(Content, new string[] { "Elements/SlowSub", "Elements/MediumSub", "Elements/FastSub" }, "Elements/Mine");
            }

            myBoat.LoadContent(Content, "Elements/Boat", "Elements/Sinkbomb");

            myStatusPanelLeft.LoadContent(Content, "Status");
            myStatusPanelRight.LoadContent(Content, "Status");
        }

        protected override void Update(GameTime aGameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            foreach (CloudElement sky in myClouds)
            {
                sky.Update(aGameTime);
            }

            foreach (EnemyElement sub in mySubs)
            {
                //Sub update will call its mines update
                sub.Update(aGameTime);

            }

            myBoat.Update(aGameTime);

            foreach (MineElement mine in myMines)
            {
                mine.Update(aGameTime);
                //if (mine.AccessTimeout == true)
                //{
                //    mine.AccessPosition = new Vector2(AccessPosition.X + AccessSize.Width / 2, AccessPosition.Y + AccessSize.Height / 2);
                //    mine.AccessTimeout = false;
                //}
                if (mine.HitBox.Intersects(myBoat.HitBox))
                {
                    myBoat.HasBeenHit();
                }
            }
            foreach (SinkBombElement sinkBomb in mySinkBombs)
            {
                sinkBomb.Update(aGameTime);
            }

            base.Update(aGameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.LightSkyBlue);
            mySpriteBatch.Begin();
            // Begin your drawing code here

            //Throw out a cloud randomly
            if ((gameTime.TotalGameTime.Seconds % 20) == 0 && gameTime.TotalGameTime.Seconds != myLatestAddedCloud)
            {
                GenerateRandomCloud(gameTime);
            }

            foreach (CloudElement cloud in myClouds)
            {
                cloud.Draw(mySpriteBatch);
            }

            foreach (EnemyElement sub in mySubs)
            {
                sub.Draw(mySpriteBatch);
            }

            myBoat.Draw(mySpriteBatch);

            foreach (MineElement mine in myMines)
            {
                mine.Draw(mySpriteBatch);
            }
            foreach (SinkBombElement sinkBomb in mySinkBombs)
            {
                sinkBomb.Draw(mySpriteBatch);
            }

            myOcean.Draw(mySpriteBatch);

            myStatusPanelLeft.Draw(mySpriteBatch, $"Boat hits: {myBoatHits}");
            myStatusPanelRight.Draw(mySpriteBatch, $"Sub hits: {mySubHits}");

            // End your drawing code here
            mySpriteBatch.End();
            base.Draw(gameTime);
        }

        private void GenerateInitialClouds()
        {
            float tempRandomSpeed = RandomNumber.Between(3, 8) / 10.0f;

            //Generate five random clouds initially and place them on the screen from start
            foreach (int cloudPosition in new int[]
            {
                RandomNumber.Between(0, myGraphics.PreferredBackBufferWidth/5*1),
                RandomNumber.Between(myGraphics.PreferredBackBufferWidth/5*1, myGraphics.PreferredBackBufferWidth/5*2),
                RandomNumber.Between(myGraphics.PreferredBackBufferWidth/5*2, myGraphics.PreferredBackBufferWidth/5*3),
                RandomNumber.Between(myGraphics.PreferredBackBufferWidth/5*3, myGraphics.PreferredBackBufferWidth/5*4),
                RandomNumber.Between(myGraphics.PreferredBackBufferWidth/5*4, myGraphics.PreferredBackBufferWidth/5*5)
            })
            {
                CloudElement aCloudElement = new CloudElement(0.6f, -1.0f, 0.0f, tempRandomSpeed, new Vector2(cloudPosition, RandomNumber.Between(1, 50)), myGraphics);
                aCloudElement.LoadContent(Content, new string[] { "Elements/cloud1", "Elements/cloud2", "Elements/cloud3", "Elements/cloud4" });
                myClouds.Add(aCloudElement);
            }
        }

        private void GenerateRandomCloud(GameTime aGameTime)
        {
            float tempRandomSpeed = RandomNumber.Between(3, 8) / 10.0f;

            myLatestAddedCloud = aGameTime.TotalGameTime.Seconds;
            CloudElement aCloudElement = new CloudElement(0.6f, -1.0f, 0.0f, tempRandomSpeed, new Vector2(myGraphics.PreferredBackBufferWidth, RandomNumber.Between(1, 50)), myGraphics);
            aCloudElement.LoadContent(Content, new string[] { "Elements/cloud1", "Elements/cloud2", "Elements/cloud3", "Elements/cloud4" });
            myClouds.Add(aCloudElement);
            foreach (CloudElement cloud in myClouds.Where(s => s.AccessOutOfBounds).ToList())
            {
                myClouds.Remove(cloud);
            }
        }
    }
}