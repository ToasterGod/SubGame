using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SubGame.Elements;
using SubGame.Types;
using System.Collections.Generic;
using System.Linq;

namespace SubGame.Levels
{
    public class Level1To5 : LevelBase, ILevel
    {
        #region Inheritance implementation
        public Level1To5(GraphicsDeviceManager aGraphics, ContentManager aContent, LevelConfig aLevelConfig)
            : base(aGraphics, aContent, aLevelConfig)
        {}
        #endregion

        #region Interface implementation methods
        public int AccessBoatHits => myBoatHits;
        public int AccessSubHits => mySubHits;
        public int AccessBoatHitsAllowed => myConfig.Health;
        public int AccessSubHitsRequired => myConfig.RequiredSubHits;



        public void Initialize()
        {
            myClouds = new List<CloudElement>();
            GenerateInitialClouds(myContent);
            myOcean = new StaticElement(1.0f, new Vector2(0, mySurfaceLevel));
            myBoat = new PlayerElement(1.0f, 0.01f, 0.0f, 1.5f, new Vector2(0, mySurfaceLevel), myGraphics, myConfig.Sinkbombs);
            myBoat.AccessSinkBombReleased += SinkBombReleased;
            mySubs = new List<EnemyElement>();
            myMines = new List<MineElement>();
            mySinkBombs = new List<SinkBombElement>();
            myBooms = new List<StaticElement>();
            mySoundEffects = new List<SoundEffect>();

            //Level1 = three subs at the time, each having one mine
            for (int i = 0; i < myConfig.Subs; i++)
            {
                EnemyElement mySub = new EnemyElement(mySurfaceLevel, 0.6f, 0.0f, 0.0f, 1.0f, new Vector2(0, 0), myGraphics, myConfig.Mines);
                mySub.AccessMineReleased += MineReleased;
                myBoat.AccessWhereIsTheBoat += mySub.BoatIsFoundAt;
                mySubs.Add(mySub);
            }

            int tempStaticTextTop = myGraphics.PreferredBackBufferHeight - 100;
            myStatusPanelLeft = new StaticText(new Vector2(20, tempStaticTextTop), new Vector2(300, 80), myGraphics);
            myStatusPanelRight = new StaticText(new Vector2(myGraphics.PreferredBackBufferWidth - 320, tempStaticTextTop), new Vector2(300, 80), myGraphics);

        }

        public void LoadContent()
        {
            myOcean.LoadContent(myContent, "Backgrounds/SolidOcean");

            foreach (EnemyElement sub in mySubs)
            {
                sub.LoadContent(myContent, new string[] { "Elements/SlowSub", "Elements/MediumSub", "Elements/FastSub" }, "Elements/Mine");
            }

            myBoat.LoadContent(myContent, "Elements/Boat", "Elements/Sinkbomb");

            mySoundEffects.Add(myContent.Load<SoundEffect>("Sounds/Bomb"));

            myStatusPanelLeft.LoadContent(myContent, "Status");
            myStatusPanelRight.LoadContent(myContent, "Status");

        }

        public void Update(GameTime aGameTime)
        {
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

            foreach (MineElement mine in myMines.ToList())
            {
                mine.Update(aGameTime);
                if (mine.MyHitBox.Intersects(myBoat.MyHitBox))
                {
                    mySoundEffects[0].Play();
                    myBoatHits++;
                    myBooms.Add(GenerateMyBoom(myContent, 1.0f, myBoat.AccessPosition, aGameTime.TotalGameTime.Seconds + 2));
                    myMines.Remove(mine);
                }
                if (mine.AccessSurfaced && mine.AccessSurfacedTime + 3 <= aGameTime.TotalGameTime.Seconds)
                {
                    myMines.Remove(mine);
                }
            }

            foreach (SinkBombElement sinkBomb in mySinkBombs.ToList())
            {
                sinkBomb.Update(aGameTime);
                foreach (EnemyElement sub in mySubs)
                {
                    if (sinkBomb.MyHitBox.Intersects(sub.MyHitBox))
                    {
                        mySoundEffects[0].Play();
                        mySubHits++;
                        myBooms.Add(GenerateMyBoom(myContent, 1.0f, sub.AccessPosition, aGameTime.TotalGameTime.Seconds + 2));
                        sub.ResetSub();
                        mySinkBombs.Remove(sinkBomb);
                    }
                }
                //TODO! Add it as an available shootable sinkbomb to myBoat
                if (sinkBomb.AccessPosition.Y > myGraphics.PreferredBackBufferHeight)
                {
                    mySinkBombs.Remove(sinkBomb);
                }
            }

            foreach (StaticElement boom in myBooms.ToList())
            {
                if (aGameTime.TotalGameTime.Seconds > boom.AccessTimeToLive)
                {
                    myBooms.Remove(boom);
                }
            }
        }

        public void Draw(SpriteBatch mySpriteBatch, GameTime gameTime)
        {
            //Throw out a cloud randomly
            if ((gameTime.TotalGameTime.Seconds % 20) == 0 && gameTime.TotalGameTime.Seconds != myLatestAddedCloud)
            {
                GenerateRandomCloud(myContent, myGraphics, gameTime);
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

            foreach (StaticElement boom in myBooms)
            {
                boom.Draw(mySpriteBatch);
            }

            myOcean.Draw(mySpriteBatch);

            myStatusPanelLeft.Draw(mySpriteBatch, $"Level: {myConfig.Name}\nBoat hits: {myBoatHits}");
            myStatusPanelRight.Draw(mySpriteBatch, $"Level: {myConfig.Name}\nSub hits: {mySubHits}");
        }
        #endregion

        #region Internal methods
        private void SinkBombReleased(SinkBombElement aSinkBomb)
           => mySinkBombs.Add(aSinkBomb);

        private void MineReleased(MineElement aMine)
            => myMines.Add(aMine);

        public StaticElement GenerateMyBoom(ContentManager myContent, float aScale, Vector2 aPosition, int aTimeToLive)
        {
            StaticElement boom = new StaticElement(aScale, aPosition, aTimeToLive);
            boom.LoadContent(myContent, "Elements/Boom");
            return boom;
        }

        private void GenerateInitialClouds(ContentManager myContent)
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
                aCloudElement.LoadContent(myContent, new string[] { "Elements/cloud1", "Elements/cloud2", "Elements/cloud3", "Elements/cloud4" });
                myClouds.Add(aCloudElement);
            }
        }

        private void GenerateRandomCloud(ContentManager myContent, GraphicsDeviceManager myGraphics, GameTime aGameTime)
        {
            float tempRandomSpeed = RandomNumber.Between(3, 8) / 10.0f;

            myLatestAddedCloud = aGameTime.TotalGameTime.Seconds;
            CloudElement aCloudElement = new CloudElement(0.6f, -1.0f, 0.0f, tempRandomSpeed, new Vector2(myGraphics.PreferredBackBufferWidth, RandomNumber.Between(1, 50)), myGraphics);
            aCloudElement.LoadContent(myContent, new string[] { "Elements/cloud1", "Elements/cloud2", "Elements/cloud3", "Elements/cloud4" });
            myClouds.Add(aCloudElement);
            foreach (CloudElement cloud in myClouds.Where(s => s.AccessOutOfBounds).ToList())
            {
                myClouds.Remove(cloud);
            }
        }
        #endregion
    }
}
