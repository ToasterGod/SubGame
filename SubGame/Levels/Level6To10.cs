﻿using ConfigModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SubGame.Elements;
using SubGame.Types;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SubGame.Levels
{
    public class Level6To10 : LevelBase, ILevel
    {
        private HelicopterElement myHelicopter;
        private StaticElement myIceberg;
        private List<Bullet> myBullets;

        #region Inheritance implementation
        public Level6To10(GraphicsDeviceManager aGraphics, ContentManager aContent, LevelData aLevelConfig)
            : base(aGraphics, aContent, aLevelConfig)
        { }
        #endregion

        #region Interface implementation properties
        public int AccessBoatHits => myBoatHits;
        public int AccessSubHits => mySubHits;
        public Rectangle AccessCollisionBox { get; internal set; }
        public int AccessBoatHitsAllowed => myConfig.Health;
        public int AccessSubHitsRequired => myConfig.RequiredSubHits;
        #endregion

        #region Interface implementation methods
        public void Initialize()
        {
            myClouds = new List<CloudElement>();
            GenerateInitialClouds(myContent);
            myOcean = new StaticElement(1.0f, new Vector2(0, mySurfaceLevel));
            myBoat = new PlayerElement(0.7f, 0.01f, 0.0f, 1.5f, new Vector2(0, mySurfaceLevel + 90), myGraphics, myConfig.Sinkbombs, myConfig.Level, myConfig.MovementDifficulty, myConfig.WeaponDifficulty);
            myBoat.AccessSinkBombReleased += SinkBombReleased;
            myBoat.BulletFired += BulletFired;
            mySubs = new List<EnemyElement>();
            myCreatures = new List<SeaCreatureElement>();
            myMines = new List<MineElement>();
            mySinkBombs = new List<SinkBombElement>();
            myBooms = new List<StaticElement>();
            mySoundEffects = new List<SoundEffect>();
            myHelicopter = new HelicopterElement(1.0f, 0.0f, 0.0f, 1.0f, new Vector2(0, 0), myGraphics);
            myBullets = new List<Bullet>();

            //Level1 = three subs at the time, each having one mine
            for (int i = 0; i < myConfig.Subs; i++)
            {
                EnemyElement mySub = new EnemyElement(mySurfaceLevel, 0.6f, 0.0f, 0.0f, 1.0f, new Vector2(0, 0), myGraphics, myConfig.Mines, myConfig.MovementDifficulty, myConfig.WeaponDifficulty);
                mySub.AccessMineReleased += MineReleased;
                myBoat.AccessWhereIsTheBoat += mySub.BoatIsFoundAt;
                mySubs.Add(mySub);
            }
            for (int i = 0; i < 3; i++)
            {
                SeaCreatureElement myCreature = new SeaCreatureElement(mySurfaceLevel, 0.6f, 0.0f, 0.0f, 1.0f, new Vector2(0, 0), myGraphics);
                myCreatures.Add(myCreature);
            }

            GenerateInitialIceberg(myContent);

            int tempStaticTextTop = myGraphics.PreferredBackBufferHeight - 180;
            myStatusPanelLeft = new ScoreAndLevelBanner(new Vector2(20, tempStaticTextTop), new Vector2(300, 80), myGraphics);
            myStatusPanelCenter = new ScoreAndLevelBanner(new Vector2((myGraphics.PreferredBackBufferWidth / 2) - 160, tempStaticTextTop), new Vector2(300, 80), myGraphics);
            myStatusPanelRight = new ScoreAndLevelBanner(new Vector2(myGraphics.PreferredBackBufferWidth - 320, tempStaticTextTop), new Vector2(300, 80), myGraphics);

        }

        public void LoadContent()
        {
            myOcean.LoadContent(myContent, "Backgrounds/SolidOcean");

            foreach (EnemyElement sub in mySubs)
            {
                sub.LoadContent(myContent, new string[] { "Elements/SlowSub", "Elements/MediumSub", "Elements/FastSub" }, "Elements/Mine");
            }

            foreach (SeaCreatureElement creature in myCreatures)
            {
                creature.LoadContent(myContent, new string[] { "Elements/TraumatizedWhale", "Elements/MaliciousShark" });
            }

            myBoat.LoadContent(myContent, "Elements/BigAssBoat", "Elements/Sinkbomb", "Elements/Bullet");
            myHelicopter.LoadContent(myContent, "Elements/Helicopter");

            mySoundEffects.Add(myContent.Load<SoundEffect>("Sounds/Bomb"));

            myStatusPanelLeft.LoadContent(myContent, "Status", "Elements/Panel");
            myStatusPanelCenter.LoadContent(myContent, "Status", "Elements/Panel");
            myStatusPanelRight.LoadContent(myContent, "Status", "Elements/Panel");

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

            // Always update all sea creatures
            foreach (SeaCreatureElement creature in myCreatures)
            {
                creature.Update(aGameTime);
            }

            // Always update boat
            if (myIceberg == null)
            {
                myBoat.Update(Convert.ToInt32(myGraphics.PreferredBackBufferWidth - 60 - myBoat.AccessSize.Width * myBoat.AccessScale), aGameTime);
            }
            else
            {
                myBoat.Update(Convert.ToInt32(myIceberg.AccessPosition.X - 60 - myBoat.AccessSize.Width * myBoat.AccessScale), aGameTime);
            }

            // Check if helicopter is not active and if so, check if we need to activate it because boat if without sinkbombs
            if (myHelicopter.AccessActive == false && myBoat.AccessSinkBombsLeft == 0)
            {
                myHelicopter.SetActive();
            }

            if (myHelicopter.AccessActive)
            {
                if (myHelicopter.AccessHovering && myBoat.MyHitBox.Intersects(myHelicopter.MyHitBox) && myHelicopter.AccessLoadedWeapons == false)
                {
                    myBoat.ReloadMines();
                    myBoatHits = 0;
                    myHelicopter.AccessLoadedWeapons = true;
                }
                myHelicopter.Update(aGameTime);
            }

            // Update all mines that are owned by the game (not released mines are owned by each sub)
            foreach (MineElement mine in myMines.ToList())
            {
                mine.Update(aGameTime);
                // If a mine is within the hitbox for the boat
                if (mine.MyHitBox.Intersects(myBoat.MyHitBox))
                {
                    mySoundEffects[0].Play();
                    myBoatHits++;
                    myBooms.Add(GenerateMyBoom(myContent, 1.0f, myBoat.AccessPosition, aGameTime.TotalGameTime.Seconds + 2));
                    myMines.Remove(mine); // Remove the mine
                }
                // Remove mines that have timed out (3 seconds at surface)
                if (mine.AccessSurfaced && mine.AccessSurfacedTime + 3 <= aGameTime.TotalGameTime.Seconds)
                {
                    myMines.Remove(mine);
                }
            }

            // Update all sinkbombs that are owned by the game (not released sinkbombs are owned by the boat)
            foreach (SinkBombElement sinkBomb in mySinkBombs.ToList())
            {
                sinkBomb.Update(aGameTime);
                foreach (EnemyElement sub in mySubs)
                {
                    // If sinkbomb is within the hitbox for a sub
                    if (sinkBomb.MyHitBox.Intersects(sub.MyHitBox))
                    {
                        mySoundEffects[0].Play();
                        mySubHits++;
                        myBooms.Add(GenerateMyBoom(myContent, 1.0f, sub.AccessPosition, aGameTime.TotalGameTime.Seconds + 2));
                        sub.ResetSub(); // Remove the sub
                        mySinkBombs.Remove(sinkBomb); // Remove the sinkbomb
                    }
                }
                // Remove sinkbombs that are out of view
                if (sinkBomb.AccessPosition.Y > myGraphics.PreferredBackBufferHeight)
                {
                    mySinkBombs.Remove(sinkBomb);
                }
            }

            foreach (Bullet bullet in myBullets.ToList())
            {
                bullet.Update(aGameTime);
                if (myIceberg != null && bullet.MyHitBox.Intersects(myIceberg.MyHitBox))
                {
                    mySoundEffects[0].Play();
                    myBooms.Add(GenerateMyBoom(myContent, 1.0f, myIceberg.AccessPosition, aGameTime.TotalGameTime.Seconds + 2));
                    myBullets.Remove(bullet);
                    myIceberg = null;
                }
                if (bullet.MyHitBox.X > myGraphics.PreferredBackBufferWidth)
                {
                    myBullets.Remove(bullet);
                }
            }

            // Clean up visual booms that has timed out
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
                GenerateContinuousClouds(myContent, myGraphics, gameTime);
            }

            foreach (CloudElement cloud in myClouds)
            {
                cloud.Draw(mySpriteBatch);
            }

            foreach (EnemyElement sub in mySubs)
            {
                sub.Draw(mySpriteBatch);
            }

            foreach (SeaCreatureElement creature in myCreatures)
            {
                creature.Draw(mySpriteBatch);
            }

            myBoat.Draw(mySpriteBatch);
            myHelicopter.Draw(mySpriteBatch);

            foreach (MineElement mine in myMines)
            {
                mine.Draw(mySpriteBatch);
            }

            foreach (SinkBombElement sinkBomb in mySinkBombs)
            {
                sinkBomb.Draw(mySpriteBatch);
            }

            myIceberg?.Draw(mySpriteBatch);

            foreach (Bullet bullet in myBullets)
            {
                bullet.Draw(mySpriteBatch);
            }

            foreach (StaticElement boom in myBooms)
            {
                boom.Draw(mySpriteBatch);
            }

            myOcean.Draw(mySpriteBatch);

            myStatusPanelLeft.Draw(mySpriteBatch, $"Health: {myConfig.Health - myBoatHits} of {myConfig.Health}");
            myStatusPanelCenter.Draw(mySpriteBatch, $"Level: {myConfig.Name}");
            myStatusPanelRight.Draw(mySpriteBatch, $"Sub hits: {mySubHits} of {myConfig.RequiredSubHits}");
        }
        #endregion

        #region Internal methods
        private void SinkBombReleased(SinkBombElement aSinkBomb)
           => mySinkBombs.Add(aSinkBomb);

        private void MineReleased(MineElement aMine)
            => myMines.Add(aMine);

        private void BulletFired(Bullet aBullet)
            => myBullets.Add(aBullet);



        public StaticElement GenerateMyBoom(ContentManager myContent, float aScale, Vector2 aPosition, int aTimeToLive)
        {
            StaticElement boom = new StaticElement(aScale, aPosition, aTimeToLive);
            boom.LoadContent(myContent, "Elements/Boom");
            return boom;
        }

        private void GenerateInitialIceberg(ContentManager myContent)
        {
            myIceberg = new StaticElement(1.0f, new Vector2(0, 0));
            myIceberg.LoadContent(myContent, "Elements/Iceberg");
            int minX = ((myGraphics.PreferredBackBufferWidth / myIceberg.AccessSize.Width) - 2) * myIceberg.AccessSize.Width;
            int maxX = myGraphics.PreferredBackBufferWidth - myIceberg.AccessSize.Width;
            int icebergX = RandomNumber.Between(minX, maxX);
            int icebergY = 130;
            myIceberg.AccessPosition = new Vector2(icebergX, icebergY);
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
                aCloudElement.LoadContent(myContent, new string[] { "Elements/cloud1", "Elements/cloud2", "Elements/cloud3", "Elements/cloud4", "Elements/ShittySeagull" });
                myClouds.Add(aCloudElement);
            }
        }

        private void GenerateContinuousClouds(ContentManager myContent, GraphicsDeviceManager myGraphics, GameTime aGameTime)
        {
            float tempRandomSpeed = RandomNumber.Between(3, 8) / 10.0f;

            myLatestAddedCloud = aGameTime.TotalGameTime.Seconds;
            CloudElement aCloudElement = new CloudElement(0.6f, -1.0f, 0.0f, tempRandomSpeed, new Vector2(myGraphics.PreferredBackBufferWidth, RandomNumber.Between(1, 50)), myGraphics);
            aCloudElement.LoadContent(myContent, new string[] { "Elements/cloud1", "Elements/cloud2", "Elements/cloud3", "Elements/cloud4", "Elements/ShittySeagull" });
            myClouds.Add(aCloudElement);
            foreach (CloudElement cloud in myClouds.Where(s => s.AccessOutOfBounds).ToList())
            {
                myClouds.Remove(cloud);
            }
        }
        #endregion
    }
}
